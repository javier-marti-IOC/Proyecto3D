using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInMeleeRange : MonoBehaviour
{
    private MeleeBT enemy;
    void Awake()
    {
        enemy = gameObject.GetComponentInParent<MeleeBT>();
    }

    public void OnTriggerEnter(Collider other)
    {
        enemy.MeleeRangeTriggerEnter(other);
    }

    public void OnTriggerExit(Collider other)
    {
        enemy.MeleeRangeTriggerExit(other);
    }
}
