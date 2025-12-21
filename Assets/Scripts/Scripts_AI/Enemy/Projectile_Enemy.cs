using UnityEngine;

public class Projectile_Enemy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Health>() != null)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(25);
        }

        Destroy(this.gameObject);
    }
}
