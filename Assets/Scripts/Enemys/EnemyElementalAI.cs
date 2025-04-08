using UnityEngine;
using UnityEngine.AI;

public class EnemyElementalAI : MonoBehaviour
{
    [Header("Enemy Settings")]
    public Element enemyElement;

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
        GameObject playerObj = GameObject.FindGameObjectWithTag(Constants.player);
        if (playerObj)
            player = playerObj.transform;

        agent.speed = normalSpeed;
        ConfigureAreaCosts();
    }

    void ConfigureAreaCosts()
    {
        // Set default area costs
        agent.SetAreaCost(NavMesh.GetAreaFromName(Constants.walkable), defaultAreaCost);
        agent.SetAreaCost(NavMesh.GetAreaFromName(Constants.water), defaultAreaCost);
        agent.SetAreaCost(NavMesh.GetAreaFromName(Constants.fire), defaultAreaCost);
        agent.SetAreaCost(NavMesh.GetAreaFromName(Constants.earth), defaultAreaCost);
        agent.SetAreaCost(NavMesh.GetAreaFromName(Constants.electric), defaultAreaCost);

        // Assign opposite area based on enemy element
        switch (enemyElement)
        {
            case Element.Water:
                oppositeAreaIndex = NavMesh.GetAreaFromName(Constants.electric);
                break;
            case Element.Fire:
                oppositeAreaIndex = NavMesh.GetAreaFromName(Constants.water);
                break;
            case Element.Earth:
                oppositeAreaIndex = NavMesh.GetAreaFromName(Constants.fire);
                break;
            case Element.Electric:
                oppositeAreaIndex = NavMesh.GetAreaFromName(Constants.earth);
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
