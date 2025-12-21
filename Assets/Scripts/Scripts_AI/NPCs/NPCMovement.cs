using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    [Header("References")]
    public GameObject NPCBody;

    [Header("Rotation")]
    public float rotationSpeed = 8f;
    
    private NavMeshAgent agent;

    [Header("================= For Chase Sequence =================")]
    public List<Transform> CirclePoints = new List<Transform>();
    public float ReachDistance = 0.3f;

    private int currentPointIndex = 0;
    public bool CanFollowPoints = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // we’ll handle rotation ourselves
    }
    
    public void _GoTotargetPoint(Transform targetPoint)
    {
        if (agent == null || targetPoint == null) return;
        agent.SetDestination(targetPoint.position);
    }   

    private void Update()
    {
        // If agent is moving, rotate NPCBody toward velocity direction
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            NPCBody.transform.rotation = Quaternion.Slerp(NPCBody.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (!CanFollowPoints || CirclePoints.Count == 0)
        {
            return;
        }

        // If agent reached destination, move to next point
        if (!agent.pathPending && agent.remainingDistance <= ReachDistance && CanFollowPoints)
        {
            GoToNextPoint();
        }
    }

    public void SetCanFollow()
    {
        CanFollowPoints = true;
    }

    public void StopFollowing()
    {
        CanFollowPoints = false;
    }

    void GoToNextPoint()
    {
        currentPointIndex = (currentPointIndex + 1) % CirclePoints.Count;
        agent.SetDestination(CirclePoints[currentPointIndex].position);
    }
}