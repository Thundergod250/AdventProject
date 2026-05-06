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

    [Header("Booleans")]
    [SerializeField] private bool isGoingHome;
    [SerializeField] private bool isGoingToStallAgain;

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

        if (agent != null)
        {
            if (VisitScore() > 10 && food_Stall != null)
            {
                agent.SetDestination(food_Stall.transform.position);
            }
            else if (destination != null)
            {
                GoToDestination();
                //// Offset destination to avoid crowding
                //Vector3 targetPos = destination.transform.position;
                //Vector2 offset = Random.insideUnitCircle * 2f;
                //Vector3 finalPos = new Vector3(targetPos.x + offset.x, targetPos.y, targetPos.z + offset.y);

                //agent.SetDestination(finalPos);

                //// Start coroutine to handle return ship after destination
                //StartCoroutine(ReturnToShipAfterDestination());
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
        //For Going To Stall
        if (VisitScore() > 10 && food_Stall != null && destination != null)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                StartCoroutine(ReduceHungerCoroutine());
            }
        }
    }

    private IEnumerator ReduceHungerCoroutine()
    {
        if (hungerMeter <= 0) yield break;

        // Wait before resetting hunger
        yield return new WaitForSeconds(Seconds);

        // Instantly reset hunger
        hungerMeter = 0;

        if(isGoingHome != true && isGoingToStallAgain != true)
            // After hunger reset, call function to move NPC
            GoToDestination();
    }

    private void GoToDestination()
    {
        if (destination != null)
        {
            // Offset destination to avoid crowding
            Vector3 targetPos = destination.transform.position;
            Vector2 offset = Random.insideUnitCircle * 2f;
            Vector3 finalPos = new Vector3(targetPos.x + offset.x, targetPos.y, targetPos.z + offset.y);

            agent.SetDestination(finalPos);

            // Chain return ship logic
            StartCoroutine(ReturnToShipAfterDestination());
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

        DecideNextAction();
    }

    public void DecideNextAction()
    {
        // Check hunger level
        if (hungerMeter < 5)
        {
            isGoingHome = true;
            isGoingToStallAgain = false;

            // Go straight home
            if (returnShip != null)
            {
                agent.SetDestination(returnShip.transform.position);
            }
        }
        else if (hungerMeter > 5)
        {
            isGoingHome = false;
            isGoingToStallAgain = true;

            // Go to stall first
            if (food_Stall != null)
            {
                agent.SetDestination(food_Stall.transform.position);
                StartCoroutine(GoToStallThenReturnShip());
            }
        }
    }

    private IEnumerator GoToStallThenReturnShip()
    {
        // Wait until NPC actually arrives at the food stall
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        // Optional pause at stall
        yield return new WaitForSeconds(Seconds);

        // Now go to return ship
        if (returnShip != null)
        {
            agent.SetDestination(returnShip.transform.position);
            isGoingHome = true;
            isGoingToStallAgain = false;
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
