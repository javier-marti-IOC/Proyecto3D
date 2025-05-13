using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealZone : MonoBehaviour
{
    public Tower tower;
    public void OnTriggerEnter(Collider other)
    {
        tower.OnTriggerHealEnter(other);
    }
    public void OnTriggerExit(Collider other)
    {
        tower.OnTriggerHealExit(other);
    }

    public void OnTriggerStay(Collider other)
    {
        tower.OnTriggerHealStay(other);
    }
}
