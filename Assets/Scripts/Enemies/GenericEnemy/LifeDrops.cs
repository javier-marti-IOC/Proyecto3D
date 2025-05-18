using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeDrops : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            other.GetComponent<VikingController>().CollectLife();  
            Destroy(gameObject);
        }
    }
}
