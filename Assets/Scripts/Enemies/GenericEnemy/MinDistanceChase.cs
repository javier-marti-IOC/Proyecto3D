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
                    Debug.Log(other.name);

        if (other.CompareTag(Constants.player))
        {
            Debug.Log("perseguir por cercania");
            enemy.Chase();
        }
    }
}
