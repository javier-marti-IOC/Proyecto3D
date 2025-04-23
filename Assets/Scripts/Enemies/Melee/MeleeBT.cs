using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeBT : Enemy
{

    void Update()
    {
        cooldownHeavyAttack -= Time.deltaTime;
        Debug.Log("C: " + cooldownHeavyAttack);
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
                    //gameObject.GetComponent<NavMeshAgent>().SetDestination(tower.transform.position);
                }
            }
            else
            {
                //El enemigo detecta al player
                if (playerDetected)
                {
                    if (/*player.GetComponent<PlayerController>().maxEmemies*/1==-1)
                    {
                        //Funcion quedar a la espera para combate

                    }
                    else
                    {
                        if (playerInAttackRange)
                        {
                            if (activeElement == Element.Earth)
                            {
                                Debug.Log(cooldownHeavyAttack);
                                if (cooldownHeavyAttack < 0)
                                {
                                    transform.LookAt(player.transform);
                                    animator.SetInteger(Constants.state,2);
                                }
                                else
                                {
                                    transform.LookAt(player.transform);
                                    animator.SetInteger(Constants.state,1);
                                }
                            }
                            else if (activeElement == Element.Fire)
                            {
                                
                            }
                        }
                        else
                        {
                            animator.SetInteger(Constants.state,0);
                            Chase();
                        }
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
