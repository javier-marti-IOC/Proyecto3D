using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackCollider : MonoBehaviour
{
    private MeleeBT meleeBT;
    void Awake()
    {
        meleeBT = gameObject.GetComponentInParent<MeleeBT>();
        GetComponent<Collider>().enabled = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        meleeBT.EarthHeavyAttackEnter(other);
    }
}
