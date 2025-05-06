using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackCollider : MonoBehaviour
{
    public MeleeBT earthBT;
    private Enemy enemy;
    void Awake()
    {
        enemy = gameObject.GetComponentInParent<Enemy>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.player) && !enemy.playerHitted)
        {
            enemy.playerHitted = true;
            //player.GetComponent<tempPlayer>().healthPoints -= gameManager.DamageCalulator(activeElement,earthBasicAttackBasicDamage,earthBasicAttackElementalDamage,player.GetComponent<tempPlayer>().activeElement);
        }
    }
}
