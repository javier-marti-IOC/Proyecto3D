using TMPro;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{

    public GameObject manaDrop;
    public GameObject goldDrop;
    public GameObject lifeDrop;
    public GameObject deathParticle;
    public Transform dropPosition;
    public Transform deathPosition;
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
    protected GameObject particulas1;
    public GameObject hitNumberBasic;
    public GameObject hitNumberElemental;

    [Header("Animation ID")]
    private int animIDSpeed;
    protected Animator animator;

    [Header("Booleans")]
    public bool isBTEnabled;
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
    [SerializeField] protected float cooldownHeavyAttack; // Cooldown para volver a realizar ataque fuerte


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
    protected bool playerDetected; // Detecto al player
    [SerializeField] protected GameObject playerDetectorDown;
    [SerializeField] protected GameObject playerDetectorUp;

    [Header("Damages")]
    [SerializeField] protected int basicAttackBasicDamage;
    [SerializeField] protected int basicAttackElementalDamage;
    [SerializeField] protected int heavyAttackBasicDamage;
    [SerializeField] protected int heavyAttackElementalDamage;
    public GameObject hitParticle;


    protected virtual void Awake()
    {
        healthPoints = 100;
        player = GameObject.FindGameObjectWithTag(Constants.player);
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = safeDistance;
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        animIDSpeed = Animator.StringToHash(Constants.speed);
        /* foreach (GameObject t in GameObject.FindGameObjectsWithTag(Constants.tower))
        {
            if (t.GetComponent<Tower>().activeElement == activeElement) tower = t.GetComponent<Tower>();
        } */

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
        agent.SetDestination(player.transform.position);
    }
    public void Chase(float stoppingDistance)
    {
        Destroy(ghost);
        playerDetectorDown.SetActive(false);
        playerDetectorUp.SetActive(false);
        agent.SetDestination(player.transform.position);
        agent.stoppingDistance = stoppingDistance;
    }
    public void StopChasing()
    {
        hudPanelCanvas.SetActive(false);
        playerDetectorDown.SetActive(true);
        playerDetectorUp.SetActive(true);
        playerDetected = false;
        player.GetComponent<VikingController>().RemoveEnemyDetection(this);
    }
    protected virtual void TowerChase()
    {
        Destroy(ghost);
        playerDetectorDown.SetActive(false);
        playerDetectorUp.SetActive(false);
        if (tower != null)
        {
            agent.SetDestination(tower.transform.position);
            agent.stoppingDistance = 0;
            animator.SetInteger(Constants.state, 0);
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
    public virtual void HealthTaken(int[] damageTaken,Element element)
    {
        gameManager.ExitAttackHelp();
        Debug.Log("Basic damage taken: " + damageTaken[0] + " Elemental damage taken: " + damageTaken[1]);
        if (damageTaken[0] > 0)
        {
            hitNumberBasic.SetActive(false);
            hitNumberBasic.SetActive(true);
            hitNumberBasic.GetComponent<TextMeshProUGUI>().text = damageTaken[0] + "";
        }
        if (damageTaken[1] > 0)
        {
            hitNumberElemental.SetActive(false);
            hitNumberElemental.SetActive(true);
            hitNumberElemental.GetComponent<TextMeshProUGUI>().text = damageTaken[1] + "";
            if (element == Element.Earth)
            {
                hitNumberElemental.GetComponent<TextMeshProUGUI>().color = Color.green;
            }
            else if (element == Element.Water)
            {
                hitNumberElemental.GetComponent<TextMeshProUGUI>().color = Color.blue;
            }
            else if (element == Element.Fire)
            {
                hitNumberElemental.GetComponent<TextMeshProUGUI>().color = Color.red;
            }
            else if (element == Element.Electric)
            {
                hitNumberElemental.GetComponent<TextMeshProUGUI>().color = Color.yellow;
            }
        }
        healthPoints -= damageTaken[0] + damageTaken[1];
        enemyHUD.UpdateHealth(healthPoints);
        PlayerDetected();
    }

    public virtual void Dying(bool drops)
    {
        if (activeElement == Element.Electric && particulas1 != null)
        {
            Destroy(particulas1);
        }

        if (tower != null)
        {
            tower.enemiesInSecondZoneRange.Remove(gameObject);
            if (tower.enemiesInHealRange.Contains(gameObject))
            {
                tower.enemiesInHealRange.Remove(gameObject);
                tower.firstZoneContact = false;
            }
            tower.CheckSecondZoneCount(tower.enemiesInSecondZoneRange);
        }
        player.GetComponent<VikingController>().RemoveEnemyDetection(this);
        if (activeElement == Element.None)
        {
            AudioManager.Instance?.Play("goldDrop");
            Instantiate(goldDrop, dropPosition.position, Quaternion.identity, null);
        }
        else if (drops)
        {
            //for random
            int random = Random.Range(0, 100);
            if (random == 0)
            {
                AudioManager.Instance?.Play("goldDrop");
                Instantiate(goldDrop, dropPosition.position, Quaternion.identity, null);
            }
            else if (random < 20)
            {
                AudioManager.Instance?.Play("manaDrop");
                Instantiate(manaDrop, dropPosition.position, Quaternion.identity, null);
                Instantiate(manaDrop, dropPosition.position, Quaternion.identity, null);
                Instantiate(manaDrop, dropPosition.position, Quaternion.identity, null);
            }
            else if (random < 50)
            {
                AudioManager.Instance?.Play("manaDrop");
                Instantiate(manaDrop, dropPosition.position, Quaternion.identity, null);
                Instantiate(manaDrop, dropPosition.position, Quaternion.identity, null);
            }
            else
            {
                AudioManager.Instance?.Play("manaDrop");
                Instantiate(manaDrop, dropPosition.position, Quaternion.identity, null);
            }
            random = Random.Range(0, 100);
            if (random < 20)
            {
                Instantiate(lifeDrop, dropPosition.position, Quaternion.identity, null);
                Instantiate(lifeDrop, dropPosition.position, Quaternion.identity, null);
            }
            else if (random < 50)
            {
                Instantiate(lifeDrop, dropPosition.position, Quaternion.identity, null);
            }
        }
        // 💥 Partículas de muerte
        if (deathParticle != null)
        {
            Instantiate(deathParticle, deathPosition.position, Quaternion.identity, null);
        }
        Debug.Log("Destruir enemigo");
        Destroy(transform.parent.gameObject);
    }

    public void PlayerDetected()
    {
        cooldownHeavyAttack = Random.Range(minCooldownTimeInclusive, maxCooldownTimeExclusive);
        playerDetected = true;
        hudPanelCanvas.SetActive(true);
    }
    public void SetStatsByLevel()
    {
        // Earth: Mucha vida base con gran escalado
        // Daño base bajo con poco escalado, su daño no es ve afectado mucho por el elemento
        // Lv.1: [3,4,5,10,40], Lv.2: [5,6,8,13,70], Lv.3: [8,9,11,16,100], Lv.4: [10,11,14,19,130] 
        if (activeElement == Element.Earth)
        {
            basicAttackBasicDamage = 3;
            basicAttackElementalDamage = 4;

            heavyAttackBasicDamage = 5;
            heavyAttackElementalDamage = 10;

            maxHealthPoints = 40;
            for (int i = 1; i < enemyLevel; i++)
            {
                basicAttackBasicDamage += 2;
                basicAttackElementalDamage += 2;

                heavyAttackBasicDamage += 3;
                heavyAttackElementalDamage += 3;

                maxHealthPoints += 30;
            }
        }
        // Water: Vida estandard con escalado estandard
        // Daño base estandard con mucho escalado elemental
        // Lv.1: [2,3,5,6,30], Lv.2: [3,8,7,13,50], Lv.3: [4,13,9,20,70], Lv.4: [5,18,11,27,90] 
        else if (activeElement == Element.Water)
        {
            basicAttackBasicDamage = 2;
            basicAttackElementalDamage = 3;

            heavyAttackBasicDamage = 5;
            heavyAttackElementalDamage = 6;

            maxHealthPoints = 30;
            for (int i = 1; i < enemyLevel; i++)
            {
                basicAttackBasicDamage += 1;
                basicAttackElementalDamage += 5;

                heavyAttackBasicDamage += 2;
                heavyAttackElementalDamage += 7;

                maxHealthPoints += 20;
            }
        }
        // Fire: Vida estandard con buen escalado
        // Daño ataque basico estandard
        // El ataque fuerte hace 16 tics por lo que el daño despues de los 8s seria:
        // Lv.1: 48, Lv.2: 96, Lv.3: 144, Lv.4: 192. Sin contar elementos ni randoms. 
        // Lv.1: [3,4,1,2,30], Lv.2: [5,8,2,4,55], Lv.3: [7,12,3,5,80], Lv.4: [9,16,4,7,105]
        else if (activeElement == Element.Fire)
        {
            basicAttackBasicDamage = 3;
            basicAttackElementalDamage = 4;

            heavyAttackBasicDamage = 1;
            heavyAttackElementalDamage = 2;

            maxHealthPoints = 30;
            for (int i = 1; i < enemyLevel; i++)
            {
                basicAttackBasicDamage += 2;
                basicAttackElementalDamage += 4;

                heavyAttackBasicDamage += 1;
                heavyAttackElementalDamage += 2;

                maxHealthPoints += 25;
            }
        }
        // Electric: Poca vida con mal esacalado
        // Daño de ataque basico flojo y mal escalado
        // Ataque fuerte con buen daño base y escalado, sin recaer demasiado en daño elemental
        // Lv.1: [2,3,15,25,25], Lv.2: [4,6,25,35,40], Lv.3: [6,9,35,45,55], Lv.4: [8,12,45,55,70]
        else if (activeElement == Element.Electric)
        {
            basicAttackBasicDamage = 2;
            basicAttackElementalDamage = 3;

            heavyAttackBasicDamage = 15;
            heavyAttackElementalDamage = 25;

            maxHealthPoints = 25;
            for (int i = 1; i < enemyLevel; i++)
            {
                basicAttackBasicDamage += 2;
                basicAttackElementalDamage += 3;

                heavyAttackBasicDamage += 10;
                heavyAttackElementalDamage += 10;

                maxHealthPoints += 15;
            }
        }
        healthPoints = maxHealthPoints;
    }
}
