using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBTEnabler : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            other.gameObject.GetComponent<Enemy>().isBTEnabled = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            other.gameObject.GetComponent<Enemy>().isBTEnabled = false;
        }
    }
}
