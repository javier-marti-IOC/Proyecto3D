using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private DistanceBT distanceBT;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TeleportZone"))
        {
            distanceBT.TeleportZoneEnter(other);
        }
    }

    void OiggerExit(Collider other)
    {
        if (other.CompareTag("TeleportZone"))
        {
            distanceBT.TeleportZoneExit(other);
        }
    }
}
