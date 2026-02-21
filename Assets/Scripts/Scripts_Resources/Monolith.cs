using UnityEngine;

public class Monolith : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint; // assign in Inspector

    // Call this when the player interacts with the monolith
    public void SetAsRespawnPoint()
    {
        if (GameManager.Instance != null && GameManager.Instance.PlayerDeathManager != null)
        {
            GameManager.Instance.PlayerDeathManager.SpawnPoint = spawnPoint.gameObject;
            Debug.Log("PlayerDeathManager spawn point updated to this monolith.");
        }
        else
        {
            Debug.LogWarning("GameManager or PlayerDeathManager not found!");
        }
    }

    // Optional getter if you still want to retrieve the spawn point directly
    public Transform GetSpawnPoint()
    {
        return spawnPoint;
    }
}
