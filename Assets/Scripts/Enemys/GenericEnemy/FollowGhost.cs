using UnityEngine;
using UnityEngine.AI;

public class FollowGhost : MonoBehaviour
{
    public GameObject ghost; // El transform del ghost a seguir
    private NavMeshAgent agent;
    private float safeDistance = 1f;  // Distancia prudencial que se quiere mantener


    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();

        agent.stoppingDistance = safeDistance;
    }

    void Update()
    {
        agent.SetDestination(ghost.transform.position);
        agent.speed = ghost.GetComponent<NavMeshAgent>().speed - 1f;

    }
}
