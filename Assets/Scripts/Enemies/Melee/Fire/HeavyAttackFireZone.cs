using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackFireZone : MonoBehaviour
{
    public MeleeBT meleeBT;

    void OnTriggerStay(Collider other)
    {
        meleeBT.HeavyAttackZoneStay(other);
    }
}
