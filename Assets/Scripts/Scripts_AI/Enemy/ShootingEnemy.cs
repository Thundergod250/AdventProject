using UnityEngine;

public class ShootingEnemy : NewEnemyMovement
{
    [SerializeField] private float stopDistance = 5f;   // Distance to stop and shoot
    [SerializeField] private float retreatDistance = 2f; // Distance to back away
    [SerializeField] private float retreatSpeed = 2f;   // Speed when retreating

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
        // Placeholder for shooting logic
        Debug.Log("Enemy shooting at player!");
        // You can instantiate bullets/projectiles here
    }
}
