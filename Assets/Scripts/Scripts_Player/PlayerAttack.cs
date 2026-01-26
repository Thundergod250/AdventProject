using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float slashCooldown = 1f; 
    [SerializeField] private GameObject bulletPrefab;   // assign in Inspector
    [SerializeField] private Transform bulletSpawnPoint; // optional spawn point (e.g. hand or front of player)
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private float bulletLifetime = 1.5f;

    private bool canSlash = true;

    public void OnSlash(InputAction.CallbackContext context)
    {
        if (!context.performed || !canSlash) return;

        // Trigger animation
        GameManager.Instance.PlayerController.PlayerAnimation?.TriggerSlash();

        // Spawn bullet
        SpawnBullet();

        // Start cooldown
        StartCoroutine(SlashCooldownRoutine());
    }

    private void SpawnBullet()
    {
        if (bulletPrefab == null) return;

        // Use spawn point if provided, otherwise player position
        Transform spawn = bulletSpawnPoint != null ? bulletSpawnPoint : transform;

        GameObject bullet = Instantiate(bulletPrefab, spawn.position, spawn.rotation);

        // Give it forward velocity
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = spawn.forward * bulletSpeed;
        }

        // Destroy after lifetime
        Destroy(bullet, bulletLifetime);
    }

    private IEnumerator SlashCooldownRoutine()
    {
        canSlash = false;
        yield return new WaitForSeconds(slashCooldown);
        canSlash = true;
    }
}