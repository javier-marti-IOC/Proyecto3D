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
    private bool isPlayerInHeavyAttackZone;

    [Header("Teleport Settings")]
    public float teleportCooldownTime;
    private float teleportCooldownTimer = 0f;
    public int teleportChance;
    public float teleportDistance;
    private float timerTeleportFunction = 0f;
    public ParticleSystem teleportParticles;

    [Header("Chase")]
    public float stoppingDistance = 8f;
    public GameObject[] lookAtPlayers;
    private bool foundLookingPlayer = false;

    [Header("Electric Basic Attack")]
    public Transform hand;
    public LineRenderer lightningLine;
    public float electricAttackRange = 8f;
    public GameObject impactPosition;

    [Header("Electric Heavy Attack")]
    public ParticleSystem lightningArea;
    public float heavyAttackDelay = 2f;
    public float lightningHeight = 10f;
    private Vector3 pendingHeavyAttackPosition;
    private ParticleSystem activeHeavyParticles;
    public GameObject heavyAttackZoneTrigger;
    private bool isAttacking = false;


    // Start is called before the first frame update


    void Start()
    {
        base.Awake();

        if (activeElement == Element.Electric)
        {
            Transform spine1 = player.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "mixamorig:Spine2");
            impactPosition = spine1.gameObject;
            heavyAttackZoneTrigger.SetActive(false);
        }
    }

    //Update is called once per frame
    void Update()
    {
        cooldownHeavyAttack -= Time.deltaTime;
        Debug.Log("PLAYER DETECTED?: " + playerDetected);
        //Esta el enemigo vivo?
        if (healthPoints > 0)
        {
            //ME ESTA LLAMANDO LA TORRE?
            if (towerCalling)
            {
                TowerChase();
            }
            else
            {
                //El enemigo detecta al player
                if (playerDetected)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                    {
                        CheckLookingPlayer();

                        if (foundLookingPlayer)
                        {
                            // SetLookingPlayersActive(false);
                            Utils.RotatePositionToTarget(gameObject.transform, player.transform, 15f);
                            switch (activeElement)
                            {
                                case Element.Water:
                                    if (cooldownHeavyAttack <= 0)
                                    {
                                        //transform.LookAt(player.transform);
                                        animator.SetInteger(Constants.state, 3);
                                    }
                                    else
                                    {
                                        //transform.LookAt(player.transform);
                                        animator.SetInteger(Constants.state, 2);
                                    }
                                    break;
                                case Element.Electric:
                                    //Funcionalidad enemigo electrico
                                    if (isPlayerInTeleportZone)
                                    {
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
                                                if (cooldownHeavyAttack <= 0)
                                                {
                                                    //transform.LookAt(player.transform);
                                                    animator.SetInteger(Constants.state, 3);
                                                }
                                                else
                                                {
                                                    //transform.LookAt(player.transform);
                                                    animator.SetInteger(Constants.state, 2);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (cooldownHeavyAttack <= 0)
                                        {
                                            //transform.LookAt(player.transform);
                                            animator.SetInteger(Constants.state, 3);
                                        }
                                        else
                                        {
                                            //transform.LookAt(player.transform);
                                            animator.SetInteger(Constants.state, 2);
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
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


                }
                else
                {
                    SetLookingPlayersActive(false);
                    Patrol();
                }
            }
        }
        else
        {
            Dying();
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
        if (!isAttacking)
        {
            agent.SetDestination(player.transform.position);
        }

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

    public void HeavyAttackZoneEnter(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            isPlayerInHeavyAttackZone = true;
            Debug.Log("Player in Heavy Attack 2Zone");
        }
    }
    public void HeavyAttackZoneExit(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            isPlayerInHeavyAttackZone = false;
            Debug.Log("Player out Heavy Attack Zone");
        }
    }

    private int TeleportProbability()
    {
        int tp = UnityEngine.Random.Range(0, 100);
        return tp;
    }

    private void TeleportToSafeZone()
    {
        Vector3 originalPosition = transform.position;
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * teleportDistance;
        randomDirection += originalPosition;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, teleportDistance, NavMesh.AllAreas))
        {
            // Instanciar partículas donde desaparece
            Instantiate(teleportParticles, originalPosition, Quaternion.identity).Play();

            // Mover al enemigo
            transform.position = hit.position;

            // Instanciar partículas donde aparece
            Instantiate(teleportParticles, hit.position, Quaternion.identity).Play();

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

        isAttacking = true;
        agent.isStopped = true;

        //Vector3 direction = (player.transform.position - hand.position).normalized;
        Vector3 direction = (impactPosition.transform.position - hand.position).normalized;
        RaycastHit hit;

        Vector3 endPoint;

        if (Physics.Raycast(hand.position, direction, out hit, electricAttackRange))
        {
            endPoint = hit.point;
            Debug.DrawRay(hand.position, direction * electricAttackRange, Color.red, 1f);

            if (hit.collider.CompareTag(Constants.player))
            {
                // Fer pupa al player
                PlayerHitted();
                Debug.Log("Player hit with electric basic attack");
            }
        }
        else
        {
            endPoint = hand.position + direction * electricAttackRange;
        }

        lightningLine.SetPosition(0, hand.position);
        lightningLine.SetPosition(1, endPoint);
        lightningLine.enabled = true;

        Invoke(nameof(EndEnemyAttack), 1.5f);
        Invoke(nameof(DisableLightningVisuals), 0.1f);
    }

    public void StartHeavyAttack()
    {
        if (player == null) return;

        isAttacking = true;
        agent.isStopped = true;

        // Guardar posició actual del player
        pendingHeavyAttackPosition = player.transform.position;

        // Instanciar particules
        activeHeavyParticles = Instantiate(lightningArea, pendingHeavyAttackPosition, Quaternion.identity);
        activeHeavyParticles.Play();

        // Activar la zona
        Transform zone = heavyAttackZoneTrigger.transform;
        zone.position = pendingHeavyAttackPosition + Vector3.up * 0.01f;
        zone.gameObject.SetActive(true);

        // Executar atac amb delay
        Invoke(nameof(ExecuteHeavyAttack), heavyAttackDelay);
        Invoke(nameof(EndEnemyAttack), heavyAttackDelay + 0.3f);
    }

    private void ExecuteHeavyAttack()
    {
        // Desactivar particules que hagin pogut quedar
        if (activeHeavyParticles != null)
        {
            Destroy(activeHeavyParticles.gameObject);
        }

        // Definir inici i final del rayo
        Vector3 start = pendingHeavyAttackPosition + Vector3.up * lightningHeight;
        Vector3 end = pendingHeavyAttackPosition;

        // Posició inicial i final visualment
        lightningLine.SetPosition(0, start);
        lightningLine.SetPosition(1, end);
        lightningLine.enabled = true; // Activar visualització

        Invoke(nameof(DisableLightningVisuals), 0.1f);

        if (isPlayerInHeavyAttackZone)
        {
            //Player rep pupa
            Debug.Log("Player hit by heavy electric attack");
            PlayerHeavyHitted();
        }
        heavyAttackZoneTrigger.SetActive(false);
    }

    private void DisableLightningVisuals()
    {
        lightningLine.enabled = false;
    }

    private void EndEnemyAttack()
    {
        isAttacking = false;
        agent.isStopped = false;
    }

    public void ResetHeavyAttackCooldown()
    {
        //playerHitted = false;
        cooldownHeavyAttack = UnityEngine.Random.Range(minCooldownTimeInclusive, maxCooldownTimeExclusive);
    }

    public void PlayerHitted()
    {
        player.GetComponent<VikingController>().HealthTaken(gameManager.DamageCalulator(activeElement, basicAttackBasicDamage, basicAttackElementalDamage, player.GetComponent<VikingController>().activeElement));

    }

    public void PlayerHeavyHitted()
    {
        player.GetComponent<VikingController>().HealthTaken(gameManager.DamageCalulator(activeElement, heavyAttackBasicDamage, heavyAttackElementalDamage, player.GetComponent<VikingController>().activeElement));
    }

    public override void Dying()
    {
        Debug.Log("Muelto");
        //tower.enemiesInSecondZoneRange.Remove(gameObject);
        //tower.CheckSecondZoneCount(tower.enemiesInSecondZoneRange);
        Instantiate(drop, dropPosition.position, Quaternion.identity, null);
        Utils.DestroyRoot(gameObject);
    }
}
