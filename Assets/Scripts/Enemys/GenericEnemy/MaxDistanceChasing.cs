using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxDistanceChasing : MonoBehaviour
{
    private Enemy enemy;
    void Awake()
    {
        enemy = gameObject.GetComponentInParent<Enemy>();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            enemy.StopChasing();
        }
    }
}
