using UnityEngine;
using UnityEngine.AI;

public class EnemyAreaConfigurator : Enemy
{

    private float updateInterval = 0.2f;
    private float timer;
    private int oppositeAreaIndex;

    void Start()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();

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
        switch (activeElement)
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
            agent.SetDestination(player.transform.position);
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
