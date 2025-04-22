using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackCollider : MonoBehaviour
{
    public MeleeBT earthBT;

    void Update()
    {
        Debug.Log(gameObject.GetComponent<BoxCollider>().enabled);
    }
    public void OnTriggerEnter(Collider other)
    {
        earthBT.basicAttackEnter(other);
    }
}
