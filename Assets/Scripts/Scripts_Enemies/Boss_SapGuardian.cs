using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SapGuardian : MonoBehaviour
{
    private enum BossState { Targeting, Attacking, Paused, Recenter, Stunned }

    [Header("Boss Reference")]
    [SerializeField] private Transform appearance; // mesh only, not colliders
    [SerializeField] private UI_BossHealth bossHealth;
    

    [Header("Bobbing Settings")]
    [SerializeField] private float bobHeight = 0.5f;
    [SerializeField] private float bobSpeed = 2f;

    [Header("Target Reference")]
    [SerializeField] private Transform player;

    [Header("Arena Settings")]
    [SerializeField] private Transform arenaCenter;       // empty object at arena center
    [SerializeField] private float maxDistanceFromCenter = 20f; // limit radius

    [Header("State Durations")]
    [SerializeField] private float targetingDuration = 3f; // build-up time
    [SerializeField] private float attackingDuration = 2f; // dash time
    [SerializeField] private float pauseDuration = 1f;     // recovery time
    [SerializeField] private float recenterDuration = 2f;  // time to move back
    [SerializeField] private float stunnedDuration = 3f;   // immobilized time

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
        // Stunned state → no movement, no bobbing
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
        direction.y = 0f; // ✅ horizontal only

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
        pos.y = arenaCenter.position.y; // ✅ keep grounded
        transform.position = pos;

        // Check arena limit
        float dist = Vector3.Distance(new Vector3(transform.position.x, arenaCenter.position.y, transform.position.z), arenaCenter.position);
        if (dist > maxDistanceFromCenter)
        {
            Debug.Log("Boss went out of bounds → recentering.");
            isDashing = false;
            StopAllCoroutines();
            StartCoroutine(Recenter());
        }
    }

    // === State Machine ===
    private IEnumerator BossLoop()
    {
        while (true)
        {
            // Targeting
            currentState = BossState.Targeting;
            yield return new WaitForSeconds(targetingDuration);

            // Attacking
            currentState = BossState.Attacking;
            dashDirection = (player.position - transform.position);
            dashDirection.y = 0f; // ✅ horizontal only
            dashDirection.Normalize();
            isDashing = true;
            yield return new WaitForSeconds(attackingDuration);
            isDashing = false;

            // Pause
            currentState = BossState.Paused;
            yield return new WaitForSeconds(pauseDuration);
        }
    }

    // === Recenter ===
    private IEnumerator Recenter()
    {
        currentState = BossState.Recenter;
        Vector3 start = transform.position;
        Vector3 end = new Vector3(arenaCenter.position.x, arenaCenter.position.y, arenaCenter.position.z);
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime / recenterDuration;
            Vector3 pos = Vector3.Lerp(start, end, t);
            pos.y = arenaCenter.position.y; // ✅ lock Y
            transform.position = pos;
            yield return null;
        }

        // After recentering, resume loop
        if (thisBoxCollider) thisBoxCollider.enabled = true; // ✅ restore collision
        StartCoroutine(BossLoop());

    }

    // === Stunned ===
    private IEnumerator Stunned()
    {
        currentState = BossState.Stunned;
        health.SetDamageMode(DamageMode.Normal);

        Debug.Log("Boss stunned!");

        // Smoothly fall over
        Quaternion startRot = transform.localRotation;
        Quaternion fallenRot = Quaternion.Euler(-90f, startRot.eulerAngles.y, startRot.eulerAngles.z);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.5f; // fall duration
            transform.localRotation = Quaternion.Slerp(startRot, fallenRot, t);
            yield return null;
        }

        // Stay stunned
        yield return new WaitForSeconds(stunnedDuration);

        // Disable collider before standing up (immunity)
        if (thisBoxCollider) thisBoxCollider.enabled = false;

        // Smoothly stand back up
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.5f; // stand-up duration
            transform.localRotation = Quaternion.Slerp(fallenRot, Quaternion.identity, t);
            yield return null;
        }

        health.SetDamageMode(DamageMode.Fortified);

        // Resume by recentering
        StartCoroutine(Recenter());
    }



    // === Collision ===
    private void OnTriggerEnter(Collider other)
    {
        // If hit by a boulder → stunned
        if (other.GetComponent<HittingBoulder>())
        {
            StopAllCoroutines();
            isDashing = false;
            StartCoroutine(Stunned());
            return;
        }

        // Otherwise damage player/targets
        Health health = other.GetComponent<Health>();
        if (health && attackableFactions.Contains(health.GetFaction()))
            health.TakeDamage(1);
    }
}
