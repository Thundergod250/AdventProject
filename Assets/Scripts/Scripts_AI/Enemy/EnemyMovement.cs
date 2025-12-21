using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public enum MovementMode
    {
        Idle,
        Roaming,
        GoToPoint,
        FollowTarget
    }

    private NavMeshAgent agent;

    [Header("General Settings")]
    [SerializeField] private MovementMode startMode = MovementMode.Roaming;

    [Header("Roaming Settings")]
    [SerializeField] private float roamRadius = 10f;     // how far from start they can roam
    [SerializeField] private float roamInterval = 5f;    // seconds before switching target

    private float roamTimer;
    private MovementMode currentMode;
    private Transform followTarget;
    private Vector3 pointTarget;

    // Keep track of base speed so we can restore it later
    private float baseSpeed;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        baseSpeed = agent.speed; // store original speed
        SetMode(startMode);
    }

    void Update()
    {
        switch (currentMode)
        {
            case MovementMode.Idle:
                agent.ResetPath(); // stop moving
                break;

            case MovementMode.Roaming:
                HandleRoaming();
                break;

            case MovementMode.GoToPoint:
                HandleGoToPoint();
                break;

            case MovementMode.FollowTarget:
                HandleFollowTarget();
                break;
        }
    }
    
    // -------------------------
    // Mode Switching
    // -------------------------
    public void SetMode(MovementMode mode)
    {
        currentMode = mode;

        if (mode == MovementMode.Roaming)
        {
            roamTimer = roamInterval;
            SetNewDestination();
        }
        else if (mode == MovementMode.Idle)
        {
            agent.ResetPath();
        }
    }
    
    // -------------------------
    // Roaming
    // -------------------------
    private void HandleRoaming()
    {
        roamTimer -= Time.deltaTime;

        if (roamTimer <= 0f || (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance))
        {
            SetNewDestination();
            roamTimer = roamInterval;
        }
    }

    private void SetNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, roamRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
    
    // -------------------------
    // Go To Point
    // -------------------------
    public void GoToPoint(Vector3 point)
    {
        pointTarget = point;
        currentMode = MovementMode.GoToPoint;
        agent.SetDestination(pointTarget);
    }

    private void HandleGoToPoint()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Once reached, stop moving
            SetMode(MovementMode.Idle);
        }
    }
    
    // -------------------------
    // Follow Target
    // -------------------------
    public void FollowTarget(Transform target)
    {
        followTarget = target;
        currentMode = MovementMode.FollowTarget;
    }

    private void HandleFollowTarget()
    {
        if (followTarget == null)
        {
            SetMode(MovementMode.Idle);
            return;
        }

        agent.SetDestination(followTarget.position);
    }

    // -------------------------
    // Speed Modifiers
    // -------------------------
    public void SlowDownAgent(int slowValueTarget)
    {
        agent.speed = baseSpeed / slowValueTarget;
    }

    public void SpeedUpAgent(int speedValueTarget)
    {
        agent.speed = baseSpeed * speedValueTarget;
    }

    public void ResetSpeed()
    {
        agent.speed = baseSpeed;
    }
}

//enemyMovement.SetMode(EnemyMovement.MovementMode.Idle);          // stop moving
//enemyMovement.SetMode(EnemyMovement.MovementMode.Roaming);       // wander randomly
//enemyMovement.GoToPoint(new Vector3(5, 0, 10));                  // move to a point
//enemyMovement.FollowTarget(playerTransform);                     // chase the player
