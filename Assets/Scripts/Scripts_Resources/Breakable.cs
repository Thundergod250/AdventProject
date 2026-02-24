using System;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [Header("Breakable Settings")]
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private float explosionLifetime = 1f;

    [Header("Resource Drop")]
    [SerializeField] private GameObject resourcePrefab; // single prefab
    private int resourceLevel = 0;

    private Health health;

    public void HandleDeath()
    {
        // Spawn VFX
        if (explosionVFX != null)
        {
            GameObject vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(vfx, explosionLifetime);
        }

        // Drop resources using SetResourceLevel
        SpawnResourceAmount(resourceLevel);

        // Destroy this breakable object
        Destroy(gameObject);
    }

    public void GiveGems()
    {
        GameManager.Instance.GoldManager.AddGold(2);
    }

    // Function to spawn resources based on level
    public void SpawnResourceAmount(int level)
    {
        resourceLevel = level;

        if (resourcePrefab != null)
        {
            for (int i = 0; i < resourceLevel; i++)
            {
                Instantiate(resourcePrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
