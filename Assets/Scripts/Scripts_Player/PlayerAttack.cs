using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAnimation playerAnimation;

    [Header("Attack Settings")]
    [SerializeField] private float slamRadius = 3f;       // area of effect
    [SerializeField] private int slamDamage = 25;         // editable/upgradable damage
    [SerializeField] private LayerMask damageMask;        // filter for enemies/harvestables

    private bool isAttacking = false;

    public void OnSlam(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (isAttacking) return; // prevent re-entry
        if (!playerMovement.IsGrounded()) return; // only attack if grounded

        playerMovement.SetCanMove(false);
        StartCoroutine(SlamRoutine());
    }

    private IEnumerator SlamRoutine()
    {
        isAttacking = true;

        // Play slam animation coroutine
        yield return StartCoroutine(playerAnimation.PlaySlam());

        // ✅ Apply damage after slam lands
        ApplySlamDamage();

        // Re-enable movement after slam finishes
        playerMovement.SetCanMove(true);
        isAttacking = false;
    }

    private void ApplySlamDamage()
    {
        // Find all colliders in radius
        Collider[] hits = Physics.OverlapSphere(transform.position, slamRadius, damageMask);

        foreach (var hit in hits)
        {
            Health health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(slamDamage);
            }
        }

        Debug.Log($"Slam hit {hits.Length} objects for {slamDamage} damage.");
    }

    // Optional: visualize slam radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, slamRadius);
    }
}