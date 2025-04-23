using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DistanceBT : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.Awake();
    }

    //Update is called once per frame
    void Update()
    {
        //Esta el enemigo vivo?
        if (healthPoints > 0)
        {
            //ME ESTA LLAMANDO LA TORRE?
            if (towerCalling)
            {
                //Estoy en la zona de curacion?
                if (onHealZone)
                {
                    //Esta la torre en cooldown
                    if (tower.GetComponent<TowerBT>().onCooldown)
                    {
                        towerCalling = false;
                    }
                    else
                    {
                        Heal();
                    }
                }
                else
                {
                    //Me acerco
                    TowerChase();
                }
            }
            else
            {
                //El enemigo detecta al player
                if (playerDetected)
                {
                    if (playerInAttackRange)
                    {
                        switch (activeElement)
                        {
                            case Element.Water:
                                Debug.Log(cooldownHeavyAttack);
                                if (cooldownHeavyAttack < 0)
                                {
                                    transform.LookAt(player.transform);
                                    animator.SetInteger(Constants.state, 2);
                                }
                                else
                                {
                                    transform.LookAt(player.transform);
                                    animator.SetInteger(Constants.state, 1);
                                }
                                break;
                            case Element.Electric:
                                //Hacer funcionalidad de Electric
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        animator.SetInteger(Constants.state, 0);
                        Chase();
                    }

                }
                else
                {
                    if (towerInRange)
                    {
                        TowerChase();
                    }
                    else
                    {
                        Patrol();
                    }
                }
            }
        }
        else
        {
            Destroy(this);
        }
    }
}
