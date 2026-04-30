using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class TouristMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject destination;
    private GameObject foodStall;
    [SerializeField] private int hunger_Meter;
    [SerializeField] private float Seconds;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        hunger_Meter = Random.Range(0, 10); // random hunger level
    }

    public void SetDestination(GameObject attraction)
    {
        destination = attraction;

        if (agent != null)
        {
            if (hunger_Meter > 5 && foodStall != null)
                agent.SetDestination(foodStall.transform.position); // First go to food stall

            else if (destination != null)
                agent.SetDestination(destination.transform.position); // Go directly to attraction
        }
    }

    public void SetFoodStall(GameObject stall)
    {
        foodStall = stall;
    }

    private void Update()
    {
        // If hunger was high and tourist has reached the food stall, start reducing hunger
        if (hunger_Meter > 5 && foodStall != null && destination != null)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                // Start coroutine to reduce hunger gradually
                StartCoroutine(ReduceHungerAndGoToDestination());
            }
        }
    }

    private IEnumerator ReduceHungerAndGoToDestination()
    {
        // Prevent multiple coroutines from stacking
        if (hunger_Meter <= 0) yield break;

        while (hunger_Meter > 0)
        {
            hunger_Meter -= 1;
            yield return new WaitForSeconds(Seconds); // subtract 1 every second
        }

        // Once hunger reaches 0, go to destination
        if (destination != null)
        {
            agent.SetDestination(destination.transform.position);
        }
    }
}
