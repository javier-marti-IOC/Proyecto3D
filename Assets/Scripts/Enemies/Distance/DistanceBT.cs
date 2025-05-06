using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class DistanceBT : Enemy
{
    private bool isPlayerInTeleportZone;

    [Header("Teleport Settings")]
    public float teleportCooldownTime = 5f;
    private float teleportCooldownTimer = 0f;
    public int teleportChance = 15;
    public float teleportDistance = 5f;
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
                    Debug.Log("WaterEnemy esta: " + agent.isStopped);
                    switch (activeElement)
                    {
                        case Element.Water:
                            if (cooldownHeavyAttack < 0)
                            {
                                transform.LookAt(player.transform);
                                animator.SetInteger(Constants.state, 3);
                            }
                            else if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                            {
                                // agent.isStopped = true;
                                // Invoke(nameof(ResumeWalking), 2f);
                                animator.SetInteger(Constants.state, 2);

                            }
                            else
                            {

                                Chase();

                            }
                            break;
                        case Element.Electric:
                            //Funcionalidad enemigo electrico
                            if (isPlayerInTeleportZone)
                            {
                                bool cooldownReady = teleportCooldownTimer <= 0;
                                bool luckyTeleport = UnityEngine.Random.Range(0, 100) < teleportChance;

                                if (cooldownReady && luckyTeleport)
                                {
                                    TeleportToSafeZone();
                                    teleportCooldownTimer = teleportCooldownTime;
                                }
                            }
                            else
                            {

                            }
                            break;
                        default:
                            break;
                    }

                }
                else
                {
                    if (towerInRange)
                    {
                        TowerChase();
                    }
                    else /* if (rotating)
                    {
                        Rotate();
                    } */
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
    public void TeleportZoneEnter(Collider other)
    {
        if (other.tag.Equals(Constants.player))
        {
            isPlayerInTeleportZone = true;
            Debug.Log("Player in teleportZone");
        }
    }
    public void TeleportZoneExit(Collider other)
    {
        if (other.tag.Equals(Constants.player))
        {
            isPlayerInTeleportZone = false;
        }
    }

    private void TeleportToSafeZone()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * teleportDistance;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, teleportDistance, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            Debug.Log("Teletransportado a nueva posición: " + hit.position);
        }
        else
        {
            Debug.Log("No se encontró una zona segura para teletransportar.");
        }
    }

    private void ResumeWalking()
    {
        agent.isStopped = false;
    }
}
