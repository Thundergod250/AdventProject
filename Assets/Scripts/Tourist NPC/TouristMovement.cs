using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class TouristMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("Places Of Interest")]
    private GameObject destination;
    private GameObject food_Stall;
    private GameObject returnShip;

    [SerializeField] private int hungerMeter;
    [SerializeField] private float Seconds = 1f; // delay used for ReduceHunger

    [Header("Hunger Requirements")]
    [SerializeField] private int maxHunger = 10;
    [SerializeField] private float canBeHungryNow = 5f; // threshold before hunger increases
    [SerializeField] private float hungerTimer = 0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        hungerMeter = Random.Range(0, 10);
    }

    public int VisitScore()
    {
        return hungerMeter + GameManager.Instance.Stall_Rating_Ref.Stall_Appeal;
    }

    public void SetDestination(GameObject attraction)
    {
        destination = attraction;

        Vector3 targetPos = destination.transform.position;

        // Random offset within a circle
        Vector2 offset = Random.insideUnitCircle * 2f; // radius = 2 units
        Vector3 finalPos = new Vector3(targetPos.x + offset.x, targetPos.y, targetPos.z + offset.y);

        if (agent != null)
        {
            if (VisitScore() > 10 && food_Stall != null)
                agent.SetDestination(food_Stall.transform.position);
            else if (destination != null)
            {
                agent.SetDestination(finalPos);
                //agent.SetDestination(destination.transform.position);

                StartCoroutine(ReturnToShipAfterDestination());
            }
        }
    }

    public void SetFoodStall(GameObject stall)
    {
        food_Stall = stall;
    }

    public void SetReturnShip(GameObject ship)
    {
        returnShip = ship;
    }

    private void Update()
    {
        HandleMovement();
        HandleHungerIncrease(); // hunger logic now lives here
    }

    public void HandleMovement()
    {
        if (VisitScore() > 10 && food_Stall != null && destination != null)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                StartCoroutine(ReduceHungerAndGoToDestination());
            }
        }
    }

    private IEnumerator ReduceHungerAndGoToDestination()
    {
        if (hungerMeter <= 0) yield break;

        // Wait before resetting hunger
        yield return new WaitForSeconds(Seconds);

        hungerMeter = 0;

        // Send NPC to destination
        if (destination != null)
        {
            agent.SetDestination(destination.transform.position);

            // Start coroutine to handle next step once destination is reached
            //StartCoroutine(GoToReturnShipAfterDestination());
        }
    }

    private IEnumerator ReturnToShipAfterDestination()
    {
        // Wait until NPC actually arrives at the destination
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null; // keep waiting until arrival
        }

        // Optional pause before heading back
        yield return new WaitForSeconds(Seconds);

        // Now go to return ship
        if (returnShip != null)
        {
            Debug.LogWarning("Return Home");
            agent.SetDestination(returnShip.transform.position);
        }
    }


    private void HandleHungerIncrease()
    {
        if (hungerMeter < maxHunger)
        {
            hungerTimer += Time.deltaTime;

            if (hungerTimer >= canBeHungryNow)
            {
                hungerTimer = 0f;
                hungerMeter += 1;
            }
        }
    }
}
