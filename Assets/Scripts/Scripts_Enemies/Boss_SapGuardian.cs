using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SapGuardian : MonoBehaviour
{
    private enum BossState { Targeting, Attacking, Paused, Recenter }

    [Header("Appearance Reference")]
    [SerializeField] private Transform appearance; // mesh only, not colliders

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

    [Header("Target Settings")] 
    [SerializeField] private List<Faction> attackableFactions;
    
    [Header("Attack Settings")]
    [SerializeField] private float dashSpeed = 10f;

    private Vector3 basePosition;
    private BossState currentState = BossState.Targeting;
    private Vector3 dashDirection;
    private bool isDashing = false;

    private void Start()
    {
        if (appearance == null || player == null || arenaCenter == null)
        {
            Debug.LogError("Boss appearance, player, or arenaCenter not assigned!");
            return;
        }

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
        direction.y = 0;

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
        transform.position += dashDirection * (dashSpeed * Time.deltaTime);

        // Check arena limit
        float dist = Vector3.Distance(transform.position, arenaCenter.position);
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
            dashDirection = (player.position - transform.position).normalized; // snapshot direction
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
        Vector3 end = arenaCenter.position;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime / recenterDuration;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        // After recentering, resume loop
        StartCoroutine(BossLoop());
    }

    // === Collision with Player ===
    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health && attackableFactions.Contains(health.GetFaction()))
            health.TakeDamage(1);
    }
}
