using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class TouristMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("References")]
    [SerializeField] private GameObject destination;
    [SerializeField] private GameObject food_Stall;
    [SerializeField] private GameObject returnToShip; // NEW: final return point

    [Header("Stats")]
    [SerializeField] private int hunger_Meter;
    [SerializeField] private float waitSeconds = 1f;

    // Queue of waypoints the tourist will follow
    private Queue<GameObject> routeQueue = new Queue<GameObject>();

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
        DecideRoute();
        MoveToNextPoint();
    }

    public void SetFoodStall(GameObject stall)
    {
        food_Stall = stall;
    }

    public void SetReturnToShip(GameObject ship)
    {
        returnToShip = ship;
    }

    /// <summary>
    /// Decide the route based on VisitScore and hunger_Meter.
    /// </summary>
    private void DecideRoute()
    {
        routeQueue.Clear();

        int score = VisitScore();

        // State 1: foodStall -> ship (VisitScore == 10)
        if (score == 10)
        {
            routeQueue.Enqueue(food_Stall);
            routeQueue.Enqueue(returnToShip);
        }
        // State 2: destination -> ship (hunger_Meter < 0)
        else if (hunger_Meter < 0)
        {
            routeQueue.Enqueue(destination);
            routeQueue.Enqueue(returnToShip);
        }
        // State 3: foodStall -> destination -> ship (score >= 5 && hunger_Meter >= 5)
        else if (score >= 5 && hunger_Meter >= 5)
        {
            routeQueue.Enqueue(food_Stall);
            routeQueue.Enqueue(destination);
            routeQueue.Enqueue(returnToShip);
        }
        // State 4: destination -> foodStall -> ship (hunger_Meter >= 5 && score < 10)
        else if (hunger_Meter >= 5 && score < 10)
        {
            routeQueue.Enqueue(destination);
            routeQueue.Enqueue(food_Stall);
            routeQueue.Enqueue(returnToShip);
        }
        // State 5: foodStall -> destination -> foodStall -> ship (score >= 10)
        else if (score >= 10)
        {
            routeQueue.Enqueue(food_Stall);
            routeQueue.Enqueue(destination);
            routeQueue.Enqueue(food_Stall);
            routeQueue.Enqueue(returnToShip);
        }
        else
        {
            // Default fallback: just go to destination then ship
            routeQueue.Enqueue(destination);
            routeQueue.Enqueue(returnToShip);
        }
    }

    private void Update()
    {
        HandleMovement();
    }

    public void HandleMovement()
    {
        if (agent != null && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (routeQueue.Count > 0)
            {
                MoveToNextPoint();
            }
        }
    }

    private void MoveToNextPoint()
    {
        if (routeQueue.Count == 0) return;

        GameObject nextPoint = routeQueue.Dequeue();
        if (nextPoint != null)
        {
            agent.SetDestination(nextPoint.transform.position);
        }
    }
}
