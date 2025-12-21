using UnityEngine;

public class Attack_Enemy : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Navigation_Enemy navigation_Enemy;

    [Header("Weapon Stats")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gunBarrel;
    [SerializeField] private float shootingInterval;
    [SerializeField] private float shootingSpeed;

    public Transform target;

    private float timer;

    void Update()
    {
        if(target != null)
        {
            timer += Time.deltaTime;

            if (timer >= shootingInterval)
            {
                ShootTarget();
                timer = 0f;
            }

            if (!target.gameObject.activeSelf)
            {
                navigation_Enemy.TargetHasDied();
            }
        }

        
    }

    void ShootTarget()
    {
        transform.LookAt(target.transform);
        Vector3 direction = (target.position - transform.position).normalized;
        GameObject proj = Instantiate(bullet, gunBarrel.transform.position, Quaternion.identity);
        Rigidbody rb = proj.GetComponent<Rigidbody>();

        rb.linearVelocity = direction * 40f;

        Destroy(proj, 2f);
    }

    public void DestroyTarget(GameObject enemyTarget)
    {
        Destroy(enemyTarget);
    }
}
