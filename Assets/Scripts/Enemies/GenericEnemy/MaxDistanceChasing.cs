using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxDistanceChasing : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            other.gameObject.GetComponent<Enemy>().StopChasing();
        }
    }
}
