using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public UI_ReticleRaycast UI_ReticleRaycast;
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

        GameManager.Instance.PlayerController.PlayerAnimation?.TriggerSlash();

        StartCoroutine(DelayedSpawnBullet());
        StartCoroutine(SlashCooldownRoutine());
    }

    private IEnumerator DelayedSpawnBullet()
    {
        yield return new WaitForSeconds(spawnDelay);

        if (bulletPrefab == null) yield break;


        Transform spawn = bulletSpawnPoint != null ? bulletSpawnPoint : transform;

        RaycastHit hit; 
        Vector3 targetPoint;

        if (UI_ReticleRaycast != null && UI_ReticleRaycast.GetRaycastHit(out hit)) { targetPoint = hit.point; }
        else
        { // fallback: shoot straight forward
          targetPoint = spawn.position + spawn.forward * 50f;
          
        }

        GameObject bulletObj = Instantiate(bulletPrefab, spawn.position, spawn.rotation);

        ProjectileBase projectile = bulletObj.GetComponent<ProjectileBase>();

        if (projectile != null) { projectile.SetTarget(targetPoint); }
    }

    private IEnumerator SlashCooldownRoutine()
    {
        canSlash = false;
        yield return new WaitForSeconds(slashCooldown);
        canSlash = true;
    }
}