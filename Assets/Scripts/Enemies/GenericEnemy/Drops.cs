using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drops : MonoBehaviour
{
    public Element element;


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            other.GetComponent<VikingController>().CollectMana(element);  
            Destroy(gameObject);
        }
    }
}
