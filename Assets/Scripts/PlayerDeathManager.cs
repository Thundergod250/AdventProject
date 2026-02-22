using UnityEngine;
using System.Collections;

public class PlayerDeathManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject SpawnPoint;
    
    public void CallDeathAndRespawnRoutine() => StartCoroutine(DeathAndRespawnRoutine());
    
    private void HandlePlayerDeath()
    {
        Player.gameObject.SetActive(false);
        Player.GetComponent<PlayerManipulator>()._DisableAllMovement();

        GameManager.Instance.MiningManager.StopMining();

        Debug.Log("Player has been disabled after trigger collision.");
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
        HandlePlayerDeath();
        yield return new WaitForSeconds(3f);
        Debug.Log("HandlePlayerRespawn");
        HandlePlayerRespawn();
    }
}
