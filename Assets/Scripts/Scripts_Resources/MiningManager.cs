using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiningManager : MonoBehaviour
{
    [Header("Prefabs & References")]
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private GameObject blockingWall;

    [Header("Spawn Settings")]
    [SerializeField] private Vector2 planeSize = new Vector2(10f, 10f); // X/Z area
    [SerializeField] private int rockCount = 10; // how many rocks to spawn
    [SerializeField] private float groundY = 0f; // fixed ground height

    [Header("Game Settings")]
    [SerializeField] private float miningDuration = 10f; // seconds
    private bool isMiningActive = false;

    private List<GameObject> spawnedRocks = new List<GameObject>();
    private Coroutine miningRoutine;

    // === Public Entry Point ===
    public void StartMining()
    {
        if (isMiningActive) return; // prevent re-entry while active
        miningRoutine = StartCoroutine(MiningSession());
    }

    // === Core Mining Flow ===
    private IEnumerator MiningSession()
    {
        isMiningActive = true;

        // Enable wall
        if (blockingWall != null)
            blockingWall.SetActive(true);

        // Spawn rocks
        for (int i = 0; i < rockCount; i++)
        {
            Vector3 spawnPos = GetRandomGroundPosition();
            GameObject rock = Instantiate(rockPrefab, spawnPos, Quaternion.identity);
            spawnedRocks.Add(rock);
        }

        // Wait for duration
        yield return new WaitForSeconds(miningDuration);

        // Cleanup
        foreach (var rock in spawnedRocks)
        {
            if (rock != null)
                Destroy(rock);
        }
        spawnedRocks.Clear();

        if (blockingWall != null)
            blockingWall.SetActive(false);

        isMiningActive = false;
        miningRoutine = null;
    }

    // === Helpers ===
    private Vector3 GetRandomGroundPosition()
    {
        float x = Random.Range(-planeSize.x * 0.5f, planeSize.x * 0.5f);
        float z = Random.Range(-planeSize.y * 0.5f, planeSize.y * 0.5f);

        // Offset by MiningManager's transform position
        return transform.position + new Vector3(x, groundY, z);
    }


    // === Gizmos ===
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 center = transform.position + new Vector3(0f, groundY, 0f);
        Vector3 size = new Vector3(planeSize.x, 0.1f, planeSize.y);
        Gizmos.DrawWireCube(center, size);
    }
}
