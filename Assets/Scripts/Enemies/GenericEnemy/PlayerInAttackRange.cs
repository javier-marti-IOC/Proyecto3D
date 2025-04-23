using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAttackRange : MonoBehaviour
{
    private Enemy enemy;
    void Awake()
    {
        enemy = gameObject.GetComponentInParent<Enemy>();
    }

    public void OnTriggerEnter(Collider other)
    {
        enemy.PlayerInAttackRangeEnter(other);
    }

    public void OnTriggerExit(Collider other)
    {
        enemy.PlayerInAttackRangeExit(other);
    }
}
