using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnter : MonoBehaviour
{
    public VikingController vikingController;
    public void OnTriggerEnter(Collider other)
    {
        vikingController.AttackEnter(other);
    }
}
