using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float slashCooldown = 0.5f; 
    [SerializeField] private float spawnDelay = 0.2f; // ⏱️ Editable delay before bullet spawns
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    public Vector3 direction;
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

        if (GameManager.Instance.UI_ReticleRaycast != null)
        {
            direction = GameManager.Instance.UI_ReticleRaycast.ray.direction;
        }
        //if (GameManager.Instance.UI_ReticleRaycast != null && GameManager.Instance.UI_ReticleRaycast.GetRaycastHit(out hit)) 
        //{ 
        //    direction = hit.point; 
        //}
        //else
        //{ // fallback: shoot straight forward
        //  direction = spawn.position + spawn.forward * 50f;

            //}

        GameObject bulletObj = Instantiate(
            bulletPrefab, 
            spawn.position, 
            spawn.rotation
        );

        ProjectileBase projectile = bulletObj.GetComponent<ProjectileBase>();

        if (projectile != null) { projectile.SetDirection(direction); }
    }

    private IEnumerator SlashCooldownRoutine()
    {
        canSlash = false;
        yield return new WaitForSeconds(slashCooldown);
        canSlash = true;
    }
}