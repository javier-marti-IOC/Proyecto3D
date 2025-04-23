using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{

    public Element activeElement;
    protected int healthPoints;
    protected GameObject player;
    public Tower tower;
    public GameManager gameManager;
    public GameObject ghostAgent;
    protected Animator animator;
    protected bool playerHitted;

    [Header("Booleans")]
    public bool towerCalling; // Booleana para saber cuando la torre nos esta llamando
    protected bool onAction; // Esta realizando alguna accion
    protected bool onCombat; // El enemigo esta en combate
    protected bool onHealZone; // Esta el enemigo en zona de cura de la torre
    protected bool playerInAttackRange; // Esta el player en mi zona de ataque
    protected bool towerInRange; // Tengo la torre en rango para patrullar

    [Header("Ranges")]
    [SerializeField] protected int minCooldownTimeInclusive; /* Tiempo minimo inclusivo del rango 
                                            (este numero si entra en el rango)*/
    [SerializeField] protected int maxCooldownTimeExclusive; /* Tiempo maximo exclusivo del rango 
                                            (este numero no entra en el rango)*/

    [Header("Cooldowns")]
    protected float cooldownHeavyAttack; // Cooldown para volver a realizar ataque fuerte


    [Header("Patrol")]
    protected NavMeshAgent agent;
    protected GameObject ghost;
    [SerializeField] protected GameObject ghostPrefab;
    [SerializeField] protected float safeDistance = 1f;  // Distancia prudencial que se quiere mantener
    protected GameObject pointPatrol;


    [Header("Chase")]
    public bool playerDetected; // Detecto al player
    [SerializeField] protected GameObject playerDetector;
    [SerializeField] protected GameObject minDistanceChase;

    [Header("Collider")]
    [SerializeField] protected Collider basicAttackCollider;
    [SerializeField] protected Collider heavyAttackCollider;

    [Header("Damages")]
    [SerializeField] protected int basicAttackBasicDamage;
    [SerializeField] protected int basicAttackElementalDamage;
    [SerializeField] protected int heavyAttackBasicDamage;
    [SerializeField] protected int heavyAttackElementalDamage;



    protected virtual void Awake()
    {
        healthPoints = 100;
        player = GameObject.FindGameObjectWithTag(Constants.player);
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = safeDistance;
        animator = GetComponent<Animator>();
        pointPatrol = GameObject.Find(Constants.pointPatrol);

    }

    // Metodos comunes
    // Indicamos virtual para poder sobreescribir el metodo si fuera necesario
    // O indicamos abstract para que las clases hijas o hereden si o si
    protected virtual void Heal()
    {

    }

    protected void Patrol()
    {
        if (ghost == null)
        {
            ghost = Instantiate(ghostPrefab, agent.transform.position, Quaternion.identity, agent.transform);
        }

        ghost.GetComponent<RunnerGhostAgent>().patrolPoint = pointPatrol;
        agent.SetDestination(ghost.transform.position);
        agent.speed = ghost.GetComponent<NavMeshAgent>().speed - 1f;
        
        animator.SetInteger(Constants.state,0);
        /*timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(agent.transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            // agent.speed = Random.Range(1.0f, 4.0f);
            timer = 0;
        }*/

    }

    public void Chase()
    {
        Destroy(ghost);
        playerDetector.SetActive(false);
        minDistanceChase.SetActive(false);
        agent.SetDestination(player.transform.position);
    }
    public void StopChasing()
    {
        playerDetector.SetActive(true);
        minDistanceChase.SetActive(true);
        playerDetected = false;
    }
    protected virtual void TowerChase()
    {
        Destroy(ghost);
        playerDetector.SetActive(false);
        minDistanceChase.SetActive(false);
        agent.SetDestination(tower.transform.position);

    }

    public void basicAttackEnter(Collider other)
    {
        if (other.tag.Equals(Constants.player) && !playerHitted )
        {
            playerHitted = true;
            //player.GetComponent<tempPlayer>().healthPoints -= gameManager.DamageCalulator(activeElement,earthBasicAttackBasicDamage,earthBasicAttackElementalDamage,player.GetComponent<tempPlayer>().activeElement);
        }
    }

    public void PlayerInAttackRangeEnter(Collider other)
    {
        if (other.tag.Equals(Constants.player))
        {
            playerInAttackRange = true;
        }
    }

    public void PlayerInAttackRangeExit(Collider other)
    {
        if (other.tag.Equals(Constants.player))
        {
            playerInAttackRange = false;
        }
    }

    private void BasicAttackActivated()
    {
        playerHitted = false;
        basicAttackCollider.enabled = true;
    }

    private void BasicAttackDisabled()
    {
        playerHitted = false;
        basicAttackCollider.enabled = false;
    }

    private void HeavyAttackActivated()
    {
        playerHitted = false;
        heavyAttackCollider.enabled = true;
    }

    private void HeavyAttackDisabled()
    {
        playerHitted = false;
        heavyAttackCollider.enabled = false;
        cooldownHeavyAttack = Random.Range(minCooldownTimeInclusive,maxCooldownTimeExclusive);
    }

    public void heavyAttackEnter(Collider other)
    {
        if (other.tag.Equals(Constants.player) && !playerHitted )
        {
            playerHitted = true;            
            //player.GetComponent<tempPlayer>().healthPoints -= gameManager.DamageCalulator(activeElement,earthHeavyAttackBasicDamage,earthHeavyAttackElementalDamage,player.GetComponent<tempPlayer>().activeElement);
        }
    }
}
