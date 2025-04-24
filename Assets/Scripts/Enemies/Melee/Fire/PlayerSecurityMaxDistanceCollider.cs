using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSecurityMaxDistanceCollider : MonoBehaviour
{
    public MeleeBT meleeBT;
    void OnTriggerEnter(Collider other)
    {
        meleeBT.PlayerSecurityMaxDistanceColliderEnter(other);
    }
    void OnTriggerExit(Collider other)
    {
        meleeBT.PlayerSecurityMaxDistanceColliderExit(other);
    }
}
