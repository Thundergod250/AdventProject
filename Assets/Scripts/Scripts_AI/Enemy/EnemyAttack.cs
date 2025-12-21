using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public enum AttackMode
    {
        None,           // docile, no attacking
        DetectAndAttack // actively searching for towers
    }

    public enum AttackType
    {
        Melee,
        Ranged
    }

    [Header("Attack Settings")]
    [SerializeField] private AttackMode startMode = AttackMode.None;
    [SerializeField] private AttackType attackType = AttackType.Melee;
    [SerializeField] private float detectionRadius = 8f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackInterval = 1.5f;
    [SerializeField] private int attackDamage = 10;

    [Header("Projectile Settings (Ranged Only)")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;

    private float attackTimer;
    private Transform currentTarget;
    private EnemyBase enemyBase;
    private AttackMode currentMode;

    private void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
        currentMode = startMode;
    }

    private void Update()
    {
        if (currentMode != AttackMode.DetectAndAttack) return;

        DetectTargets();

        if (currentTarget != null)
        {
            float dist = Vector3.Distance(transform.position, currentTarget.position);

            if (attackType == AttackType.Melee)
            {
                if (dist > attackRange)
                {
                    enemyBase.Movement.FollowTarget(currentTarget);
                }
                else
                {
                    attackTimer -= Time.deltaTime;
                    if (attackTimer <= 0f)
                    {
                        AttackMelee();
                        attackTimer = attackInterval;
                    }
                }
            }
            else if (attackType == AttackType.Ranged)
            {
                if (dist > attackRange)
                {
                    enemyBase.Movement.FollowTarget(currentTarget);
                }
                else
                {
                    enemyBase.Movement.SetMode(EnemyMovement.MovementMode.Idle);

                    attackTimer -= Time.deltaTime;
                    if (attackTimer <= 0f)
                    {
                        AttackRanged();
                        attackTimer = attackInterval;
                    }
                }
            }
        }
    }

    // -------------------------
    // Mode Switching
    // -------------------------
    public void SetAttackMode(AttackMode mode)
    {
        currentMode = mode;
        if (mode == AttackMode.None)
        {
            currentTarget = null;
        }
    }

    // -------------------------
    // Detection
    // -------------------------
    private void DetectTargets()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

        currentTarget = null;
        float closestDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            TowerController tower = hit.GetComponent<TowerController>();
            if (tower != null)
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    currentTarget = hit.transform;
                }
            }
        }
    }

    // -------------------------
    // Attack Behaviors
    // -------------------------
    private void AttackMelee()
    {
        if (currentTarget == null) return;

        Health towerHealth = currentTarget.GetComponent<Health>();
        if (towerHealth != null)
        {
            towerHealth.TakeDamage(attackDamage);
            Debug.Log($"{gameObject.name} melee attacked {currentTarget.name} for {attackDamage} damage.");
        }
    }

    private void AttackRanged()
    {
        if (currentTarget == null || projectilePrefab == null) return;

        // Get the tower controller and its attack location
        TowerController tower = currentTarget.GetComponent<TowerController>();
        Transform targetPoint = currentTarget; // fallback
        if (tower != null && tower.GetAttackLocation() != null)
        {
            targetPoint = tower.GetAttackLocation();
        }

        // Spawn projectile
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Initialize projectile toward the attack location
        ProjectileBase projectile = proj.GetComponent<ProjectileBase>();
        if (projectile != null)
        {
            projectile.Initialize(
                targetPoint,
                attackDamage,
                projectileSpeed,
                projectilePrefab,
                ProjectileBase.ProjectileOwnerType.Enemy
            );
        }

        Debug.Log($"{gameObject.name} fired a projectile at {targetPoint.name}.");
    }

    // -------------------------
    // Gizmos (Scene Visualization)
    // -------------------------
    private void OnDrawGizmosSelected()
    {
        // Detection radius (yellow)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Attack range (red)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
