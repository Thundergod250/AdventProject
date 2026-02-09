using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float slashCooldown = 0.5f; 
    [SerializeField] private float spawnDelay = 0.2f; // ⏱️ Editable delay before bullet spawns
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
 
    private bool canSlash = true;

    public void OnSlash(InputAction.CallbackContext context)
    {
        if (!context.performed || !canSlash) return;

        if (GameManager.Instance.UIManager.IsUIBlockingGameplay())
            return;

        //GameManager.Instance.PlayerController.PlayerAnimation?.TriggerSlash();

        StartCoroutine(DelayedSpawnBullet());
        StartCoroutine(SlashCooldownRoutine());
    }

    private IEnumerator DelayedSpawnBullet()
    {
        yield return new WaitForSeconds(spawnDelay); if (bulletPrefab == null) yield break; 

        // Default spawn transform
        Transform spawn = bulletSpawnPoint != null ? bulletSpawnPoint : transform; 

        // Get ray from UI reticle
        Ray ray = GameManager.Instance.UI_ReticleRaycast != null ? GameManager.Instance.UI_ReticleRaycast.ray : new Ray(spawn.position, spawn.forward); 

        Vector3 direction = ray.direction; 
        
        // ✅ Project spawn point onto the ray line
        Vector3 spawnPos = ClosestPointOnRay(ray, spawn.position); 
        
        // Instantiate bullet exactly on the ray line
        GameObject bulletObj = Instantiate( 
            bulletPrefab,
            bulletSpawnPoint.position,
            Quaternion.identity
            //Quaternion.LookRotation(direction) 
         ); 
        
        if (bulletObj.TryGetComponent(out ProjectileBase projectile)) 
        {
            // initial = gun forward, target = reticle ray direction
            Vector3 initialDir = bulletSpawnPoint.forward; 
            Vector3 targetDir = direction;

            projectile.SetDirection(initialDir, targetDir);
        }
    }

    private Vector3 ClosestPointOnRay(Ray ray, Vector3 point)
    {
        // Find the closest point on the ray to the given spawn point
        Vector3 toPoint = point - ray.origin; 
        float t = Vector3.Dot(toPoint, ray.direction.normalized); 
        return ray.origin + ray.direction.normalized * t;
    }

    private IEnumerator SlashCooldownRoutine()
    {
        canSlash = false;
        yield return new WaitForSeconds(slashCooldown);
        canSlash = true;
    }
}