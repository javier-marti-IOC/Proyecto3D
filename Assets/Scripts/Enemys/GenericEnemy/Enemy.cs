using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{

    protected Element activeElement;
    protected int healthPoints;
    protected GameObject player;
    protected GameObject tower;
    public GameManager gameManager;
    public GameObject ghostAgent;
    protected Animator animator;


    [Header("Booleans")]
    protected bool towerCalling; // Booleana para saber cuando la torre nos esta llamando
    protected bool onAction; // Esta realizando alguna accion
    protected bool onCombat; // El enemigo esta en combate
    protected bool onHealZone; // Esta el enemigo en zona de cura de la torre
    protected bool playerInAttackRange; // Esta el player en mi zona de ataque
    protected bool towerInRange; // Tengo la torre en rango para patrullar

    [Header("Ranges")]
    protected int minCooldownTimeInclusive; /* Tiempo minimo inclusivo del rango 
                                            (este numero si entra en el rango)*/
    protected int maxCooldownTimeExclusive; /* Tiempo maximo exclusivo del rango 
                                            (este numero no entra en el rango)*/

    [Header("Cooldowns")]
    protected float cooldownHeavyAttack; // Cooldown para volver a realizar ataque fuerte


    [Header("Patrol")]
    protected NavMeshAgent agent;
    protected GameObject ghost;
    [SerializeField] protected GameObject ghostPrefab;
    [SerializeField] protected float safeDistance = 1f;  // Distancia prudencial que se quiere mantener

    [Header("Chase")]
    public bool playerDetected; // Detecto al player
    [SerializeField] protected GameObject playerDetector;
    [SerializeField] protected GameObject minDistanceChase;



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

        agent.SetDestination(ghost.transform.position);
        agent.speed = ghost.GetComponent<NavMeshAgent>().speed - 1f;
        
        animator.SetInteger("Anim",0);
        ghostAgent.GetComponent<RunnerAgent>().chase = false;
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
    protected virtual void TowerPatrol()
    {

    }



}
