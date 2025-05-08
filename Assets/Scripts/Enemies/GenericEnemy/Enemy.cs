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

    [Header("Booleans")]
    public bool towerCalling; // Booleana para saber cuando la torre nos esta llamando
    protected bool onAction; // Esta realizando alguna accion
    protected bool onCombat; // El enemigo esta en combate
    protected bool onHealZone; // Esta el enemigo en zona de cura de la torre
    public bool playerInAttackRange; // Esta el player en mi zona de ataque
    protected bool towerInRange; // Tengo la torre en rango para patrullar
    public bool playerHitted;
    protected bool attacking;

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
    [SerializeField] protected GameObject pointPatrol;

    /* [Header("Rotacion")]
    protected Enemy enemy;
    protected bool rotating = false;
    protected Quaternion startRotation;
    protected Quaternion targetRotation;
    [SerializeField] protected float rotationTimer = 0f;
    [SerializeField] protected float wanderRotationTimer = 2f; // tiempo que debe durar la rotación */

    [Header("Chase")]
    public bool playerDetected; // Detecto al player
    [SerializeField] protected GameObject playerDetectorDown;
    [SerializeField] protected GameObject playerDetectorUp;
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

        agent.stoppingDistance = safeDistance;

        ghost.GetComponent<RunnerGhostAgent>().patrolPoint = pointPatrol;
        agent.SetDestination(ghost.transform.position);
        agent.speed = ghost.GetComponent<NavMeshAgent>().speed - 1f;

        animator.SetInteger(Constants.state, 1);

    }

    // No se implementa
    /* public void RotateRandomly()
    {
        float randomYRotation = Random.Range(0f, 180f);
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0, randomYRotation, 0);
        rotationTimer = 0f;
        rotating = true;
    }
    protected void Rotate()
    {
        animator.SetInteger(Constants.state, 0);
        Destroy(ghost);
        rotationTimer += Time.deltaTime;
        float t = rotationTimer / wanderRotationTimer;
        t = Mathf.Clamp01(t); // asegurar que no pase de 1

        transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

        if (t >= 1f)
        {
            rotating = false;
        }
    } */

    public void Chase()
    {
        Destroy(ghost);
        playerDetectorDown.SetActive(false);
        playerDetectorUp.SetActive(false);
        minDistanceChase.SetActive(false);
        agent.SetDestination(player.transform.position);
    }
    public void Chase(float stoppingDistance)
    {
        Destroy(ghost);
        playerDetectorDown.SetActive(false);
        playerDetectorUp.SetActive(false);
        minDistanceChase.SetActive(false);
        agent.SetDestination(player.transform.position);
        agent.stoppingDistance = stoppingDistance;
    }
    public void StopChasing()
    {
        playerDetectorDown.SetActive(true);
        playerDetectorUp.SetActive(true);
        minDistanceChase.SetActive(true);
        playerDetected = false;
    }
    protected virtual void TowerChase()
    {
        Destroy(ghost);
        playerDetectorDown.SetActive(false);
        playerDetectorUp.SetActive(false);
        minDistanceChase.SetActive(false);
        agent.SetDestination(tower.transform.position);

    }

    public void BasicAttackActivated()
    {
        playerHitted = false;
        basicAttackCollider.enabled = true;
    }

    public void BasicAttackDisabled()
    {
        playerHitted = false;
        basicAttackCollider.enabled = false;
    }

    public void HeavyAttackActivated()
    {
        playerHitted = false;
        heavyAttackCollider.enabled = true;
    }

    public void HeavyAttackDisabled()
    {
        playerHitted = false;
        heavyAttackCollider.enabled = false;
        cooldownHeavyAttack = Random.Range(minCooldownTimeInclusive, maxCooldownTimeExclusive);
    }
    public void StartAttack()
    {
        attacking = true;
    }

    public void EndAttack()
    {
        attacking = false;

    }
}
