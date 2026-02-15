using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SapGuardian : MonoBehaviour
{
    private enum BossState { Targeting, Attacking, Paused, Recenter, Stunned, Dead }

    [Header("Boss Reference")]
    [SerializeField] private Transform appearance; 
    [SerializeField] private UI_BossHealth bossHealth;

    [Header("Appearance Sets")]
    [SerializeField] private GameObject[] bossBodyPartSet1; // corrupted
    [SerializeField] private GameObject[] bossBodyPartSet2; // purified

    [Header("Bobbing Settings")]
    [SerializeField] private float bobHeight = 0.5f;
    [SerializeField] private float bobSpeed = 2f;

    [Header("Target Reference")]
    [SerializeField] private Transform player;

    [Header("Arena Settings")]
    [SerializeField] private Transform arenaCenter;       
    [SerializeField] private float maxDistanceFromCenter = 20f;

    [Header("State Durations")]
    [SerializeField] private float targetingDuration = 3f;
    [SerializeField] private float attackingDuration = 2f;
    [SerializeField] private float pauseDuration = 1f;
    [SerializeField] private float recenterDuration = 2f;
    [SerializeField] private float stunnedDuration = 3f;

    [Header("Target Settings")] 
    [SerializeField] private List<Faction> attackableFactions;
    
    [Header("Attack Settings")]
    [SerializeField] private float dashSpeed = 10f;

    private Health health;
    private Collider thisBoxCollider;
    private Vector3 basePosition;
    private BossState currentState = BossState.Targeting;
    private Vector3 dashDirection;
    private bool isDashing = false;

    private void OnEnable()
    {
        if (appearance == null || player == null || arenaCenter == null)
        {
            Debug.LogError("Boss appearance, player, or arenaCenter not assigned!");
            return;
        }
        
        health = GetComponent<Health>();
        thisBoxCollider = GetComponent<Collider>();

        bossHealth.OnActivate(health);

        health.OnDeath.AddListener(OnDeath);

        basePosition = appearance.localPosition;
        StartCoroutine(BossLoop());
    }

    private void Update()
    {
        if (currentState == BossState.Targeting)
        {
            HandleTargetingBobbing();
            FacePlayer();
        }
        else if (currentState == BossState.Attacking)
        {
            HandleAggressiveBobbing();
            if (isDashing) HandleDash();
        }
        // Stunned/Dead → no movement, no bobbing
    }
    
    // === Collision ===
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HittingBoulder>())
        {
            StopAllCoroutines();
            isDashing = false;
            if (currentState != BossState.Dead)
                StartCoroutine(Stunned());
            return;
        }

        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth && attackableFactions.Contains(targetHealth.GetFaction()))
            targetHealth.TakeDamage(1);
    }

    // === Bobbing ===
    private void HandleTargetingBobbing()
    {
        float offset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        appearance.localPosition = basePosition + Vector3.up * offset;
    }

    private void HandleAggressiveBobbing()
    {
        float offset = Mathf.Sin(Time.time * (bobSpeed * 3f)) * (bobHeight * 2f);
        appearance.localPosition = basePosition + Vector3.up * offset;
    }

    // === Facing ===
    private void FacePlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 5f
            );
        }
    }

    private void FaceDashDirection()
    {
        if (dashDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dashDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 10f
            );
        }
    }

    // === Dash ===
    private void HandleDash()
    {
        FaceDashDirection();
        Vector3 pos = transform.position;
        pos += dashDirection * (dashSpeed * Time.deltaTime);
        pos.y = arenaCenter.position.y;
        transform.position = pos;

        float dist = Vector3.Distance(new Vector3(transform.position.x, arenaCenter.position.y, transform.position.z), arenaCenter.position);
        if (dist > maxDistanceFromCenter)
        {
            Debug.Log("Boss went out of bounds → recentering.");
            isDashing = false;
            StopAllCoroutines();
            if (currentState != BossState.Dead)
                StartCoroutine(Recenter());
        }
    }

    // === State Machine ===
    private IEnumerator BossLoop()
    {
        while (currentState != BossState.Dead)
        {
            currentState = BossState.Targeting;
            yield return new WaitForSeconds(targetingDuration);

            currentState = BossState.Attacking;
            dashDirection = (player.position - transform.position);
            dashDirection.y = 0f;
            dashDirection.Normalize();
            isDashing = true;
            yield return new WaitForSeconds(attackingDuration);
            isDashing = false;

            currentState = BossState.Paused;
            yield return new WaitForSeconds(pauseDuration);
        }
    }

    // === Recenter ===
    private IEnumerator Recenter()
    {
        currentState = BossState.Recenter;
        Vector3 start = transform.position;
        Vector3 end = arenaCenter.position;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime / recenterDuration;
            Vector3 pos = Vector3.Lerp(start, end, t);
            pos.y = arenaCenter.position.y;
            transform.position = pos;
            yield return null;
        }

        if (thisBoxCollider) thisBoxCollider.enabled = true;

        // Only restart loop if not dead
        if (currentState != BossState.Dead)
            StartCoroutine(BossLoop());
    }

    // === Stunned ===
    private IEnumerator Stunned()
    {
        currentState = BossState.Stunned;
        health.SetDamageMode(DamageMode.Normal);

        Debug.Log("Boss stunned!");

        Quaternion startRot = transform.localRotation;
        Quaternion fallenRot = Quaternion.Euler(-90f, startRot.eulerAngles.y, startRot.eulerAngles.z);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.5f;
            transform.localRotation = Quaternion.Slerp(startRot, fallenRot, t);
            yield return null;
        }

        yield return new WaitForSeconds(stunnedDuration);

        if (thisBoxCollider) thisBoxCollider.enabled = false;

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.5f;
            transform.localRotation = Quaternion.Slerp(fallenRot, Quaternion.identity, t);
            yield return null;
        }

        health.SetDamageMode(DamageMode.Fortified);

        if (currentState != BossState.Dead)
            StartCoroutine(Recenter());
    }
    
    private void OnDeath()
    {
        Debug.Log("Boss defeated!");
        StopAllCoroutines();
        currentState = BossState.Dead;

        if (thisBoxCollider) thisBoxCollider.enabled = false;

        StartCoroutine(DeathSequence());
    }


    private IEnumerator DeathSequence()
    {
        // ✅ Always stand upright with X=0, Y=-220
        Quaternion startRot = transform.localRotation;
        Quaternion targetRot = Quaternion.Euler(0f, -220f, 0f);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.5f; // stand-up duration
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        // Now recenter
        yield return StartCoroutine(RecenterOnDeath());

        // Swap appearances
        foreach (var part in bossBodyPartSet1)
            if (part) part.SetActive(false);

        foreach (var part in bossBodyPartSet2)
            if (part) part.SetActive(true);

        Debug.Log("Boss transformed to purified form.");
    }

    
    private IEnumerator RecenterOnDeath()
    {
        Vector3 start = transform.position;
        Vector3 end = arenaCenter.position;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime / recenterDuration;
            Vector3 pos = Vector3.Lerp(start, end, t);
            pos.y = arenaCenter.position.y;
            transform.position = pos;
            yield return null;
        }

        // ✅ No restart of BossLoop here
    }
}
