using UnityEngine;

public class PlayerMonolithInteraction : MonoBehaviour
{
    private MonolithManager monolithManager;

    private void Awake()
    {
        monolithManager = GameManager.Instance.MonolithManager;
    }

    // Call this when player interacts with a monolith
    public void InteractWithMonolith(Monolith monolith)
    {
        if (monolith != null && monolithManager != null)
        {
            Transform spawnPoint = monolith.GetSpawnPoint();
            monolithManager.UpdateSpawnPoint(spawnPoint);
            Debug.Log("Player interacted with monolith. Spawn point updated.");
        }
    }
}
