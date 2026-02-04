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

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // Auto-despawn after lifetime
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector3 direction) 
    { 
        moveDirection = direction;

        rb.linearVelocity = direction.normalized * speed;

        // Rotate projectile to face movement direction
        if (moveDirection != Vector3.zero)
        { 
            transform.rotation = Quaternion.LookRotation(moveDirection); 
        }
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