using UnityEngine;
using UnityEngine.AI;

public class FollowGhost : MonoBehaviour
{
    public GameObject ghost; // El transform del ghost a seguir
    private NavMeshAgent agent;

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.SetDestination(ghost.transform.position);
        agent.speed = ghost.GetComponent<NavMeshAgent>().speed;

    }
}
