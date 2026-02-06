using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifetime = 1.5f;
    [SerializeField] private GameObject explosionVFX; // optional prefab for impact effect
    [SerializeField] private float explosionLifetime = 1f; // how long the VFX stays before despawn
    [SerializeField] private float turnRate = 5f; // how fast the bullet curves
    private Vector3 moveDirection;

    private Vector3 currentDirection;
    private Vector3 targetDirection;

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

    public void SetDirection(Vector3 initialDirection, Vector3 targetDir) 
    {
        currentDirection = initialDirection.normalized; 
        targetDirection = targetDir.normalized; 

        rb.linearVelocity = currentDirection * speed; 
        transform.rotation = Quaternion.LookRotation(currentDirection);
        //moveDirection = direction;

        //rb.linearVelocity = direction.normalized * speed;

        //// Rotate projectile to face movement direction
        //if (moveDirection != Vector3.zero)
        //{ 
        //    transform.rotation = Quaternion.LookRotation(moveDirection); 
        //}
    }

    private void Update()
    { 
        // Gradually rotate currentDirection toward targetDirection
        currentDirection = Vector3.RotateTowards( 
            currentDirection, 
            targetDirection, 
            turnRate * Time.deltaTime, 
            1f // max magnitude change
        ); 
        
        rb.linearVelocity = currentDirection * speed; 
        transform.rotation = Quaternion.LookRotation(currentDirection);
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