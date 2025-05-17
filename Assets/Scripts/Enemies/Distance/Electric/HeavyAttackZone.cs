using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackZone : MonoBehaviour
{
    public DistanceBT distanceBT;

    void OnTriggerEnter(Collider other)
    {
        distanceBT.HeavyAttackZoneEnter(other);
    }

    void OnTriggerExit(Collider other)
    {
        distanceBT.HeavyAttackZoneExit(other);
    }
}
