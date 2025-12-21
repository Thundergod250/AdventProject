using UnityEngine;
using UnityEngine.AI;

public class EnemyChasingNPCMovement : MonoBehaviour
{
    public Transform NPC;
    public float updateRate = 0.2f; // How often to update destination

    private NavMeshAgent agent;
    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (NPC == null)
            return;

        timer += Time.deltaTime;

        // Update destination at intervals (better performance & smoother paths)
        if (timer >= updateRate)
        {
            timer = 0f;
            agent.SetDestination(NPC.position);
        }
    }

    public void ChaseTheNPC(Transform npc)
    {
        NPC = npc;
    }
}
