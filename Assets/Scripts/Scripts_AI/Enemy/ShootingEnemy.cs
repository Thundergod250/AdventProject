using UnityEngine;

public class ShootingEnemy : NewEnemyMovement
{
    [SerializeField] private float stopDistance = 8f;   // Distance to stop and shoot
    [SerializeField] private float retreatDistance = 6f; // Distance to back away
    [SerializeField] private float retreatSpeed = 2f;   // Speed when retreating

    [SerializeField] private GameObject projectilePrefab; // Assign in Inspector
    [SerializeField] private Transform firePoint;         // Where the projectile spawns
    [SerializeField] private float fireCooldown = 3f;     // Time between shots

    private float lastFireTime;

    protected override void MoveToPlayer()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        if (distance > stopDistance)
        {
            // Chase the player
            transform.position += direction * speed * Time.deltaTime;
        }
        else if (distance <= stopDistance && distance > retreatDistance)
        {
            // Stop and shoot
            ShootAtPlayer();
        }
        else if (distance <= retreatDistance)
        {
            // Back away slowly
            transform.position -= direction * retreatSpeed * Time.deltaTime;
        }

        // Always face the player
        transform.LookAt(playerTransform);
    }

    private void ShootAtPlayer()
    {
        if (Time.time - lastFireTime >= fireCooldown)
        {
            lastFireTime = Time.time;

            // Spawn projectile
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Initialize projectile direction
            ProjectileBase pb = projectile.GetComponent<ProjectileBase>();
            if (pb != null)
            {
                pb.SetDirection(firePoint.forward); // Pass the enemy's facing direction
            }
        }
    }
}
