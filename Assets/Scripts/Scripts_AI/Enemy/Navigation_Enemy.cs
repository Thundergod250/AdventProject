using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class Navigation_Enemy : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Attack_Enemy attack_Enemy;

    [Header("Colliders")]
    [SerializeField] private SphereCollider sphereCollider;

    [Header("Navigation Target")]
    [SerializeField] private List<GameObject> targetsAcquired = new List<GameObject>();
    [SerializeField] GameObject currentTarget;

    [Header("Vars")]
    [SerializeField] private float distance;
    [SerializeField] private float nearestDistance;

    public NavMeshAgent navigation;

    private void Update()
    {
        if(targetsAcquired.Count > 0)
        {
            FindNearestTarget();
            StoppingDistanceWithinTarget();
        }
    }

    #region Collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != null)
        {
            targetsAcquired.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        targetsAcquired.Remove(other.gameObject);

        if (targetsAcquired.Count == 0)
        {
            nearestDistance = 20;
        }
    }
    #endregion

    #region Navigation&TargetingFuncs
    private void FindNearestTarget()
    {
        for (int i = 0; i < targetsAcquired.Count; i++)
        {
            distance = Vector3.Distance(this.transform.position, targetsAcquired[i].transform.position);

            if (distance < nearestDistance)
            {
                currentTarget = targetsAcquired[i];
                nearestDistance = distance;
            }
        }

        if (currentTarget != null)
        {
            navigation.destination = currentTarget.transform.position;
        }
    }
    private void StoppingDistanceWithinTarget()
    {
        if (currentTarget == null) return;

        if (distance <= navigation.stoppingDistance)
        {
            OnReachedTarget();
        }
    }

    private void OnReachedTarget()
    {
        navigation.isStopped = true;

        if (currentTarget != null)
        {
            this.GetComponent<Attack_Enemy>().target = currentTarget.transform;

        }
    }

    public void TargetHasDied()
    {
        targetsAcquired.Remove(currentTarget);
        currentTarget = null;
        nearestDistance = 21;
        attack_Enemy.target = null;
        navigation.isStopped = false;
    }
    #endregion
}
