using UnityEngine;

public class NewEnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 3f; // enemy movement speed

    private Transform playerTransform;
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
        if (playerTransform != null)
        {
            // Move towards the player
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Optional: rotate to face the player
            transform.LookAt(playerTransform);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if collided with player
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(2); // call player's Health script
                Debug.Log("Enemy collided with player. Damage applied.");
            }
        }
    }
}
