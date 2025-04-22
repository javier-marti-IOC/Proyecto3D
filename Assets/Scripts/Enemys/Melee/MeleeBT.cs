using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeBT : Enemy
{
    private bool playerHitted;
    public int earthBasicAttackBasicDamage;
    public int earthBasicAttackElementalDamage;
    public int earthHeavyAttackBasicDamage;
    public int earthHeavyAttackElementalDamage;
    public Collider earthBasicAttackCollider;
    public Collider earthHeavyAttackCollider;

    // Start is called before the first frame update
    void Start()
    {
        healthPoints = 200;
        activeElement = Element.Earth;
    }

    //Update is called once per frame
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
                            else if (activeElement == Element.Fire)
                            {

                            }
                        }
                        else
                        {
                            animator.SetInteger("Anim",0);
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

    public void basicAttackEnter(Collider other)
    {
        if (other.tag.Equals(Constants.player) && !playerHitted )
        {
            playerHitted = true;
            Debug.Log("Player hitted with earth basic attack");
            //player.GetComponent<tempPlayer>().healthPoints -= gameManager.DamageCalulator(activeElement,earthBasicAttackBasicDamage,earthBasicAttackElementalDamage,player.GetComponent<tempPlayer>().activeElement);
            Debug.Log("Player health: " + player.GetComponent<tempPlayer>().healthPoints);
        }
    }

    public void heavyAttackEnter(Collider other)
    {
        if (other.tag.Equals(Constants.player) && !playerHitted )
        {
            playerHitted = true;
            Debug.Log("Player hitted with earth basic attack");
            //player.GetComponent<tempPlayer>().healthPoints -= gameManager.DamageCalulator(activeElement,earthHeavyAttackBasicDamage,earthHeavyAttackElementalDamage,player.GetComponent<tempPlayer>().activeElement);
            Debug.Log("Player health: " + player.GetComponent<tempPlayer>().healthPoints);
        }
    }

    private void earthBasicAttackActivated()
    {
        playerHitted = false;
        earthBasicAttackCollider.enabled = true;
    }

    private void earthBasicAttackDisactivated()
    {
        playerHitted = false;
        earthBasicAttackCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(Constants.player))
        {
            playerInAttackRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals(Constants.player))
        {
            playerInAttackRange = false;
        }
    }

    private void earthHeavyAttackActivated()
    {
        playerHitted = false;
        earthHeavyAttackCollider.enabled = true;
    }

    private void earthHeavyAttackDisactivated()
    {
        playerHitted = false;
        earthHeavyAttackCollider.enabled = false;
        cooldownHeavyAttack = Random.Range(minCooldownTimeInclusive,maxCooldownTimeExclusive);
    }
}
