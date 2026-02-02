using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifetime = 1.5f;
    [SerializeField] private GameObject explosionVFX; // optional prefab for impact effect
    [SerializeField] private float explosionLifetime = 1f; // how long the VFX stays before despawn
    private Vector3 moveDirection;
    public Vector3 direction;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // Move forward immediately
        //if (rb != null)
        //    rb.linearVelocity = transform.forward * speed;

        // Auto-despawn after lifetime
        Destroy(gameObject, lifetime);
    }

    public void SetTarget(Vector3 targetPoint) 
    { 
        moveDirection = (targetPoint - transform.position).normalized;
        
        rb.linearVelocity = moveDirection * speed;
    
    }

    private void OnTriggerEnter(Collider other)
    {
        // Spawn VFX if assigned
        if (explosionVFX != null)
        {
            GameObject vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(vfx, explosionLifetime); // destroy VFX after its lifetime
        }

        // Destroy projectile on impact
        Destroy(gameObject);
    }
}