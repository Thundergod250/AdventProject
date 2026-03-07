using UnityEngine;

public class NewEnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f; // enemy movement speed

    [Header("Enemy Model")]
    [SerializeField] public Transform model;
    public Vector3 lastDirection;

    public Transform playerTransform;
    private Health playerHealth;

    private void Start()
    {
        // Get player reference from MiningManager through GameManager
        if (GameManager.Instance != null && GameManager.Instance.MiningManager != null)
        {
            GameObject playerObj = GameManager.Instance.MiningManager.Player; // Player reference in MiningManager
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
                playerHealth = playerObj.GetComponent<Health>();
            }
        }
    }

    private void Update()
    {
        MoveToPlayer();
    }

    protected virtual void MoveToPlayer()
    {
        if (playerTransform != null)
        {
            // Move towards the player
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            Debug.LogWarning($"direction: {direction}");

            // Store last direction if moving
            if (direction.sqrMagnitude > 0.001f)
            {
                lastDirection = direction;
            }

            // Rotate the model to face movement direction
            if (lastDirection != Vector3.zero)
                model.rotation = Quaternion.LookRotation(lastDirection, Vector3.up);
        }
    }

    protected virtual void DamagePlayer()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(2); // call player's Health script
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the trigger was the player
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            DamagePlayer();
        }
    }
}
