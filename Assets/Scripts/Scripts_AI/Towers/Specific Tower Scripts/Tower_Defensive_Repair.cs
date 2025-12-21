using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Defensive_Repair : TowerDefensiveBase
{
    [SerializeField] protected int healAmount = 1;
    [SerializeField] protected float delayPerTick = 1f;

    public float detectionRadius = 15f;

    [SerializeField] protected List<TowerController> towersNear;

    [SerializeField] protected List<Health> towersNearHealth;


    private void Start()
    {
        StartCoroutine(CO_HealPerTick());
    }

    private void Update()
    {
        TowerChecking();
    }

    private void TowerChecking()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider hit in hits)
        {
            if (!towersNear.Contains( hit.GetComponent<TowerController>())  )
            {
                towersNear.Add(hit.GetComponent<TowerController>());

                if (!towersNearHealth.Contains(hit.GetComponentInChildren<Health>()) )
                {
                    towersNearHealth.Add(hit.GetComponentInChildren<Health>());
                }
            }
        }
    }


    IEnumerator CO_HealPerTick()
    {
        yield return new WaitForSeconds(delayPerTick);

        for (int i = 0; i < towersNearHealth.Count; i++) 
        {
            towersNearHealth[i]?.Heal(healAmount);
        }

        StartCoroutine(CO_HealPerTick());
    }
}
