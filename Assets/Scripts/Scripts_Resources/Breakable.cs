using System;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [Header("Breakable Settings")]
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private float explosionLifetime = 1f;
    [SerializeField] private GameObject resourceDropped;

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
        if (health != null) 
            health.OnDeath.AddListener(HandleDeath);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT HIT HIT");
        // Check if hit by a projectile
        if (other.gameObject.GetComponent<ProjectileBase>())
        {
            ProjectileBase projectile = other.gameObject.GetComponent<ProjectileBase>(); 
            Debug.Log("DAMAGE TAKEN");
            health?.TakeDamage(25); // damage value, adjust as needed
            Destroy(projectile.gameObject); // remove projectile on impact
        }
    }
    
    private void HandleDeath()
    {
        // Spawn VFX
        if (explosionVFX != null)
        {
            GameObject vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(vfx, explosionLifetime);
        }

        // Drop resource
        if (resourceDropped != null)
        {
            Instantiate(resourceDropped, transform.position, Quaternion.identity);
        }

        // Destroy this breakable object
        Destroy(gameObject);
    }
}