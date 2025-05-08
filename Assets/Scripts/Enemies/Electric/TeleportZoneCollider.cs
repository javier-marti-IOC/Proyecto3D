using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public DistanceBT distanceBT;

    void OnTriggerEnter(Collider other)
    {
        distanceBT.TeleportZoneEnter(other);
    }

    void OnTriggerExit(Collider other)
    {
        distanceBT.TeleportZoneExit(other);
    }
}
