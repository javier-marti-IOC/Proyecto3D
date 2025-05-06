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
        if (other.CompareTag(Constants.player))
        {
            enemy.playerInAttackRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            enemy.playerInAttackRange = false;
        }
    }
}
