using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float slashCooldown = 1f; 
    [SerializeField] private GameObject bulletPrefab;   // assign in Inspector
    [SerializeField] private Transform bulletSpawnPoint; // optional spawn point

    private bool canSlash = true;

    public void OnSlash(InputAction.CallbackContext context)
    {
        if (!context.performed || !canSlash) return;

        // ✅ Block attack if UI is open
        if (GameManager.Instance.UIManager.IsUIBlockingGameplay())
            return;

        GameManager.Instance.PlayerController.PlayerAnimation?.TriggerSlash();
        SpawnBullet();
        StartCoroutine(SlashCooldownRoutine());
    }


    private void SpawnBullet()
    {
        if (bulletPrefab == null) return;

        Transform spawn = bulletSpawnPoint != null ? bulletSpawnPoint : transform;
        Instantiate(bulletPrefab, spawn.position, spawn.rotation);
    }

    private IEnumerator SlashCooldownRoutine()
    {
        canSlash = false;
        yield return new WaitForSeconds(slashCooldown);
        canSlash = true;
    }
}