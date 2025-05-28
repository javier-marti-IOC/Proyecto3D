using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinDistance : MonoBehaviour
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
            enemy.PlayerDetected();
        }
    }
}
