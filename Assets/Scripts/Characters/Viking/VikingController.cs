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

    [Header("Mana Bars")]
    public int earthMana;
    public int fireMana;
    public int waterMana;
    public int electricMana;

    [Header("HUD")]
    public HealthHUD vikingHealthHUD;
    public ElementsHUD elementsHUD;
    public PauseMenu pauseMenu;

    [Header("Combat")]
    public Animator animator;
    public int healthPoints;
    public GameObject swordCollider;
    public Element activeElement;
    private bool isBasicAttack;
    public int basicAttackBasicDamage;
    public int basicAttackMagicDamage;
    public int heavyAttackBasicDamage;
    public int heavyAttackMagicDamage;
    private GameManager gameManager;

    [Header("Booleans")]
    public bool OnAction;
    public bool isRolling;

    [Header("Cooldowns")]
    public float rollCooldown;
    private float reduceMana = 1f;

    [Header("Enemies")]
    public List<Enemy> enemiesInCombat;
    public int maxEnemies;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        //Valors Inicials
        earthMana = 0;
        fireMana = 0;
        waterMana = 0;
        electricMana = 0;
        healthPoints = 100;
        activeElement = Element.None;
        OnAction = false;
        swordCollider.SetActive(false);
        isBasicAttack = true;

        //DPAD
        dpadAction = inputActions.FindAction("DPAD");
        dpadAction.Enable();
        basicAttackAction = inputActions.FindAction("BasicAttack");
        basicAttackAction.Enable();
        heavyAttackAction = inputActions.FindAction("HeavyAttack");
        heavyAttackAction.Enable();
        rollAction = inputActions.FindAction("Roll");
        heavyAttackAction.Enable();

        //HUD
        vikingHealthHUD.SetHealth(healthPoints);
    }

    // Update is called once per frame
    void Update()
    {
        if (healthPoints <= 0)
        {
            Dying();
        }
        else
        {
            //Timers
            rollCooldown -= Time.deltaTime;
            reduceMana -= Time.deltaTime;

            if (!OnAction)
            {
                //DPAD
                dpadValue = dpadAction.ReadValue<Vector2>();
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

            if (reduceMana < 0)
            {
                reduceMana = 0.5f;
                if (activeElement == Element.Earth)
                {
                    earthMana -= 1;
                    elementsHUD.earthReduce(earthMana);
                    if (earthMana <= 0)
                    {
                        elementsHUD.EarthStopBlink();
                        activeElement = Element.None;
                    }
                }
                else if (activeElement == Element.Water)
                {
                    waterMana -= 1;
                    elementsHUD.waterReduce(waterMana);
                    if (waterMana <= 0)
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
                    if (fireMana <= 0)
                    {
                        elementsHUD.FireStopBlink();
                        activeElement = Element.None;
                    }
                }
                else if (activeElement == Element.Electric)
                {
                    electricMana -= 1;
                    elementsHUD.lightningReduce(electricMana);
                    if (electricMana <= 0)
                    {
                        elementsHUD.LightningStopBlink();
                        activeElement = Element.None;
                    }
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
            elementsHUD.earthReduce(earthMana);
            changed = true;
        }
        else if (element == Element.Water && waterMana == 100 && activeElement != Element.Water)
        {
            elementsHUD.waterReduce(waterMana);
            changed = true;
        }
        else if (element == Element.Fire && fireMana == 100 && activeElement != Element.Fire)
        {
            elementsHUD.fireReduce(fireMana);
            changed = true;
        }
        else if (element == Element.Electric && electricMana == 100 && activeElement != Element.Electric)
        {
            elementsHUD.lightningReduce(electricMana);
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

    //Roll
    public void Roll()
    {
        animator.SetTrigger("Roll");
    }
    public void InmunityEnable()
    {
        isRolling = true;
    }

    public void InmunityDisable()
    {
        isRolling = false;
    }


    //Combat
    public void BasicAttack()
    {
        animator.SetTrigger("SoftAttack");
        OnAction = true;
        isBasicAttack = true;
    }
    public void HeavyAttack()
    {
        animator.SetTrigger("HardAttack");
        OnAction = true;
        isBasicAttack = false;
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
    public void AttackEnter(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            ResetEnemyDetection(other.GetComponent<Enemy>());
            int damageDeal;
            if (isBasicAttack)
            {
                damageDeal = gameManager.DamageCalulator(activeElement, basicAttackBasicDamage, basicAttackMagicDamage, other.GetComponent<Enemy>().activeElement);
                other.GetComponent<Enemy>().HealthTaken(damageDeal);
                Debug.Log("Basic Attack Damage Deal: " + damageDeal);
            }
            else
            {
                damageDeal = gameManager.DamageCalulator(activeElement, heavyAttackBasicDamage, heavyAttackMagicDamage, other.GetComponent<Enemy>().activeElement);
                other.GetComponent<Enemy>().HealthTaken(damageDeal);
                Debug.Log("Heavy Attack Damage Deal: " + damageDeal);
            }
        }
        if (other.CompareTag(Constants.tower))
        {
            Debug.Log("TowerHit");
            other.GetComponent<Tower>().HealthTaken(5);
        }
    }
    public void HealthTaken(int healthTaken)
    {
        if (!isRolling)
        {
            healthPoints -= healthTaken;
            vikingHealthHUD.SetHealth(healthPoints);
        }
    }
    public void Dying()
    {
        Debug.Log("DEADGE");
        //animator.SetTrigger("Dying");
        OnAction = true;
        pauseMenu.ToggleDeath();
        healthPoints = 100;
    }

    //Control de maxim enemics en combat
    public bool EnemyDetecion(Enemy enemy)
    {
        if (enemiesInCombat.Contains(enemy))
        {
            return true;
        }
        else if (enemiesInCombat.Count < maxEnemies)
        {
            enemiesInCombat.Add(enemy);
            Debug.Log("Added");
            return true;
        }
        Debug.Log("Exceed");
        return false;
    }

    public void RemoveEnemyDetection(Enemy enemy)
    {
        enemiesInCombat.Remove(enemy);
    }

    public void ResetEnemyDetection(Enemy enemy)
    {
        for (int i = enemiesInCombat.Count - 1; i >= 0; i--)
        {
            RemoveEnemyDetection(enemiesInCombat[i]);
        }
        enemiesInCombat.Add(enemy);
    }

    //Recollir Drops
    public void CollectMana(Element element)
    {
        int mana = 25;
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
            earthMana += mana;
            if (earthMana > 100) earthMana = 100;
            elementsHUD.earthAdd(earthMana);

        }
        else if (element == Element.Water && activeElement != Element.Water)
        {
            waterMana += mana;
            if (waterMana > 100) waterMana = 100;
            elementsHUD.waterAdd(waterMana);
        }
        else if (element == Element.Fire && activeElement != Element.Fire)
        {
            fireMana += mana;
            if (fireMana > 100) fireMana = 100;
            elementsHUD.fireAdd(fireMana);
        }
        else if (element == Element.Electric && activeElement != Element.Electric)
        {
            electricMana += mana;
            if (electricMana > 100) electricMana = 100;
            elementsHUD.lightningAdd(electricMana);
        }
    }
    public void CollectLife()
    {
        healthPoints += 10;
        if (healthPoints > 100) healthPoints = 100;
        vikingHealthHUD.SetHealth(healthPoints);
    }
    
    //Reiniciar intent
    public void Continue()
    {
        gameManager.ResetEnemies();
        gameObject.GetComponent<ThirdPersonController>().enabled = false;
        transform.position = new Vector3(96, 0, 21);
        gameObject.GetComponent<ThirdPersonController>().enabled = true;
        Start();
    }
}
