using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaterBT : Enemy
{
    private bool playerInFront;
    private bool rotatePosition;
    public bool instanciaPrueba = false;

    // Start is called before the first frame update

    void Start()
    {
        base.Awake();
        activeElement = Element.Water;
    }

    //Update is called once per frame
    void Update()
    {
        if (instanciaPrueba)
        {
            Patrol();
        }
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
                    if (playerInAttackRange)
                    {
                        //Funcionalidad enemigo agua
                        if (playerInFront)
                        {
                            if (rotatePosition)
                            {
                                // Funcion de rotar la posicion
                            }
                            else
                            {
                                if (cooldownHeavyAttack < 0) // TODO Comprobar como se calcula 
                                {
                                    // Ataque basico
                                }
                                else
                                {
                                    // Genero cooldown de heavyAttack
                                    // Ataque fuerte
                                }
                            }
                        }
                        // Revisar la duplicidad de codigo en esta parte
                        else
                        {
                            if (cooldownHeavyAttack < 0) // TODO Comprobar como se calcula 
                            {
                                // Ataque basico
                            }
                            else
                            {
                                // Genero cooldown de heavyAttack
                                // Ataque fuerte
                            }
                        }
                    }
                    else
                    {
                        // Funcion de perseguir
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
                        // Patrol();
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
