using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public enum ProjectileOwnerType
    {
        Tower,
        Enemy
    }

    private Transform target;
    private int damage;
    private float speed;
    private GameObject prefabRef; 
    private ProjectileOwnerType ownerType;

    [Header("Lifetime")]
    [SerializeField] private float lifetime = 5f;
    private float lifeTimer;

    [Header("Explosion VFX")]
    [SerializeField] private GameObject explosionVFX; // pooled explosion prefab
    
    public void Initialize(Transform target, int damage, float speed, GameObject prefabRef, ProjectileOwnerType ownerType)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
        this.prefabRef = prefabRef;
        this.ownerType = ownerType;

        lifeTimer = lifetime;
    }

    private void Update()
    {
        // Lifetime countdown
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Explode();
            Debug.Log("This " + gameObject.name + " exploded on Timer");
            ReturnToPool();
            return;
        }

        if (target == null)
        {
            Explode();
            Debug.Log("This " + gameObject.name + " exploded on No Target");
            ReturnToPool();
            return;
        }

        // Move toward target
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * (speed * Time.deltaTime);

        // Rotate projectile to face its movement direction
        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    
    private void ApplyDamageToEnemy(EnemyBase enemy)
    {
        if (enemy != null) 
            enemy.Health.TakeDamage(damage);
    }
    
    private void ApplyDamageToTower(TowerController tower)
    {
        if (tower != null) 
            tower.TowerHealth.TakeDamage(damage);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Ignore self collisions or unrelated colliders
        if (other.gameObject == gameObject) return;

        if (ownerType == ProjectileOwnerType.Tower)
        {
            if (other.TryGetComponent(out EnemyBase enemy))
            {
                ApplyDamageToEnemy(enemy);
                Debug.Log($"{gameObject.name} hit Enemy: {enemy.name}");
                Explode();
                ReturnToPool();
            }
        }

        if (ownerType == ProjectileOwnerType.Enemy)
        {
            if (other.TryGetComponent(out TowerController tower))
            {
                ApplyDamageToTower(tower);
                Debug.Log($"{gameObject.name} hit Tower: {tower.name}");
                Explode();
                ReturnToPool();
            }
        }
    }

    
    private void Explode()
    {
        if (explosionVFX == null) return;

        // Spawn explosion via pooling
        GameObject vfx = GameManager.Instance.SpawnObject(
            explosionVFX,
            null,
            transform.position,
            Quaternion.identity
        );

        // If explosion prefab has DelayDisable, it will auto‑return
        var delay = vfx.GetComponent<DelayDisable>();
        if (delay != null)
            delay.SetPrefabReference(explosionVFX);
    }
    
    private void ReturnToPool()
    {
        if (prefabRef != null)
            GameManager.Instance.ObjectPooling.Return(prefabRef, gameObject);
        else
            gameObject.SetActive(false);
    }
}
