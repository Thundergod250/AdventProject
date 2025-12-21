using System.Collections;
using UnityEngine;

public class ExplosiveBase : MonoBehaviour
{
    // For the sake of VFX, spawn here
    [SerializeField] float _damage = 0f;

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();

        if (enemy)
        {
            enemy.Health.TakeDamage((int)_damage);

            Destroy(this?.gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(CO_SelfDestroy());
    }

    IEnumerator CO_SelfDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this?.gameObject);
    }
}
