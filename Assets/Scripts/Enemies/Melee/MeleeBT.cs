using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeBT : Enemy
{
    private bool playerInSecurityDistance;
    [Header("Collider")]
    [SerializeField] protected Collider basicAttackCollider;
    [SerializeField] protected Collider heavyAttackCollider;
    
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
                    if (!player.GetComponent<VikingController>().EnemyDetecion(this))
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

    public void AttackEnter(Collider other)
    {
        if (other.CompareTag(Constants.player) && !playerHitted)
        {
            playerHitted = true;
            other.GetComponent<VikingController>().HealthTaken(gameManager.DamageCalulator(activeElement,basicAttackBasicDamage,basicAttackElementalDamage,other.GetComponent<VikingController>().activeElement));
        }
    }
}
