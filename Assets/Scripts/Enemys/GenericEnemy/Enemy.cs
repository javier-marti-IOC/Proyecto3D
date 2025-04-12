using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{

    [SerializeField] protected Element activeElement;
    protected int healthPoints;
    [SerializeField] protected GameObject player;
    protected GameObject tower;


    [Header("Booleans")]
    protected bool towerCalling; // Booleana para saber cuando la torre nos esta llamando
    protected bool onAction; // Esta realizando alguna accion
    protected bool onCombat; // El enemigo esta en combate
    protected bool onHealZone; // Esta el enemigo en zona de cura de la torre
    protected bool playerDetected; // Detecto al player
    protected bool playerInAttackRange; // Esta el player en mi zona de ataque
    protected bool towerInRange; // Tengo la torre en rango para patrullar

    [Header("Ranges")]
    [SerializeField] protected int minCooldownTimeInclusive; /* Tiempo minimo inclusivo del rango 
                                            (este numero si entra en el rango)*/
    [SerializeField] protected int maxCooldownTimeExclusive; /* Tiempo maximo exclusivo del rango 
                                            (este numero no entra en el rango)*/

    [Header("Cooldowns")]
    protected float cooldownHeavyAttack; // Cooldown para volver a realizar ataque fuerte


    [Header("Patrol ")]
    [SerializeField] private float wanderTimer;
    [SerializeField] private float wanderRadius;
    [HideInInspector] public GameObject mesh;
    private NavMeshAgent agent;
    private float timer;


    protected virtual void Awake()
    {
        healthPoints = 100;
        player = GameObject.FindGameObjectWithTag(Constants.player);
        mesh = GameObject.Find(Constants.navMeshSurface);
        agent = GetComponent<NavMeshAgent>();

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    // Metodos comunes
    // Indicamos virtual para poder sobreescribir el metodo si fuera necesario
    // O indicamos abstract para que las clases hijas o hereden si o si
    protected virtual void Heal()
    {

    }

    protected virtual void Patrol()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(agent.transform.position, wanderRadius, -1);
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

    protected virtual void TowerPatrol()
    {

    }
}
