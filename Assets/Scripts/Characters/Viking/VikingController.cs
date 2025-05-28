using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class VikingController : MonoBehaviour
{

    public GameObject body;
    //DPAD
    [SerializeField] private InputActionAsset inputActions;
    private InputAction dpadAction;
    private InputAction basicAttackAction;
    private InputAction heavyAttackAction;
    private InputAction rollAction;
    private Vector2 dpadValue;

    [Header("Elements")]
    public int earthMana;
    public int fireMana;
    public int waterMana;
    public int electricMana;
    public GameObject earthEffect;
    public GameObject waterEffect;
    public GameObject fireEffect;
    public GameObject electricEffect;

    [Header("HUD")]
    public HealthHUD vikingHealthHUD;
    public ElementsHUD elementsHUD;
    public PauseMenu pauseMenu;

    [Header("Combat")]
    public Animator animator;
    public int healthPoints;
    public Collider swordCollider;
    public GameObject slash;
    public Transform slashPosition;
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

    [Header("Heal")]
    public GameObject healParticles;
    private float healParticlesTimer;
    public GameObject deathParticle;
    public Transform deathParticlePosition;

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
        swordCollider.enabled = false;
        isBasicAttack = true;
        earthEffect.SetActive(false);
        waterEffect.SetActive(false);
        fireEffect.SetActive(false);
        electricEffect.SetActive(false);

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
        if (healParticlesTimer < 0)
        {
            healParticles.SetActive(false);
        }
        else
        {
            healParticlesTimer -= Time.deltaTime;
        }
        if (healthPoints <= 0)
        {
            Dying();
        }
        else
        {
            //Timers
            rollCooldown -= Time.deltaTime;
            reduceMana -= Time.deltaTime;

            if (!OnAction && GetComponent<ThirdPersonController>().Grounded)
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
                        earthMana = 0;
                        earthEffect.SetActive(false);
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
                        waterMana = 0;
                        waterEffect.SetActive(false);
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
                        fireMana = 0;
                        fireEffect.SetActive(false);
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
                        electricMana = 0;
                        electricEffect.SetActive(false);
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
            earthEffect.SetActive(true);
            changed = true;
        }
        else if (element == Element.Water && waterMana == 100 && activeElement != Element.Water)
        {
            waterEffect.SetActive(true);
            changed = true;
        }
        else if (element == Element.Fire && fireMana == 100 && activeElement != Element.Fire)
        {
            fireEffect.SetActive(true);
            changed = true;
        }
        else if (element == Element.Electric && electricMana == 100 && activeElement != Element.Electric)
        {
            electricEffect.SetActive(true);
            changed = true;
        }
        //Desactivar elemento antiguo
        if (changed)
        {
            AudioManager.Instance?.Play("ActivateElement");
            if (activeElement == Element.Earth)
            {
                earthMana = 0;
                elementsHUD.earthReduce(earthMana);
                elementsHUD.EarthStopBlink();
                earthEffect.SetActive(false);
            }
            else if (activeElement == Element.Water)
            {
                waterMana = 0;
                elementsHUD.waterReduce(waterMana);
                elementsHUD.WaterStopBlink();
                waterEffect.SetActive(false);
            }
            else if (activeElement == Element.Fire)
            {
                fireMana = 0;
                elementsHUD.fireReduce(fireMana);
                elementsHUD.FireStopBlink();
                fireEffect.SetActive(false);
            }
            else if (activeElement == Element.Electric)
            {
                electricMana = 0;
                elementsHUD.lightningReduce(electricMana);
                elementsHUD.LightningStopBlink();
                electricEffect.SetActive(false);
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
        swordCollider.enabled = true;
        if (activeElement != Element.None)
        {
            Instantiate(slash,swordCollider.transform.position,Quaternion.identity,null);
        }
    }

    public void ColliderAttackDisable()
    {
        swordCollider.enabled = false;
    }
    public void AttackEnter(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            //ResetEnemyDetection(other.GetComponent<Enemy>());
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
        else if (other.CompareTag(Constants.tower))
        {
            if (gameManager.ElementInteraction(other.GetComponent<Tower>().activeElement, activeElement) < 0 && !isBasicAttack)
            {
                if (activeElement == Element.Earth) earthMana = 0;
                else if (activeElement == Element.Water) waterMana = 0;
                else if (activeElement == Element.Fire) fireMana = 0;
                else if (activeElement == Element.Electric) electricMana = 0;
                Debug.Log("TowerHit");
                other.GetComponent<Tower>().HealthTaken(34);
            }
            else if (other.GetComponent<Tower>().activeElement == Element.None && activeElement != Element.None && !isBasicAttack)
            {
                other.GetComponent<Tower>().HealthTaken(50);
                earthMana = 0;
                elementsHUD.earthReduce(earthMana);
                elementsHUD.EarthStopBlink();
                waterMana = 0;
                elementsHUD.waterReduce(waterMana);
                elementsHUD.WaterStopBlink();
                fireMana = 0;
                elementsHUD.fireReduce(fireMana);
                elementsHUD.FireStopBlink();
                electricMana = 0;
                elementsHUD.lightningReduce(electricMana);
                elementsHUD.LightningStopBlink();
            }
        }
        //else if (other.CompareTag(Constants.seta))
        {
            
        }
    }
    public void SlashAttackEnter(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            int damageDeal;
            damageDeal = gameManager.DamageCalulator(activeElement, 0, heavyAttackMagicDamage, other.GetComponent<Enemy>().activeElement);
            other.GetComponent<Enemy>().HealthTaken(damageDeal);
            Debug.Log("Slash Attack Damage Deal: " + damageDeal);
        }
    }
    public void HealthTaken(int healthTaken)
    {
        if (!isRolling)
        {
            AudioManager.Instance?.Play("HitMarker");
            healthPoints -= healthTaken;
            vikingHealthHUD.SetHealth(healthPoints);
        }
    }
    public void Dying()
    {
        //animator.SetTrigger("Dying");
        OnAction = true;
        GetComponent<ThirdPersonController>().enabled = false;
        body.SetActive(false);
        pauseMenu.ToggleDeath();
        if (deathParticle != null && deathParticlePosition != null)
        {
            Instantiate(deathParticle,slashPosition.position,Quaternion.identity,null);
        }
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
            return true;
        }
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
            AudioManager.Instance.Play("PickUpGoldOrbe");
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
        else
        {
            AudioManager.Instance.Play("PickUpOrbe");
            if (element == Element.Earth && activeElement != Element.Earth)
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
    }
    public void CollectLife()
    {
        healParticles.SetActive(false);
        healParticles.SetActive(true);
        healParticlesTimer = 2f;
        AudioManager.Instance.Play("PickUpOrbe");
        healthPoints += 30;
        if (healthPoints > 100) healthPoints = 100;
        vikingHealthHUD.SetHealth(healthPoints);
    }

    // Funciones para reproducir sonidos de ataque, se llaman en la animación
    public void PlayBasicAttackSound()
    {
        AudioManager.Instance?.Play("PlayerBasicAttack");
    }

    public void PlayHeavyAttackSound()
    {
        AudioManager.Instance?.Play("PlayerHeavyAttack");
    }
    public void PlayDashSound()
    {
        AudioManager.Instance?.Play("PlayerDash");
    }
}
