using UnityEngine;
using UnityEngine.AI;

public class TouristMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject destination;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(GameObject attraction)
    {
        destination = attraction;
        if (agent != null && destination != null)
        {
            agent.SetDestination(destination.transform.position);
        }
    }
}
