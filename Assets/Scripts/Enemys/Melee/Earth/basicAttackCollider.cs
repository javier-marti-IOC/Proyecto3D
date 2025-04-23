using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackCollider : MonoBehaviour
{
    public MeleeBT earthBT;

    public void OnTriggerEnter(Collider other)
    {
        earthBT.basicAttackEnter(other);
    }
}
