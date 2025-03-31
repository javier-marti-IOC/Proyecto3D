using UnityEngine;
using UnityEngine.AI;

public enum ElementType { Water, Fire, Rock, Electricity }

public class EnemyElementalAI : MonoBehaviour
{
    [Header("Enemy Settings")]
    public ElementType enemyElement;

    [Header("Movement Settings")]
    public float normalSpeed = 3.5f;
    public float slowedSpeed = 1.5f;

    [Header("NavMesh Area Costs")]
    public float defaultAreaCost = 1f;
    public float oppositeAreaCost = 10f;

    private NavMeshAgent agent;
    private Transform player;
    private float updateInterval = 0.2f;
    private float timer;
    private int oppositeAreaIndex;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj)
            player = playerObj.transform;

        agent.speed = normalSpeed;
        ConfigureAreaCosts();
    }

    void ConfigureAreaCosts()
    {
        // Set default area costs
        agent.SetAreaCost(NavMesh.GetAreaFromName("Walkable"), defaultAreaCost);
        agent.SetAreaCost(NavMesh.GetAreaFromName("Water"), defaultAreaCost);
        agent.SetAreaCost(NavMesh.GetAreaFromName("Fire"), defaultAreaCost);
        agent.SetAreaCost(NavMesh.GetAreaFromName("Rock"), defaultAreaCost);
        agent.SetAreaCost(NavMesh.GetAreaFromName("Electricity"), defaultAreaCost);

        // Assign opposite area based on enemy element
        switch (enemyElement)
        {
            case ElementType.Water:
                oppositeAreaIndex = NavMesh.GetAreaFromName("Electricity");
                break;
            case ElementType.Fire:
                oppositeAreaIndex = NavMesh.GetAreaFromName("Water");
                break;
            case ElementType.Rock:
                oppositeAreaIndex = NavMesh.GetAreaFromName("Fire");
                break;
            case ElementType.Electricity:
                oppositeAreaIndex = NavMesh.GetAreaFromName("Rock");
                break;
        }

        agent.SetAreaCost(oppositeAreaIndex, oppositeAreaCost);
    }

    void Update()
    {
        if (player == null) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            agent.SetDestination(player.position);
            timer = updateInterval;
        }

        AdjustSpeedBasedOnArea();
    }

    void AdjustSpeedBasedOnArea()
    {
        NavMeshHit hit;
        agent.SamplePathPosition(NavMesh.AllAreas, 0.0f, out hit);

        if ((hit.mask & (1 << oppositeAreaIndex)) != 0)
        {
            agent.speed = slowedSpeed;  // Área contraria → más lento claramente
        }
        else
        {
            agent.speed = normalSpeed;  // Área normal → velocidad normal claramente
        }
    }
}
