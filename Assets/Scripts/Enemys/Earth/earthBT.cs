using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EarthBT : Enemy
{

    // Start is called before the first frame update
    void Start()
    {
        healthPoints = 200;
        activeElement = Element.Earth;
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
                    //gameObject.GetComponent<NavMeshAgent>().SetDestination(tower.transform.position);
                }
            }
            else
            {
                //El enemigo detecta al player
                if (playerDetected)
                {
                    if (/*player.GetComponent<PlayerController>().maxEmemies*/playerDetected)
                    {
                        //Funcion quedar a la espera para combate

                    }
                    else
                    {
                        if (playerInAttackRange)
                        {
                            //Cada enemigo
                        }
                        else
                        {
                            Patrol();
                        }
                    }
                }
                else
                {
                    if (towerInRange)
                    {
                        TowerPatrol();
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
