using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinDistanceChase : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            other.gameObject.GetComponent<Enemy>().PlayerDetected();
        }
    }
}
