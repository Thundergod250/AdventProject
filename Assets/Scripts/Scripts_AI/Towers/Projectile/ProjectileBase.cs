using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifetime = 1.5f;
    [SerializeField] float startTime = 0;
    [SerializeField] private GameObject explosionVFX; // optional prefab for impact effect
    [SerializeField] private float explosionLifetime = 1f; // how long the VFX stays before despawn

    [Header("Target Settings")]
    [SerializeField] private List<Faction> attackableFactions;

    private Rigidbody rb;
    private Vector3 currentDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetDirection(Vector3 direction)
    {
        currentDirection = direction.normalized;
    }

    private void Update()
    {
        // Move straight in the assigned direction
        rb.linearVelocity = currentDirection * speed;

        // Rotate to face movement direction
        if (currentDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(currentDirection);
        }

        if (startTime < lifetime)
        {
            startTime += Time.deltaTime;
        }
        else
        {
            SpawnVFX();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth && attackableFactions.Contains(targetHealth.GetFaction()))
        {
            SpawnVFX();

            targetHealth.TakeDamage(1);
            // Destroy projectile on impact
            Destroy(gameObject);
        }
    }

    private void SpawnVFX()
    {
        // Spawn VFX if assigned
        if (explosionVFX != null)
        {
            GameObject vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(vfx, explosionLifetime); // destroy VFX after its lifetime
        }
    }
}