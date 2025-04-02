using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wizardBT : MonoBehaviour
{
    public tempPlayer player;
    [HideInInspector]
    public tempGameManager.Element activeElement;

    //Mana
    private float grassMana;
    private float waterMana;
    private float fireMana;
    private float electricMana;

    //Booleans
    [HideInInspector]
    public bool playerInAttackRange;
    public bool playerOnWizardMeleeRange;
    private bool onAttackEdge;
    private bool onAction;

    //Timers
    public float switchElementTime;
    private float switchElementTimer;
    public float heavyAttackTime;
    private float heavyAttackTimer;

    //BasicAttack
    public GameObject basicSpell;
    public Transform spellExitPoint;

    // Start is called before the first frame update
    void Start()
    {
        //Inizialize Element
        activeElement = tempGameManager.Element.None;

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
        if (!onAction)
            {
            //Is player on my action range?
            if(playerInAttackRange)
            {
                //Im in a positive element interaction with the player?
                if(tempGameManager.PositiveElementInteraction(activeElement,player.activeElement))
                {
                    //Is player Slowed?
                    if(player.slowed)
                    {
                        //Nav Mesh track point to Action Edge
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
                        activeElement = tempGameManager.getCounterElement(player.activeElement);
                    }
                    else
                    {
                        if(player.slowed)
                        {
                            //Nav Mesh track point to Action Edge
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
                                if (activeElement != tempGameManager.Element.None)
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
            }
        }
    }


    void basicSpellCast()
    {  
        
        Instantiate(basicSpell, spellExitPoint.position, Quaternion.identity);
    }

    void HeavySpellCast()
    {  
        
    }

    private bool CounterElementAvailable(tempGameManager.Element element)
    {
        if (element == tempGameManager.Element.None)
        {
            int num = Random.Range(1,5);
            if (num == 1 && fireMana == 100) return true;
            else if (num == 2 && electricMana == 100) return true;
            else if (num == 3 && waterMana == 100) return true;
            else if (num == 4 && grassMana == 100) return true;
            return false;
        } 
        else if (element == tempGameManager.Element.Grass && fireMana == 100) return true;
        else if (element == tempGameManager.Element.Water && electricMana == 100) return true;
        else if (element == tempGameManager.Element.Fire && waterMana == 100) return true;
        else if (element == tempGameManager.Element.Electric && grassMana == 100) return true;
        return false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            playerInAttackRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            playerInAttackRange = false;
        }
    }
}
