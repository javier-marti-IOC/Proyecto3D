using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEnemy : MonoBehaviour
{
    public Element selectedType;

    void Update()
    {
        if(Input.GetKey(KeyCode.J))
        {
            Debug.Log("---> ELEMENTO: " + selectedType);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Tower"))
        {   
            Debug.Log("TOCANDO LA TORRE");
        }
    }

}
