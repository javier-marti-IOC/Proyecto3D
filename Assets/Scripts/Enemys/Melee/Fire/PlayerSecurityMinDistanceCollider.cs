using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSecurityMinDistanceCollider : MonoBehaviour
{
    public MeleeBT meleeBT;
    void OnTriggerEnter(Collider other)
    {
        meleeBT.PlayerSecurityMinDistanceColliderEnter(other);
    }
    void OnTriggerExit(Collider other)
    {
        meleeBT.PlayerSecurityMinDistanceColliderExit(other);
    }
}
