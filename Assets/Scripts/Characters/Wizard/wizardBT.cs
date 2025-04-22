using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class wizardBT : MonoBehaviour
{
    public GameManager gameManager;
    public tempPlayer player;
    [HideInInspector]
    public Element activeElement;
    //Mana
    private float grassMana;
    private float waterMana;
    private float fireMana;
    private float electricMana;

    //Booleans
    [HideInInspector]
    public bool playerInAttackRange;
    public bool playerOnWizardMeleeRange;
    public bool onAttackEdge;
    private bool onAction;

    //Timers
    public float switchElementTime;
    private float switchElementTimer;
    public float heavyAttackTime;
    private float heavyAttackTimer;

    //BasicAttack
    public GameObject basicSpell;
    public Transform spellExitPoint;

    private float t = 2; 

    // Start is called before the first frame update
    void Start()
    {
        //Inizialize Element
        activeElement = Element.None;

        //Inizialize Mana
        grassMana= 100f;
        waterMana= 100f;
        fireMana = 100f;
        electricMana = 100f;

        //Inizialize Booleans
        playerInAttackRange = false;
        playerOnWizardMeleeRange = false;
        onAttackEdge = false;

        //Inizialize Timers
        switchElementTimer = switchElementTime;
        heavyAttackTimer = heavyAttackTime;
    }

    // Update is called once per frame
    void Update()
    {
        heavyAttackTimer -= Time.deltaTime;
        if (!onAction)
            {
            //Is player on my action range?
            if(playerInAttackRange)
            {
                //Im in a positive element interaction with the player?
                if(gameManager.ElementInteraction(activeElement,player.activeElement) > 0)
                {
                    Debug.Log(player.slowed);
                    //Is player Slowed?
                    if(player.slowed)
                    {
                        Vector3 direction = transform.position - player.transform.position;
                        direction.Normalize(); // Normalize to get only the direction

                        // Calculate the new position in the opposite direction
                        Vector3 newPosition = transform.position + direction * 100;

                        // Set the new position as the destination
                        gameObject.GetComponent<NavMeshAgent>().SetDestination(newPosition);
                        
                    }
                    else
                    {
                        //Is player attacking Wizard?
                        if(player.onAttack && playerOnWizardMeleeRange)
                        {
                            //Can the wizard dodge?
                            if(Random.Range(1,500) == 250f)
                            {
                                //Dodge
                                onAction = true; 
                            }
                        }
                        else
                        {
                            if(heavyAttackTimer < 0 || Random.Range(1,300) == 150f)
                            {
                                //HeavyAttack
                                onAction = true; 
                                //Aixo anira per animació
                                HeavySpellCast();
                            }
                            else
                            {
                                //BasicAttack
                                onAction = true;
                                //Aixo anira per animació
                                basicSpellCast();
                            }
                        }
                    }
                }
                else
                {
                    if (switchElementTimer < 0 && CounterElementAvailable(player.activeElement))
                    {
                        activeElement = gameManager.getCounterElement(player.activeElement);
                    }
                    else
                    {
                        if(player.slowed && !onAttackEdge)
                        {
                            //Nav Mesh track point to Action Edge
                            Vector3 direction = transform.position - player.transform.position;
                            //Debug.Log("Player poition: " + player.transform.position + " Wizard position: " + transform.position + " Direction: " + direction );

                            // Calculate the new position in the opposite direction
                            Vector3 newPosition = transform.position + direction * 0.1f;

                            // Set the new position as the destination
                            gameObject.GetComponent<NavMeshAgent>().SetDestination(newPosition);
                        }
                        else
                        {
                            //Is player attacking Wizard?
                            if(player.onAttack && playerOnWizardMeleeRange)
                            {
                                //Can the wizard dodge?
                                if(Random.Range(1,500) == 250f)
                                {
                                    //Dodge
                                    onAction = true; 
                                }
                            }
                            else
                            {
                                if (activeElement != Element.None)
                                {                               
                                    if(heavyAttackTimer < 0 || Random.Range(1,300) == 150f)
                                    {
                                        //HeavyAttack
                                        onAction = true; 
                                        //Aixo anira per animació
                                        HeavySpellCast();
                                    }
                                    else
                                    {
                                        //BasicAttack
                                        onAction = true;
                                        //Aixo anira per animació
                                        basicSpellCast();
                                    }
                                }
                                else
                                {
                                    onAction = true;
                                    basicSpellCast();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //Nav Mesh track point to Action Edge
                gameObject.GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
            }
        }
        else
        {
            Vector3 direction = player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 4);
            if ((t -= Time.deltaTime) <= 0) 
            { 
                onAction =false;
                t = 2; 
            }    
        }
        
    }


    void basicSpellCast()
    {  
        // Smooth look at the player

        //transform.LookAt(player.transform);
        Instantiate(basicSpell, spellExitPoint.position, Quaternion.identity);
    }

    void HeavySpellCast()
    {  
        heavyAttackTimer = heavyAttackTime;
    }

    private bool CounterElementAvailable(Element element)
    {
        if (element == Element.None)
        {
            int num = Random.Range(1,5);
            if (num == 1 && fireMana == 100) return true;
            else if (num == 2 && electricMana == 100) return true;
            else if (num == 3 && waterMana == 100) return true;
            else if (num == 4 && grassMana == 100) return true;
            return false;
        } 
        else if (element == Element.Earth && fireMana == 100) return true;
        else if (element == Element.Water && electricMana == 100) return true;
        else if (element == Element.Fire && waterMana == 100) return true;
        else if (element == Element.Electric && grassMana == 100) return true;
        return false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("ENTER");
            playerInAttackRange = true;
            onAttackEdge = true;
            gameObject.GetComponent<NavMeshAgent>().SetDestination(transform.position);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("EXIT");
            playerInAttackRange = false;
            onAttackEdge = false;
        }
    }

    public void OnInerTriggerEnter()
    {
        onAttackEdge = false;
    }
    public void OnInerTriggerExit()
    {
        onAttackEdge = true;
    }
}
