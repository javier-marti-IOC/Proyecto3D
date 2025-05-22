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
    private bool isAttacking;
    private Vector3 pendingHeavyAttackPosition;

    [Header("FireEnemy properties")]
    public GameObject fireZonePrefab;
    public float heavyAttackDelay = 2f;
    public float fireZoneDuration;

        [Header("EarthEnemy AudioSources")]
    public AudioSource audioEarthDeath;
    public AudioSource audioEarthBasicAttack;
    public AudioSource audioEarthHeavyAttack;
    public AudioSource audioEarthHit;

        [Header("FireEnemy AudioSources")]
    public AudioSource audioFireDeath;
    public AudioSource audioFireBasicAttack;
    public AudioSource audioFireHit;

    void Update()
    {
        if (!isBTEnabled) return;
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
                if (playerDetected)
                {
                    if (!player.GetComponent<VikingController>().EnemyDetecion(this))
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), 1 * Time.deltaTime);
                        animator.SetInteger(Constants.state, 0);
                    }
                    else
                    {
                        if (playerInAttackRange)
                        {
                            if (activeElement == Element.Earth)
                            {
                                agent.SetDestination(transform.position);
                                if (cooldownHeavyAttack < 0)
                                {
                                    Debug.Log("HEAVY ATTACK");
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
                            animator.SetInteger(Constants.state, 0);
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
            Dying(true);
        }
    }
    public void PlayerSecurityMinDistanceColliderEnter(Collider other)
    {
        if (other.tag.Equals(Constants.player))
        {
            playerInSecurityDistance = false;
        }
    }
    public void PlayerSecurityMinDistanceColliderExit(Collider other)
    {
        if (other.tag.Equals(Constants.player))
        {
            playerInSecurityDistance = true;
        }
    }
    public void PlayerSecurityMaxDistanceColliderEnter(Collider other)
    {
        if (other.tag.Equals(Constants.player))
        {
            playerInSecurityDistance = true;
        }
    }
    public void PlayerSecurityMaxDistanceColliderExit(Collider other)
    {
        if (other.tag.Equals(Constants.player))
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
        private void EndEnemyAttack()
    {
        isAttacking = false;
        agent.isStopped = false;
    }
    
    public void StartHeavyAttack()
    {
        if (player == null) return;

        isAttacking = true;
        agent.isStopped = true;

        pendingHeavyAttackPosition = player.transform.position;

        AudioManager.Instance.Play("FireInvoke");
        GameObject zone = Instantiate(fireZonePrefab, pendingHeavyAttackPosition + Vector3.up * 0.01f, Quaternion.identity);
        
        // Pasar referencia del enemigo a la zona
        zone.GetComponent<FireZone>().Initialize(this);
        
        Destroy(zone, fireZoneDuration);

        Invoke(nameof(EndEnemyAttack), heavyAttackDelay + 0.3f);
    }

    public void DealFireZoneDamage(Collider playerCollider)
    {
        if (playerCollider.TryGetComponent(out VikingController vc))
        {
            int dmg = gameManager.DamageCalulator(activeElement, heavyAttackBasicDamage, heavyAttackElementalDamage, vc.activeElement);
            vc.HealthTaken(dmg);
        }
    }

    public void EarthBasicAttackEnter(Collider other)
    {
        if (other.CompareTag(Constants.player) && !playerHitted)
        {
            playerHitted = true;
            other.GetComponent<VikingController>().HealthTaken(gameManager.DamageCalulator(activeElement, basicAttackBasicDamage, basicAttackElementalDamage, other.GetComponent<VikingController>().activeElement));
        }
    }
    public void EarthHeavyAttackEnter(Collider other)
    {
        if (other.CompareTag(Constants.player) && !playerHitted)
        {
            playerHitted = true;
            other.GetComponent<VikingController>().HealthTaken(gameManager.DamageCalulator(activeElement,heavyAttackBasicDamage,heavyAttackElementalDamage,other.GetComponent<VikingController>().activeElement));
        }
    }
}
