using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackCollider : MonoBehaviour
{
    private MeleeBT meleeBT;
    void Awake()
    {
        meleeBT = gameObject.GetComponentInParent<MeleeBT>();
    }
    public void OnTriggerEnter(Collider other)
    {
        meleeBT.AttackEnter(other);
    }
}
