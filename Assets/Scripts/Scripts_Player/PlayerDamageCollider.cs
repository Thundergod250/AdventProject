using System;
using UnityEngine;

public class PlayerDamageCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null) 
            other.GetComponent<Health>().TakePercentageDamage(50);
    }
}
