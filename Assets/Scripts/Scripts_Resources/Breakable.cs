using System;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [Header("Breakable Settings")]
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private float explosionLifetime = 1f;
    [SerializeField] private GameObject resourceDropped;

    private Health health;
    
    public void HandleDeath()
    {
        // Spawn VFX
        if (explosionVFX != null)
        {
            GameObject vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(vfx, explosionLifetime);
        }

        // Drop resource
        if (resourceDropped != null) 
            Instantiate(resourceDropped, transform.position, Quaternion.identity);

        // Destroy this breakable object
        Destroy(gameObject);
    }
}