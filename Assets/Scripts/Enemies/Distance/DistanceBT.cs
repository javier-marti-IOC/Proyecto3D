using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class DistanceBT : Enemy
{
    private bool isPlayerInTeleportZone;

    [Header("Teleport Settings")]
    public float teleportCooldownTime;
    private float teleportCooldownTimer = 0f;
    public int teleportChance;
    public float teleportDistance;
    private float timerTeleportFunction = 0f;

    [Header("Chase")]
    public float stoppingDistance = 8f;
    public GameObject[] lookAtPlayers;
    private bool foundLookingPlayer = false;

    [Header("Electric Attack")]
    public Transform hand;
    public LineRenderer lightningLine;
    public Light attackLight;
    public float electricAttackRange = 10f;
    public int electricDamage = 20;

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

                    switch (activeElement)
                    {
                        case Element.Water:
                            if (cooldownHeavyAttack < 0)
                            {
                                // transform.LookAt(player.transform);
                                animator.SetInteger(Constants.state, 3);
                            }
                            else if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                            {
                                CheckLookingPlayer();

                                if (foundLookingPlayer)
                                {
                                    // SetLookingPlayersActive(false);
                                    Utils.RotatePositionToTarget(gameObject.transform, player.transform, 15f);
                                    animator.SetInteger(Constants.state, 2);
                                }
                                else
                                {
                                    // agent.radius = 5f;
                                    CheckAgentSpeed();
                                    // animator.SetInteger(Constants.state, 1);

                                    Chase(3f);
                                }
                            }
                            else
                            {
                                CheckAgentSpeed();
                                // animator.SetInteger(Constants.state, 1);

                                Chase(stoppingDistance);

                            }
                            break;
                        case Element.Electric:
                            //Funcionalidad enemigo electrico
                            if (isPlayerInTeleportZone)
                                {
                                    transform.LookAt(player.transform); // igual substituir per -> Utils.RotatePositionToTarget(transform, player.transform, 15f);
                                    teleportCooldownTimer -= Time.deltaTime;
                                    timerTeleportFunction += Time.deltaTime;

                                    if (timerTeleportFunction >= 1f)
                                    {
                                        timerTeleportFunction = 0f;
                                        int tp = TeleportProbability();
                                        Debug.Log("Teleport Probability: " + tp);

                                        bool cooldownReady = teleportCooldownTimer <= 0;
                                        bool luckyTeleport = tp <= teleportChance;

                                        if (cooldownReady || luckyTeleport)
                                        {
                                            TeleportToSafeZone();
                                            teleportCooldownTimer = teleportCooldownTime;
                                        }
                                        else
                                        {
                                            if (cooldownHeavyAttack < 0)
                                            {
                                                //transform.LookAt(player.transform);
                                                animator.SetInteger(Constants.state,3);
                                            }
                                            else
                                            {
                                                //transform.LookAt(player.transform);
                                                animator.SetInteger(Constants.state,2);
                                            }
                                        }
                                    }
                                }
                            else
                            {
                                if (cooldownHeavyAttack < 0)
                                {
                                    //transform.LookAt(player.transform);
                                    animator.SetInteger(Constants.state,3);
                                }
                                else
                                {
                                    //transform.LookAt(player.transform);
                                    animator.SetInteger(Constants.state,2);
                                }
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
                        SetLookingPlayersActive(false);
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
    private void SetLookingPlayersActive(bool active)
    {
        foreach (GameObject lookAtPlayer in lookAtPlayers)
        {
            if (lookAtPlayer.activeSelf != active)
            {
                lookAtPlayer.SetActive(active);
            }
        }
    }
    private void CheckLookingPlayer()
    {
        SetLookingPlayersActive(true);
        agent.SetDestination(player.transform.position);

        if (lookAtPlayers.Any(lookAtPlayer => lookAtPlayer.GetComponent<LookAtPlayer>().CentralRay()))
        {
            foundLookingPlayer = true;
        }
        else
        {
            foundLookingPlayer = false;
        }

    }


    public void TeleportZoneEnter(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            isPlayerInTeleportZone = true;
            Debug.Log("Player in teleportZone");
        }
    }
    public void TeleportZoneExit(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            isPlayerInTeleportZone = false;
            Debug.Log("Player out teleportZone");
        }
    }

    private int TeleportProbability ()
    {
        int tp = UnityEngine.Random.Range(0, 100);
        return tp;
    }

    private void TeleportToSafeZone()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * teleportDistance;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, teleportDistance, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            Debug.Log("Teletransportado a nueva posición: " + hit.position);
        }
        else
        {
            Debug.Log("No se encontró una zona segura para teletransportar.");
        }
    }

    public void ShootLightning()
    {
        if (player == null) return;

        Vector3 direction = (player.transform.position - hand.position).normalized;
        RaycastHit hit;

        Vector3 endPoint;

        if (Physics.Raycast(hand.position, direction, out hit, electricAttackRange))
        {
            endPoint = hit.point;
            Debug.DrawRay(hand.position, direction * electricAttackRange, Color.red, 1f);

            if (hit.collider.CompareTag(Constants.player))
            {
                // Fer pupa al player
                Debug.Log("Player hit with electric attack");
            }
        }
        else
        {
            endPoint = hand.position + direction * electricAttackRange;
        }

        lightningLine.SetPosition(0, hand.position);
        lightningLine.SetPosition(1, endPoint);
        lightningLine.enabled = true;

        attackLight.enabled = true;

        Invoke(nameof(DisableLightningVisuals), 0.1f);
    }

    private void DisableLightningVisuals()
    {
        lightningLine.enabled = false;
        attackLight.enabled = false;
    }

}
