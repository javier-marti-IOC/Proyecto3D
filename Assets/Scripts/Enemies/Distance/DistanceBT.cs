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
    public float electricAttackRange = 8f;
    public GameObject impactPosition;
    public RayoController lightningEffectBasicAttack;
    public RayoController lightningEffectHeavyAttack;

    [Header("Electric Heavy Attack")]
    public ParticleSystem lightningArea1;
    public ParticleSystem lightningArea2;
    public ParticleSystem lightningArea3;
    public float heavyAttackDelay = 2f;
    public float lightningHeight = 10f;
    private Vector3 pendingHeavyAttackPosition;
    private ParticleSystem activeHeavyParticles;
    private ParticleSystem activeHeavyParticles2;
    public GameObject heavyAttackZoneTrigger;
    private bool isAttacking = false;
    private bool hitted = false;
    private bool canApplyHeavyDamage = false;

    [Header("WaterEnemy AudioSources")]
    public AudioSource audioWaterDeath;
    public AudioSource audioWaterHit;

    [Header("ElectricEnemy AudioSources")]
    public AudioSource audioElectricDeath;
    public AudioSource audioElectricBasicAttack;
    public AudioSource audioElectricHeavyAttack;
    public AudioSource audioElectricHit;
    public AudioSource audioElectricTeleport;

    // Start is called before the first frame update
    void Start()
    {

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
        if (!isBTEnabled) return;
        cooldownHeavyAttack -= Time.deltaTime;
        //Esta el enemigo vivo?
        if (healthPoints > 0)
        {
            if (!hitted)
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
                        if (!player.GetComponent<VikingController>().EnemyDetecion(this))
                        {
                            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), 1 * Time.deltaTime);
                            animator.SetInteger(Constants.state, 0);
                        }
                        else
                        {
                            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                            {
                                CheckLookingPlayer();

                                if (foundLookingPlayer)
                                {
                                    cooldownHeavyAttack -= Time.deltaTime;
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
                animator.SetInteger(Constants.state, 4);
            }

        }
        else
        {
            Dying(true);
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

            if (canApplyHeavyDamage)
            {
                PlayerHeavyHitted();
                canApplyHeavyDamage = false;
            }
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
            // Instancia sonido teleport
            audioElectricTeleport.Play();
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

        Vector3 direction = (impactPosition.transform.position - hand.position).normalized;
        RaycastHit hit;

        Vector3 endPoint;

        if (Physics.Raycast(hand.position, direction, out hit, electricAttackRange))
        {
            endPoint = hit.point;
            Debug.DrawRay(hand.position, direction * electricAttackRange, Color.red, 1f);
            audioElectricBasicAttack.Play();
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

        //Cridar a la funció del rayo Controller
        lightningEffectBasicAttack.PlayLightning(hand.position, endPoint, 0.1f);

        Invoke(nameof(EndEnemyAttack), 1.5f);
    }

    public void StartHeavyAttack()
    {
        if (player == null) return;

        isAttacking = true;
        agent.isStopped = true;

        // Guardar posició actual del player
        pendingHeavyAttackPosition = player.transform.position;

        // Instanciar particules
        activeHeavyParticles = Instantiate(lightningArea1, pendingHeavyAttackPosition, Quaternion.identity);
        activeHeavyParticles.Play();
        activeHeavyParticles2 = Instantiate(lightningArea2, pendingHeavyAttackPosition, Quaternion.identity);
        activeHeavyParticles2.Play();

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
        if (activeHeavyParticles2 != null)
        {
            Destroy(activeHeavyParticles2.gameObject);
        }

        // Definir inici i final del rayo
        Vector3 start = pendingHeavyAttackPosition + Vector3.up * lightningHeight;
        Vector3 end = pendingHeavyAttackPosition;

        //Cridar a la funció del rayo Controller
        lightningEffectHeavyAttack.PlayLightning(start, end, 0.1f);
        audioElectricHeavyAttack.Play();

        GameObject lightningFlash = new GameObject("LightningFlash");
        lightningFlash.transform.position = start;

        Light light = lightningFlash.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = new Color(1f, 229f / 255f, 0f); // #FFE500
        light.intensity = 20f;
        light.range = 50f;
        light.shadows = LightShadows.None;

        // Destruir la luz tras 0.1 segundos (aparición breve)
        Destroy(lightningFlash, 0.1f);

        Invoke(nameof(EnableHeavyAttackParticles), 0.1001f);

        canApplyHeavyDamage = true;

        if (isPlayerInHeavyAttackZone)
        {
            //Player rep pupa
            Debug.Log("Player hit by heavy electric attack");
            PlayerHeavyHitted();
        }

        Invoke(nameof(DisableHeavyAttackZone), 1f);
        //heavyAttackZoneTrigger.SetActive(false);
    }
    private void EndEnemyAttack()
    {
        isAttacking = false;
        agent.isStopped = false;
    }

    private void EnableHeavyAttackParticles()
    {
        activeHeavyParticles = Instantiate(lightningArea3, pendingHeavyAttackPosition, Quaternion.identity);
        activeHeavyParticles.Play();
        Invoke(nameof(DisableHeavyAttackParticles), 0.6f);
    }
    private void DisableHeavyAttackParticles()
    {
        Destroy(activeHeavyParticles.gameObject);
    }

    private void DisableHeavyAttackZone()
    {
        heavyAttackZoneTrigger.SetActive(false);
        canApplyHeavyDamage = false;
    }


    public void ResetHeavyAttackCooldown()
    {
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



    public override void HealthTaken(int damageTaken)
    {
        if (audioWaterHit != null)
        {
            audioWaterHit.Play();
        }
        else if (audioElectricHit != null)
        {
            audioElectricHit.Play();
        }

        base.HealthTaken(damageTaken);
        hitted = true;
        agent.isStopped = true;
        hitParticle.SetActive(true);
        if (ghost != null) Destroy(ghost);

        playerDetectorDown.SetActive(false);
        playerDetectorUp.SetActive(false);
        SetLookingPlayersActive(false);
        Invoke(nameof(SetFalseHitted), 0.5f);
    }
    // Método compatible con Animation Event
    public void SetHittedFalse()
    {
        // hitted = false;
        agent.isStopped = false;
        hitParticle.SetActive(false);
    }

    private void SetFalseHitted()
    {
        hitted = false;
    }
}
