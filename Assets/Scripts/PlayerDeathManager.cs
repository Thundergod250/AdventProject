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

        // Stop camera movement through your GameManager reference
        if (GameManager.Instance != null && GameManager.Instance.FreeLookCamControl != null)
        {
            GameManager.Instance.FreeLookCamControl.DisableCameraMovement();
        }

        Debug.Log("Player has been disabled after trigger collision.");
    }

    // Handles player respawn logic
    public void HandlePlayerRespawn()
    {
        // Re-enable the player
        Player.gameObject.SetActive(true);

        // Move player to spawn point if assigned
        if (SpawnPoint != null)
        {
            Player.transform.position = SpawnPoint.transform.position;
            Player.transform.rotation = SpawnPoint.transform.rotation;
        }

        // Enable camera movement through your GameManager reference
        if (GameManager.Instance != null && GameManager.Instance.FreeLookCamControl != null)
        {
            GameManager.Instance.FreeLookCamControl.EnableCameraMovement();
        }

        GameManager.Instance.GoldManager.DivideGold(2);

        Debug.Log("Player has respawned and camera movement re-enabled.");
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
