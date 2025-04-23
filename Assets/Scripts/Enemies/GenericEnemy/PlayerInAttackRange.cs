using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAttackRange : MonoBehaviour
{
    public Enemy enemy;

    public void OnTriggerEnter(Collider other)
    {
        enemy.PlayerInAttackRangeEnter(other);
    }

    public void OnTriggerExit(Collider other)
    {
        enemy.PlayerInAttackRangeExit(other);
    }
}
