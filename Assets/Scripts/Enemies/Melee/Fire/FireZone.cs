using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireZone : MonoBehaviour
{
    private MeleeBT owner;
    private float damageInterval = 0.5f;
    private float timer;

    public void Initialize(MeleeBT owner)
    {
        this.owner = owner;
        timer = damageInterval;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Constants.player) && owner != null)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                owner.DealFireZoneDamage(other);
                timer = damageInterval;
            }
        }
    }

}