using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class MeleeBT : Enemy
{
    private bool playerInSecurityDistance;
    [Header("Collider")]
    [SerializeField] protected Collider basicAttackCollider;
    [SerializeField] protected Collider heavyAttackCollider;

    public GameObject heavyAttackFireZoneTrigger;

    private bool isAttacking;
    private bool fireZoneArea = false;
    private Vector3 pendingHeavyAttackPosition;
    public float heavyAttackDelay = 2f;
    private float timerFireStay = 0.5f;

    void Start()
    {
        if (activeElement == Element.Fire)
        {
            //Transform spine1 = player.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "mixamorig:Spine2");
            //impactPosition = spine1.gameObject;
            heavyAttackFireZoneTrigger.SetActive(false);
        }
    }
    void Update()
    {
        cooldownHeavyAttack -= Time.deltaTime;
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
                }
                else
                {
                    //Me acerco
                    //gameObject.GetComponent<NavMeshAgent>().SetDestination(tower.transform.position);
                    TowerChase();
                }
            }
            else
            {
                //El enemigo detecta al player
                Debug.Log("playerDetected " + playerDetected);
                if (playerDetected)
                {
                    //if (!player.GetComponent<VikingController>().EnemyDetecion(this))
                    if (1 == 0)
                    {
                        Debug.Log("NO");
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), 1 * Time.deltaTime);
                        animator.SetInteger(Constants.state, 0);
                    }
                    else
                    {
                        if (playerInAttackRange)
                        {
                            if (activeElement == Element.Earth)
                            {
                                if (cooldownHeavyAttack < 0)
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
                            else if (activeElement == Element.Fire)
                            {
                                // Esta el player usando el elemento de agua
                                if (player.GetComponent<VikingController>().activeElement == Element.Water)
                                {
                                    // Esta a una distancia prudencial del player?
                                    if (playerInSecurityDistance)
                                    {
                                        // Tiene cooldown de ataque en area?
                                        if (cooldownHeavyAttack < 0)
                                        {
                                            //transform.LookAt(player.transform);
                                            animator.SetInteger(Constants.state, 3);
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

                                }
                                else
                                { // Tiene cooldown de ataque en area?
                                    if (cooldownHeavyAttack < 0)
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
                            if (!attacking)
                            {
                                CheckAgentSpeed();
                                Chase();
                            }
                        }
                    }
                }
                else
                {
                    CheckAgentSpeed();
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
            Dying();
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

    public void BasicAttackActivated()
    {
        playerHitted = false;
        basicAttackCollider.enabled = true;
    }

    public void BasicAttackDisabled()
    {
        playerHitted = false;
        basicAttackCollider.enabled = false;
    }

    public void HeavyAttackActivated()
    {
        playerHitted = false;
        heavyAttackCollider.enabled = true;
    }

    public void HeavyAttackDisabled()
    {
        playerHitted = false;
        heavyAttackCollider.enabled = false;
        cooldownHeavyAttack = Random.Range(minCooldownTimeInclusive, maxCooldownTimeExclusive);
    }
    
        public void ResetHeavyAttackCooldown()
    {
        cooldownHeavyAttack = Random.Range(minCooldownTimeInclusive, maxCooldownTimeExclusive);
    }

    public void StartHeavyAttack()
    {
        if (player == null) return;

        isAttacking = true;
        agent.isStopped = true;

        // Guardar posició actual del player
        pendingHeavyAttackPosition = player.transform.position;

        // Instanciar particules
        //activeHeavyParticles = Instantiate(lightningArea, pendingHeavyAttackPosition, Quaternion.identity);
        //activeHeavyParticles.Play();

        // Activar la zona
        Transform zone = heavyAttackFireZoneTrigger.transform;
        zone.position = pendingHeavyAttackPosition + Vector3.up * 0.01f;
        zone.gameObject.SetActive(true);

        // Executar atac amb delay
        //Invoke(nameof(ExecuteHeavyAttack), heavyAttackDelay);
        Invoke(nameof(EndEnemyAttack), heavyAttackDelay + 0.3f);
    }

        private void EndEnemyAttack()
    {
        isAttacking = false;
        agent.isStopped = false;
    }
    public void HeavyAttackZoneStay(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            fireZoneArea = true;
            Debug.Log("Player in heavy fire zone: " + fireZoneArea);
            if (timerFireStay < 0)
            {
                timerFireStay = 0.5f;
                other.GetComponent<VikingController>().HealthTaken(gameManager.DamageCalulator(activeElement, heavyAttackBasicDamage, heavyAttackElementalDamage, other.GetComponent<VikingController>().activeElement));
            }
            else
            {
                timerFireStay -= Time.deltaTime;
            }
        }
    }

    public void AttackEnter(Collider other)
    {
        if (other.CompareTag(Constants.player) && !playerHitted)
        {
            playerHitted = true;
            other.GetComponent<VikingController>().HealthTaken(gameManager.DamageCalulator(activeElement, basicAttackBasicDamage, basicAttackElementalDamage, other.GetComponent<VikingController>().activeElement));
        }
    }
}
