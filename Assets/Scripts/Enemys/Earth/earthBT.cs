using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class earthBT : MonoBehaviour
{
    public Element activeElement;
    public int healthPoints;
    public GameObject player;
    public GameObject tower;

    //Boolans
    public bool towerCalling;
    private bool onAction;
    public bool onHealZone;
    private bool playerInRange;

    // Start is called before the first frame update
    void Start()
    {
        activeElement = Element.Earth;
        healthPoints = 100;
        player = GameObject.FindGameObjectWithTag("Player");
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
                if (playerInRange)
                {
                    
                }
                else
                {

                }
            }
        }
        else
        {
            Destroy(this);
        }
    }

    public void Heal()
    {

    }
}
