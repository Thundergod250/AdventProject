using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float slashCooldown = 0.5f; 
    [SerializeField] private float spawnDelay = 0.2f; // ⏱️ Editable delay before bullet spawns

    [Header("Bullet Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    private bool canSlash = true;

    public void OnSlash(InputAction.CallbackContext context)
    {
        if (!context.performed || !canSlash) return;

        if (GameManager.Instance.UIManager.IsUIBlockingGameplay()) return;

        GameManager.Instance.PlayerController.PlayerAnimation?.TriggerSlash();

        StartCoroutine(DelayedSpawnBullet());
        StartCoroutine(SlashCooldownRoutine());
    }

    private IEnumerator DelayedSpawnBullet()
    {
        yield return new WaitForSeconds(spawnDelay);

        if (bulletPrefab == null) yield break;

        Transform spawn = bulletSpawnPoint != null ? bulletSpawnPoint : transform;

        // Get ray direction from UI reticle
        Vector3 direction = Vector3.forward;

        if (GameManager.Instance.UI_ReticleRaycast != null)
        {
            direction = GameManager.Instance.UI_ReticleRaycast.ray.direction;
        }

        GameObject bulletObj = Instantiate(
            bulletPrefab,
            bulletSpawnPoint.position,
            Quaternion.LookRotation(direction)
        );

        // Assign direction to projectile
        if (bulletObj.TryGetComponent(out ProjectileBase projectile)) 
        { 
            projectile.SetDirection(direction); 
        }
    }

    private IEnumerator SlashCooldownRoutine()
    {
        canSlash = false;
        yield return new WaitForSeconds(slashCooldown);
        canSlash = true;
    }
}