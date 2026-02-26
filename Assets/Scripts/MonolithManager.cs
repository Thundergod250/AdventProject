using UnityEngine;

public class MonolithManager : MonoBehaviour
{
    private Transform currentSpawnPoint;

    public void UpdateSpawnPoint(Transform newSpawnPoint)
    {
        currentSpawnPoint = newSpawnPoint;

        // Update PlayerDeathManager with the new spawn point
        PlayerDeathManager deathManager = GameManager.Instance.GetComponent<PlayerDeathManager>();
        if (deathManager != null)
        {
            deathManager.SpawnPoint = currentSpawnPoint.gameObject;
            Debug.Log("PlayerDeathManager spawn point updated to latest monolith.");
        }
    }

    public Transform GetCurrentSpawnPoint()
    {
        return currentSpawnPoint;
    }
}
