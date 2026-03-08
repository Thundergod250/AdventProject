using System;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [Header("Breakable Settings")]
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private float explosionLifetime = 1f;

    [Header("Resource Drop")]
    [SerializeField] private GameObject resourcePrefab; // single prefab
    public int GemsDropped = 0;

    private Health health;

    public void HandleDeath()
    {
        // Spawn VFX
        if (explosionVFX != null)
        {
            GameObject vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(vfx, explosionLifetime);
        }

        GiveGems(GemsDropped);

        // Destroy this breakable object
        Destroy(gameObject);
    }

    public void GiveGems(int gemAmount)
    {
        GameManager.Instance.GoldManager.AddGold(gemAmount);
    }
}
