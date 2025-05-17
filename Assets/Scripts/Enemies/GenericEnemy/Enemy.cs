using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{

    public GameObject drop;
    public Transform dropPosition;
    public EnemyHUD enemyHUD;
    public GameObject hudPanelCanvas;
    public Element activeElement;
    public int healthPoints;
    public int maxHealthPoints = 100;
    public int enemyLevel = 1;
    protected GameObject player;
    public Tower tower;
    public GameManager gameManager;
    public GameObject ghostAgent;

    [Header("Animation ID")]
    private int animIDSpeed;
    protected Animator animator;

    [Header("Booleans")]
    public bool towerCalling; // Booleana para saber cuando la torre nos esta llamando
    protected bool onAction; // Esta realizando alguna accion
    protected bool onCombat; // El enemigo esta en combate
    protected bool onHealZone; // Esta el enemigo en zona de cura de la torre
    public bool playerInAttackRange; // Esta el player en mi zona de ataque
    [SerializeField] public bool towerInRange; // Tengo la torre en rango para patrullar
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
        gameManager = FindObjectOfType<GameManager>();
        animIDSpeed = Animator.StringToHash(Constants.speed);
        foreach (GameObject t in GameObject.FindGameObjectsWithTag(Constants.tower))
        {
            if (t.GetComponent<Tower>().activeElement == activeElement) tower = t.GetComponent<Tower>();
        }

    }

    // Metodos comunes
    // Indicamos virtual para poder sobreescribir el metodo si fuera necesario
    // O indicamos abstract para que las clases hijas o hereden si o si

    protected void CheckAgentSpeed()
    {
        animator.SetInteger(Constants.state, 0);
        animator.SetFloat(animIDSpeed, agent.velocity.magnitude);
    }

    protected void Patrol()
    {
        CheckAgentSpeed();

        if (ghost == null)
        {
            ghost = Instantiate(ghostPrefab, agent.transform.position, Quaternion.identity, agent.transform);
        }

        agent.stoppingDistance = safeDistance;

        ghost.GetComponent<RunnerGhostAgent>().patrolPoint = pointPatrol;
        agent.SetDestination(ghost.transform.position);
        agent.speed = ghost.GetComponent<NavMeshAgent>().speed - 1f;

        // animator.SetInteger(Constants.state, 1);

    }

    protected void TowerPatrol()
    {
        CheckAgentSpeed();

        if (ghost == null)
        {
            ghost = Instantiate(ghostPrefab, agent.transform.position, Quaternion.identity, agent.transform);
        }

        agent.stoppingDistance = safeDistance;

        ghost.GetComponent<RunnerGhostAgent>().patrolPoint = tower.gameObject;
        agent.SetDestination(ghost.transform.position);
        agent.speed = ghost.GetComponent<NavMeshAgent>().speed - 1f;

        // animator.SetInteger(Constants.state, 1);

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
        hudPanelCanvas.SetActive(false);
        playerDetectorDown.SetActive(true);
        playerDetectorUp.SetActive(true);
        minDistanceChase.SetActive(true);
        playerDetected = false;
        player.GetComponent<VikingController>().RemoveEnemyDetection(this);
    }
    protected virtual void TowerChase()
    {
        Destroy(ghost);
        playerDetectorDown.SetActive(false);
        playerDetectorUp.SetActive(false);
        minDistanceChase.SetActive(false);
        if (tower != null)
        {
            agent.SetDestination(tower.transform.position);
        }
    }
    public void StartAttack()
    {
        attacking = true;
    }

    public void EndAttack()
    {
        attacking = false;
    }
    public void HealthTaken(int damageTaken)
    {
        healthPoints -= damageTaken;
        enemyHUD.UpdateHealth(healthPoints);
        PlayerDetected();
    }
    public virtual void Dying()
    {
        Debug.Log("Muelto");
        if (tower != null)
        {
            tower.enemiesInSecondZoneRange.Remove(gameObject);
            tower.CheckSecondZoneCount(tower.enemiesInSecondZoneRange);
        }
        player.GetComponent<VikingController>().RemoveEnemyDetection(this);
        Instantiate(drop, dropPosition.position, Quaternion.identity, null);
        Destroy(transform.parent.gameObject);
    }

    public void PlayerDetected()
    {
        playerDetected = true;
        hudPanelCanvas.SetActive(true);
    }
}
