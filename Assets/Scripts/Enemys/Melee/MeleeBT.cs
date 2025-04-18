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
                            if (activeElement == Element.Earth)
                            {
                                
                            } 
                            else if (activeElement == Element.Fire)
                            {

                            }
                        }
                        else
                        {
                            //Patrol();
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
                        //Patrol();
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
            player.GetComponent<tempPlayer>().healthPoints -= gameManager.DamageCalulator(activeElement,earthBasicAttackBasicDamage,earthBasicAttackElementalDamage,player.GetComponent<tempPlayer>().activeElement);
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
}
