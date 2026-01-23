using UnityEngine;

public class Hitscan : MonoBehaviour
{
    public Camera Camera;
    [SerializeField] private float range = 100f;
    public GameObject Player;

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(Player.transform.position + Vector3.up, Player.transform.forward);

        int layerMask = ~LayerMask.GetMask("Player"); // everything except Player
        //Limits how far you can look
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            //if (!hit.collider.gameObject.GetComponent<PlayerMovement>())
                Debug.Log("Hit object: " + hit.collider.gameObject.name);
            // Apply damage if target has health component var health = hit.collider.GetComponent<Health>(); if (health != null) { health.TakeDamage(damage); } // Optional: spawn impact effect Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal)); }
        }
    }
}
