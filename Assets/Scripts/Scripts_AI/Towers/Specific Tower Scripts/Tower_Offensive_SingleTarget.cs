using UnityEngine;

public class Tower_Offensive_SingleTarget : TowerOffensiveBase
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 12f; // 👈 bullet speed set here

    [Header("Tower Stats")]
    public int towerDamageLevel = 1;
    public int towerDamage = 10;
    public int towerFireRateLevel = 1; 
    public float towerFireRate = 1f;
    public float detectionRadius = 10f;

    private float fireCooldown;

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            TryFireAtTarget();
            fireCooldown = towerFireRate;
        }
    }

    private void TryFireAtTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider hit in hits)
        {
            EnemyBase enemy = hit.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                Debug.Log(enemy);

                FireProjectile(enemy.transform);
                break; // Only fire at one target
            }
        }
    }

    private void FireProjectile(Transform target)
    {
        if (projectilePrefab == null || target == null) return;

        // 👇 Spawn via ObjectPooling through GameManager
        GameObject proj = GameManager.Instance.SpawnObject(
            projectilePrefab,
            null,
            firePoint.position,
            Quaternion.identity
        );

        ProjectileBase temp = proj.GetComponent<ProjectileBase>();
        if (temp != null)
        {
            temp.Initialize(target, towerDamage, projectileSpeed, projectilePrefab, ProjectileBase.ProjectileOwnerType.Tower); 
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}