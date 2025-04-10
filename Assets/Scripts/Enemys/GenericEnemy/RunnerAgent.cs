using UnityEngine;
using UnityEngine.AI;

public class RunnerAgent : MonoBehaviour
{
    [SerializeField] private float wanderTimer;
    [SerializeField] private float wanderRadius;
    [HideInInspector] public GameObject mesh;
    private NavMeshAgent agent;
    private float timer;

    private float proximityThreshold = 0.5f; // Distancia para recalcular el destino


    // Start is called before the first frame update
    void Start()
    {
        mesh = GameObject.Find(Constants.navMeshSurface);
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
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
            if (Random.Range(1, 3) == 1)
            {
                RotateRandomly(); // TODO retocar funcion de rotar
            }
            else
            {
                SetNewDestination();
                timer = 0;
            }
        }

        // También calcula nuevo destino si ha pasado el tiempo
        if (timer >= wanderTimer)
        {
            SetNewDestination();
            timer = 0;
        }
    }
    private void RotateRandomly()
    {
        Debug.Log("Rotar posicion");
        float randomYRotation = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0, randomYRotation, 0);
    }

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
