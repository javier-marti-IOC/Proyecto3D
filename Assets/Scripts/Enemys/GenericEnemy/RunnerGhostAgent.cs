using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class RunnerGhostAgent : MonoBehaviour
{
    [SerializeField] private float wanderTimer;
    [SerializeField] private float wanderRadius;
    private NavMeshAgent agent;
    private float timer;

    private float proximityThreshold = 1f; // Distancia para recalcular el destino
    // private bool rotating;
    public bool chase;
    public GameObject player;


    // Start is called before the first frame update



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        player = GameObject.FindWithTag(Constants.player);
    }

    // Update is called once per frame
    void Update()
    {
        if (chase)
        {
            Chase();
        }
        else
        {
            Patrol();

        }
    }

    private void Chase()
    {
        agent.SetDestination(player.transform.position);
    }


    private void Patrol()
    {
        //Sumar segundos
        timer += Time.deltaTime;

        // Verifica si está suficientemente cerca del destino
        if (!agent.pathPending && agent.remainingDistance <= proximityThreshold)
        {
            // 50% de probabilidad de rotar antes de continuar
            /* if (!rotating && Random.Range(1, 3) == 1)
            {
                RotateRandomly(); // TODO retocar funcion de rotar
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
    /* private void RotateRandomly()
    {
        Debug.Log("Rotar posicion");
        rotating = true;
        float randomYRotation = Random.Range(0f, 180f);
        transform.rotation = Quaternion.Euler(0, randomYRotation, 0);
        Invoke("StopRotating", wanderTimer);
    } */
    /* private void StopRotating()
    {
        rotating = false;
    } */

    private void SetNewDestination()
    {
        Vector3 newPos = RandomNavSphere(agent.transform.position, wanderRadius, -1);
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
