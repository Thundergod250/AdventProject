using System.Collections;
using UnityEngine;

public class Tower_Defensive_HealingTower : TowerDefensiveBase
{
    [SerializeField] protected int healAmount = 10;
    [SerializeField] protected float delayPerTick = 1f;

    [SerializeField] protected PlayerController playerController;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            playerController = other.GetComponent<PlayerController>();
            StartCoroutine(CO_HealPerTick());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            playerController = null;
        }
    }

    IEnumerator CO_HealPerTick()
    {
        yield return new WaitForSeconds(delayPerTick);
        playerController?.gameObject.GetComponentInChildren<Health>()?.Heal(healAmount);

        if (playerController != null)
        {
            StartCoroutine(CO_HealPerTick());
        }
    }
}
