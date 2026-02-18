using UnityEngine;
using System.Collections;

public class PlayerDeathManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject SpawnPoint;

    // Handles player death logic
    public void HandlePlayerDeath()
    {
        // Disable the player
        Player.gameObject.SetActive(false);

        // Disable all movement via PlayerManipulator on Player
        Player.GetComponent<PlayerManipulator>()._DisableAllMovement();

        GameManager.Instance.MiningManager.StopMining();

        Debug.Log("Player has been disabled after trigger collision.");
    }

    // Handles player respawn logic
    public void HandlePlayerRespawn()
    {
        // Move player to spawn point if assigned
        if (SpawnPoint != null)
        {
            Player.transform.position = SpawnPoint.transform.position;
            Player.transform.rotation = SpawnPoint.transform.rotation;
        }

        // Re-enable the player
        Player.gameObject.SetActive(true);

        // Enable all movement via PlayerManipulator on Player
        Player.GetComponent<PlayerManipulator>()._EnableAllMovement();

        GameManager.Instance.GoldManager.DivideGold(2);

        Debug.Log("Player has respawned and all movement re-enabled.");
    }

    public void CallDeathAndRespawnRoutine()
    {
        StartCoroutine(DeathAndRespawnRoutine());
    }

    // Coroutine to handle death and respawn sequence
    public IEnumerator DeathAndRespawnRoutine()
    {
        // Call death handler
        HandlePlayerDeath();

        // Wait for the specified delay
        yield return new WaitForSeconds(3f);

        Debug.Log("HandlePlayerRespawn");
        // Call respawn handler
        HandlePlayerRespawn();
    }
}
