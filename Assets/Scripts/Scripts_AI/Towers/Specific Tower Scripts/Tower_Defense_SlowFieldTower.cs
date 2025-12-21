using System.Collections.Generic;
using UnityEngine;

public class Tower_Defense_SlowFieldTower : TowerDefensiveBase
{
    [SerializeField] protected int slowMultiplier = 4;
    public float detectionRadius = 15f;

    [SerializeField] protected List<EnemyMovement> enemiesMovement;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyMovement>() )
        {
            EnemyMovement enemyTarget = other.GetComponent<EnemyMovement>();
            enemiesMovement.Add(enemyTarget);

            enemyTarget.SlowDownAgent(slowMultiplier);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EnemyMovement>())
        {
            EnemyMovement enemyTarget = other.GetComponent<EnemyMovement>();
            enemiesMovement.Add(enemyTarget);

            enemyTarget.SpeedUpAgent(slowMultiplier);
        }
    }
}
