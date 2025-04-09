using UnityEngine;
using UnityEngine.AI;

public class RunnerAgent : MonoBehaviour
{
    [SerializeField] private float wanderTimer;
    [SerializeField] private float wanderRadius;
    [HideInInspector] public GameObject mesh;

    private Transform target;
    private NavMeshAgent agent;
    private float timer;


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
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(mesh.transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            // agent.speed = Random.Range(1.0f, 4.0f);
            timer = 0;
        }
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
