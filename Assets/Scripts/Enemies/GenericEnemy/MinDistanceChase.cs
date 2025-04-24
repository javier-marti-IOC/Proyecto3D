using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinDistanceChase : MonoBehaviour
{
    private Enemy enemy;
    void Awake()
    {
        enemy = gameObject.GetComponentInParent<Enemy>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            enemy.playerDetected = true;
        }
    }
}
