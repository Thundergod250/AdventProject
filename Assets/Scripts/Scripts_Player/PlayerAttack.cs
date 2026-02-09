using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float slamRadius = 3f;   // area of effect
    [SerializeField] private int slamDamage = 25;     // editable/upgradable damage
    [SerializeField] private LayerMask damageMask;    // filter for enemies/harvestables

    private PlayerMovement playerMovement;
    private PlayerAnimation playerAnimation;
    private bool isAttacking = false;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    public void OnSlam(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (isAttacking) return; 
        if (!playerMovement.IsGrounded()) return; 

        playerMovement.SetCanMove(false);
        StartCoroutine(SlamRoutine());
    }

    private IEnumerator SlamRoutine()
    {
        isAttacking = true;

        yield return StartCoroutine(playerAnimation.PlaySlam());

        ApplySlamDamage();

        playerMovement.SetCanMove(true);
        isAttacking = false;
    }

    private void ApplySlamDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, slamRadius, damageMask);

        foreach (var hit in hits)
        {
            Health health = hit.GetComponent<Health>();
            if (health) 
                health.TakeDamage(slamDamage);
        }

        Debug.Log($"Slam hit {hits.Length} objects for {slamDamage} damage.");
    }

    // ✅ Called by PlayerStats to sync values
    public void SetAttackValues(int damage, int radius)
    {
        slamDamage = damage;
        slamRadius = radius;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, slamRadius);
    }
}
