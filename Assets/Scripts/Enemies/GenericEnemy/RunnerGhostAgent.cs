using UnityEngine;
using UnityEngine.AI;

public class RunnerGhostAgent : MonoBehaviour
{
    [Header("Patrulla")]
    [SerializeField] private float wanderTimer;
    [SerializeField] private float wanderRadius;
    private NavMeshAgent agent;
    private float timer;
    private float proximityThreshold = 1f; // Distancia para recalcular el destino
    // private bool rotating;
    private GameObject player;
    public GameObject patrolPoint;


    // Start is called before the first frame update


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        player = GameObject.Find(Constants.player);
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }


    private void Patrol()
    {
        //Sumar segundos
        timer += Time.deltaTime;

        // Verifica si está suficientemente cerca del destino
        if (!agent.pathPending && agent.remainingDistance <= proximityThreshold)
        {
            // 50% de probabilidad de rotar antes de continuar
            /* if (Random.Range(1, 3) == 1)
            {
                
            }
            else
            { */
                SetNewDestination();
                timer = 0;
            /* } */
        }

        // También calcula nuevo destino si ha pasado el tiempo
        if (timer >= wanderTimer)
        {
            SetNewDestination();
            timer = 0;
        }
    }


private void SetNewDestination()
{
    Vector3 newPos = RandomNavSphere(patrolPoint.transform.position, wanderRadius, -1);
    agent.SetDestination(newPos);
}

private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
{
    Vector3 randDirection = Random.insideUnitSphere * dist;

    randDirection += origin;

    NavMeshHit navHit;

    NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

    return navHit.position;
}
}
