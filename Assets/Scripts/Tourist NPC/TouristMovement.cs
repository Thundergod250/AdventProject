using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class TouristMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject destination;
    private GameObject food_Stall;

    [SerializeField] private int hunger_Meter;
    [SerializeField] private float Seconds = 1f; // delay used for ReduceHunger

    [Header("Hunger Requirements")]
    [SerializeField] private int maxHunger = 10;
    [SerializeField] private float canBeHungryNow = 5f; // threshold before hunger increases
    [SerializeField] private float hungerTimer = 0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        hunger_Meter = Random.Range(0, 10);
    }

    public int VisitScore()
    {
        return hunger_Meter + GameManager.Instance.Stall_Rating_Ref.Stall_Appeal;
    }

    public void SetDestination(GameObject attraction)
    {
        destination = attraction;

        if (agent != null)
        {
            if (VisitScore() > 10 && food_Stall != null)
                agent.SetDestination(food_Stall.transform.position);
            else if (destination != null)
                agent.SetDestination(destination.transform.position);
        }
    }

    public void SetFoodStall(GameObject stall)
    {
        food_Stall = stall;
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
        if (hunger_Meter <= 0) yield break;

        yield return new WaitForSeconds(Seconds);

        hunger_Meter = 0;

        if (destination != null)
        {
            agent.SetDestination(destination.transform.position);
        }
    }

    private void HandleHungerIncrease()
    {
        if (hunger_Meter < maxHunger)
        {
            hungerTimer += Time.deltaTime;

            if (hungerTimer >= canBeHungryNow)
            {
                hungerTimer = 0f;
                hunger_Meter += 1;
            }
        }
    }
}
