using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class VikingController : MonoBehaviour
{

    //DPAD
    [SerializeField] private InputActionAsset inputActions;
    private InputAction dpadAction;
    private InputAction basicAttackAction;
    private InputAction heavyAttackAction;
    private InputAction rollAction;
    private Vector2 dpadValue;
    

    //Mana Bars
    public int earthMana;
    public int fireMana;
    public int waterMana;
    public int electricMana;

    public ElementsHUD vikingElementsHUD;
    public HealthHUD vikingHealthHUD;
    public Element activeElement;
    public Animator animator;
    public int healthPoints;
    public GameObject swordCollider;
    private bool isBasicAttack;
    public int basicAttackBasicDamage;
    public int basicAttackMagicDamage;
    public int heavyAttackBasicDamage;
    public int heavyAttackMagicDamage;
    private GameManager gameManager;
    public ElementsHUD elementsHUD;

    public bool OnAction;
    public bool isRolling;
    public float rollCooldown;
    private float reduceMana = 1f;
    // Start is called before the first frame update
    void Start()
    {
        earthMana = 0;
        fireMana = 0;
        waterMana = 0;
        electricMana = 0;
        healthPoints = 100;
        activeElement = Element.None;
        dpadAction =  inputActions.FindAction("DPAD");
        dpadAction.Enable();
        basicAttackAction = inputActions.FindAction("BasicAttack");
        basicAttackAction.Enable();
        heavyAttackAction = inputActions.FindAction("HeavyAttack");
        heavyAttackAction.Enable();
        rollAction = inputActions.FindAction("Roll");
        heavyAttackAction.Enable();
        OnAction = false;
        swordCollider.SetActive(false);
        isBasicAttack = true;
        vikingHealthHUD.SetHealth(healthPoints);
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        dpadValue = dpadAction.ReadValue<Vector2>();
        rollCooldown -= Time.deltaTime;
        if (!OnAction && healthPoints > 0)
        {
            if (dpadValue.x > 0.5f)
            {
                ChangeElement(Element.Earth);
            }
            if (dpadValue.x < -0.5f)
            {
                ChangeElement(Element.Electric);
            }
            if (dpadValue.y > 0.5f)
            {
                ChangeElement(Element.Fire);
            }
            if (dpadValue.y < -0.5f)
            {
                ChangeElement(Element.Water);
            }
            if (basicAttackAction.WasPerformedThisFrame() && !isRolling)
            {
                BasicAttack();
            }
            if (heavyAttackAction.ReadValue<float>() > 0.5f && !isRolling)
            {
                HeavyAttack();
            }
            if (rollAction.WasPerformedThisFrame() && rollCooldown < 0f)
            {
                rollCooldown = 1.2f;
                Roll();
            }
        }
        if (healthPoints <= 0)
        {
            Dying();
        }
        reduceMana -= Time.deltaTime;
        if (reduceMana < 0 )
        {
            reduceMana = 0.5f;
            if (activeElement == Element.Earth)
            {
                earthMana -= 1;
                elementsHUD.earthReduce(earthMana);
                if (earthMana  <= 0)
                {
                    elementsHUD.EarthStopBlink();
                    activeElement = Element.None;
                }
            }
            else if (activeElement == Element.Water)
            {
                waterMana -= 1;
                elementsHUD.waterReduce(waterMana);
                if (waterMana  <= 0)
                {
                    elementsHUD.WaterStopBlink();
                    activeElement = Element.None;
                }
            }   
            else if (activeElement == Element.Fire)
            {
                fireMana -= 1;
                //if (fireMana <= 0) StopElementBlink(activeElement);
                elementsHUD.fireReduce(fireMana);
                if (fireMana  <= 0)
                {
                    elementsHUD.FireStopBlink();
                    activeElement = Element.None;
                }
            }   
            else if (activeElement == Element.Electric)
            {
                electricMana -= 1;
                elementsHUD.lightningReduce(electricMana);
                if (electricMana  <= 0)
                {
                    elementsHUD.LightningStopBlink();
                    activeElement = Element.None;
                }
            }  
        }
    }

    private void ChangeElement(Element element)
    {
        bool changed = false;
        //ACtivar elemento nuevo
        if (element == Element.Earth && earthMana == 100 && activeElement != Element.Earth) 
        {
            vikingElementsHUD.earthReduce(earthMana);
            changed = true;
        }
        else if (element == Element.Water && waterMana == 100 && activeElement != Element.Water)
        {
            vikingElementsHUD.waterReduce(waterMana);
            changed = true;
        }
        else if (element == Element.Fire && fireMana == 100 && activeElement != Element.Fire)
        {
            vikingElementsHUD.fireReduce(fireMana);
            changed = true;
        }
        else if (element == Element.Electric && electricMana == 100 && activeElement != Element.Electric)
        {
            vikingElementsHUD.lightningReduce(electricMana);
            changed = true;
        }
        //Desactivar elemento antiguo
        if (changed)
        {
            if (activeElement == Element.Earth)
            {
                earthMana = 0;
                elementsHUD.earthReduce(earthMana);
                elementsHUD.EarthStopBlink();
            }
            else if (activeElement == Element.Water)
            {
                waterMana = 0;
                elementsHUD.waterReduce(waterMana);
                elementsHUD.WaterStopBlink();
            }
            else if (activeElement == Element.Fire)
            {
                fireMana = 0;
                elementsHUD.fireReduce(fireMana);
                elementsHUD.FireStopBlink();
            }
            else if (activeElement == Element.Electric)
            {
                electricMana = 0;
                elementsHUD.lightningReduce(electricMana);
                elementsHUD.LightningStopBlink();
            }
            activeElement = element;
        }
    }

    public void BasicAttack()
    {
        animator.SetTrigger("SoftAttack");
        OnAction = true;
        isBasicAttack = true;
    }
    public void Roll()
    {
        animator.SetTrigger("Roll");
    }
    public void HeavyAttack()
    {
        animator.SetTrigger("HardAttack");
        OnAction = true;
        isBasicAttack = false;
    }
    public void Dying()
    {
        animator.SetTrigger("Dying");
        OnAction = true;
        
    }

    public void AttackEnter(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            int damageDeal;
            if (isBasicAttack)
            {
                damageDeal = gameManager.DamageCalulator(activeElement,basicAttackBasicDamage,basicAttackMagicDamage,other.GetComponentInParent<Enemy>().activeElement);
                other.GetComponentInParent<Enemy>().HealthTaken(damageDeal);
                Debug.Log("Basic Attack Damage Deal: " + damageDeal);
            }
            else
            {
                damageDeal = gameManager.DamageCalulator(activeElement,heavyAttackBasicDamage,heavyAttackMagicDamage,other.GetComponentInParent<Enemy>().activeElement);
                other.GetComponentInParent<Enemy>().HealthTaken(damageDeal);
                Debug.Log("Heavy Attack Damage Deal: " + damageDeal);
            }
        }
        if (other.CompareTag(Constants.tower))
        {
            Debug.Log("TowerHit");
            other.GetComponent<Tower>().HealthTaken(5);
        }
    }
    public void EndAction()
    {
        OnAction = false;
    }

    public void ColliderAttackEnable()
    {
        swordCollider.SetActive(true);
    }

    public void ColliderAttackDisable()
    {
        swordCollider.SetActive(false);
    }

    public void InmunityEnable()
    {
        isRolling = true;
    }

    public void InmunityDisable()
    {
        isRolling = false;
    }
    public void HealthTaken(int healthTaken)
    {
        if (!isRolling)
        {
            healthPoints -= healthTaken;
            vikingHealthHUD.SetHealth(healthPoints);
        }
    }
    public void CollectMana(Element element)
    {
        int min = 30;
        int max = 45;
        if (element == Element.None)
        {
            if (activeElement != Element.Earth) 
            {
                earthMana = 100;
                elementsHUD.earthAdd(earthMana);
            }
            if (activeElement != Element.Water)
            {
                waterMana = 100;
                elementsHUD.waterAdd(waterMana);
            }
            if (activeElement != Element.Fire) 
            {
                fireMana = 100;
                elementsHUD.fireAdd(fireMana);
            }
            if (activeElement != Element.Electric) 
            {
                electricMana = 100;
                elementsHUD.lightningAdd(electricMana);
            }
        }
        else if (element == Element.Earth && activeElement != Element.Earth)
        {
            earthMana += Random.Range(min,max);
            if (earthMana > 100) earthMana = 100;
            elementsHUD.earthAdd(earthMana);

        }
        else if (element == Element.Water && activeElement != Element.Water)
        {
            waterMana += Random.Range(min,max);
            if (waterMana > 100) waterMana = 100;
            elementsHUD.waterAdd(waterMana);
        }   
        else if (element == Element.Fire && activeElement != Element.Fire)
        {
            fireMana += Random.Range(min,max);
            if (fireMana > 100) fireMana = 100;
            elementsHUD.fireAdd(fireMana);
        }   
        else if (element == Element.Electric && activeElement != Element.Electric)
        {
            electricMana += Random.Range(min,max);
            if (electricMana > 100) electricMana = 100;
            elementsHUD.lightningAdd(electricMana);
        }    
    }
}
