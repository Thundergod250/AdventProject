using UnityEngine;
using System.Collections;

public class PlayerDeathManager : MonoBehaviour
{
    [Header("References")]
    public GameObject Player;
    public GameObject SpawnPoint;

    [Header("Death Settings")]
    [SerializeField] private float lavaHeightThreshold = -3f; //OCEAN HEIGHT
    [SerializeField] private float respawnDelay = 3f;

    private bool isRespawning = false;

    public void CallDeathAndRespawnRoutine() => StartCoroutine(DeathAndRespawnRoutine());

    private void Update()
    {
        // Continuously check if player has fallen below threshold
        if (!isRespawning && Player != null && Player.transform.position.y < lavaHeightThreshold)
        {
            Debug.Log("Player fell below lava threshold.");
            StartCoroutine(DeathAndRespawnRoutine());
        }
    }

    private void HandlePlayerDeath()
    {
        Player.gameObject.SetActive(false);
        Player.GetComponent<PlayerManipulator>()._DisableAllMovement();

        GameManager.Instance.MiningManager.StopMining();

        Debug.Log("Player has been disabled after trigger collision or lava fall.");
    }

    private void HandlePlayerRespawn()
    {
        if (SpawnPoint != null)
        {
            Player.transform.position = SpawnPoint.transform.position;
            Player.transform.rotation = SpawnPoint.transform.rotation;
        }

        Player.gameObject.SetActive(true);
        Player.GetComponent<PlayerManipulator>()._EnableAllMovement();
        GameManager.Instance.GoldManager.ReduceGoldPercentage(0.5f);

        Debug.Log("Player has respawned and all movement re-enabled.");
    }

    private IEnumerator DeathAndRespawnRoutine()
    {
        isRespawning = true;
        HandlePlayerDeath();
        yield return new WaitForSeconds(respawnDelay);
        Debug.Log("HandlePlayerRespawn");
        HandlePlayerRespawn();
        isRespawning = false;
    }
}