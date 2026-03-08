using UnityEngine;

public class HeadButtingEnemy : NewEnemyMovement
{
    [Header("Headbutting Settings")]
    [SerializeField] private float retreatDistance = 5f;   // how far to run away
    [SerializeField] private float retreatSpeed = 8f;      // speed while retreating
    [SerializeField] private float chaseResumeDelay = 1f;  // pause before chasing again

    private bool isRetreating = false;
    private float retreatTimer = 0f;

    protected override void MoveToPlayer()
    {
        if (playerTransform == null) return;

        if (isRetreating)
        {
            RetreatFromPlayer();
        }
        else
        {
            base.MoveToPlayer(); // normal chase behavior
        }
    }

    private void RetreatFromPlayer()
    {
        // Move away from player
        Vector3 direction = (transform.position - playerTransform.position).normalized;
        transform.position += direction * retreatSpeed * Time.deltaTime;

        // Track distance
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance >= retreatDistance)
        {
            // Start timer before resuming chase
            retreatTimer += Time.deltaTime;
            if (retreatTimer >= chaseResumeDelay)
            {
                isRetreating = false;
                retreatTimer = 0f;
            }
        }

        // Rotate model to face retreat direction
        if (direction.sqrMagnitude > 0.001f)
        {
            lastDirection = direction;
            model.rotation = Quaternion.LookRotation(lastDirection, Vector3.up);
        }
    }

    protected override void DamagePlayer()
    {
        Debug.LogWarning("DamagePlayer");
        //base.DamagePlayer();

        //isRetreating = true;
    }
}
