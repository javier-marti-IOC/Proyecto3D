using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxDistanceChasing : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            if (other.GetComponent<Enemy>().activeElement == Element.Water)
            {
                other.GetComponent<DistanceBT>().ActivateDetectors(false);
            }
            else
            {
                other.gameObject.GetComponent<Enemy>().StopChasing();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.enemy) && other.GetComponent<Enemy>().activeElement == Element.Water)
            {
                other.GetComponent<DistanceBT>().ActivateDetectors(true);
            }
    }
}
