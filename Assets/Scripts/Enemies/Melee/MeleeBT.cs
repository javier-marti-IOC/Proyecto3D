using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeBT : Enemy
{
    private bool playerInSecurityDistance;
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
                                // Esta el player usando el elemento de agua
                                if(player.GetComponent<tempPlayer>().activeElement == Element.Water)
                                {
                                    // Esta a una distancia prudencial del player?
                                    if(playerInSecurityDistance)
                                    {
                                        // Tiene cooldown de ataque en area?
                                        if (cooldownHeavyAttack < 0)
                                        {
                                            transform.LookAt(player.transform);
                                            animator.SetInteger("Anim",2);
                                        }
                                        else
                                        {
                                            // Se queda mirandolo
                                            transform.LookAt(player.transform);
                                        }
                                    }
                                    else
                                    {
                                        // Alejarse
                                        Vector3 direction = transform.position - player.transform.position; // Ir en direccion contraria
                                        Vector3 newPosition = transform.position + direction * 0.1f; // Calcula la nueva posicion en la direccion opuesta
                                        agent.SetDestination(newPosition); 
                                    }

                                } else
                                { // Tiene cooldown de ataque en area?
                                    if (cooldownHeavyAttack < 0)
                                    {
                                        transform.LookAt(player.transform);
                                        animator.SetInteger("Anim",2);
                                    }
                                    else
                                    {
                                        transform.LookAt(player.transform);
                                        animator.SetInteger("Anim",1);
                                    }
                                }
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
    public void PlayerSecurityMinDistanceColliderEnter(Collider other)
    {
        if(other.tag.Equals(Constants.player))
        {
            playerInSecurityDistance = false;
        }
    }
    public void PlayerSecurityMinDistanceColliderExit(Collider other)
    {
        if(other.tag.Equals(Constants.player))
        {
            playerInSecurityDistance = true;
        }
    }
    public void PlayerSecurityMaxDistanceColliderEnter(Collider other)
    {
        if(other.tag.Equals(Constants.player))
        {
            playerInSecurityDistance = true;
        }
    }
    public void PlayerSecurityMaxDistanceColliderExit(Collider other)
    {
        if(other.tag.Equals(Constants.player))
        {
            playerInSecurityDistance = false;
        }
    }
}
