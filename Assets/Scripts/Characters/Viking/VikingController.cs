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
    public GameObject slashEarth;
    public GameObject slashWater;
    public GameObject slashFire;
    public GameObject slashElectric;
    public Transform slashPosition;
    public Element activeElement;
    private bool isBasicAttack;
    public int basicAttackBasicDamage;
    public int basicAttackMagicDamage;
    public int heavyAttackBasicDamage;
    public int heavyAttackMagicDamage;
    private GameManager gameManager;
    public GameObject hitParticles;
    public GameObject lowHealthEffect;

    [Header("Booleans")]
    public bool OnAction;
    public bool isRolling;
    private bool isSpendingMana;
    private bool ChangeElementErrorPlayed;
    //private bool isShining;

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

    [Header("Shine")]
    public Renderer characterRenderer;
    public float shineDuration;
    private int shineElement;
    private bool isShiningUp = false;
    private float shineTimer = 0;
    public Material baseMaterial;
    public Material noneMaterial;
    public Material earthMaterial;
    public Material waterMaterial;
    public Material fireMaterial;
    public Material electricMaterial;
    public Material healMaterial;

    [Header("Horns")]
    public Renderer rightHorn;
    public Renderer leftHorn;
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
        hitParticles.SetActive(false);
        lowHealthEffect.SetActive(false);

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
        //CollectMana(Element.None);
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
        if (healthPoints <= 20)
        {
            lowHealthEffect.SetActive(true);
        }
        else
        {
            lowHealthEffect.SetActive(false);
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
                    if (activeElement != Element.None) isSpendingMana = true;
                }
                if (heavyAttackAction.ReadValue<float>() > 0.5f && !isRolling)
                {
                    HeavyAttack();
                    if (activeElement != Element.None) isSpendingMana = true;
                }
                if (rollAction.WasPerformedThisFrame() && rollCooldown < 0f)
                {
                    rollCooldown = 1.2f;
                    Roll();
                }
            }

            if (reduceMana < 0 && isSpendingMana)
            {
                reduceMana = 0.5f;
                if (activeElement == Element.Earth)
                {
                    earthMana -= 1;
                    earthEffect.SetActive(true);
                    elementsHUD.earthReduce(earthMana);
                    if (earthMana <= 0)
                    {
                        elementsHUD.EarthStopBlink();
                        activeElement = Element.None;
                        earthMana = 0;
                        earthEffect.SetActive(false);
                        isSpendingMana = false;
                        rightHorn.material = noneMaterial;
                        leftHorn.material = noneMaterial;
                    }
                }
                else if (activeElement == Element.Water)
                {
                    waterMana -= 1;
                    waterEffect.SetActive(true);
                    elementsHUD.waterReduce(waterMana);
                    if (waterMana <= 0)
                    {
                        elementsHUD.WaterStopBlink();
                        activeElement = Element.None;
                        waterMana = 0;
                        waterEffect.SetActive(false);
                        isSpendingMana = false;
                        rightHorn.material = noneMaterial;
                        leftHorn.material = noneMaterial;
                    }
                }
                else if (activeElement == Element.Fire)
                {
                    fireMana -= 1;
                    fireEffect.SetActive(true);
                    elementsHUD.fireReduce(fireMana);
                    if (fireMana <= 0)
                    {
                        elementsHUD.FireStopBlink();
                        activeElement = Element.None;
                        fireMana = 0;
                        fireEffect.SetActive(false);
                        isSpendingMana = false;
                        rightHorn.material = noneMaterial;
                        leftHorn.material = noneMaterial;
                    }
                }
                else if (activeElement == Element.Electric)
                {
                    electricMana -= 1;
                    electricEffect.SetActive(true);
                    elementsHUD.lightningReduce(electricMana);
                    if (electricMana <= 0)
                    {
                        elementsHUD.LightningStopBlink();
                        activeElement = Element.None;
                        electricMana = 0;
                        electricEffect.SetActive(false);
                        isSpendingMana = false;
                        rightHorn.material = noneMaterial;
                        leftHorn.material = noneMaterial;
                    }
                }
            }
        }
        if (isShiningUp)
        {
            shineTimer += Time.deltaTime;
            float t = shineTimer / shineDuration;
            if (shineElement == 0)
            {
                characterRenderer.material = noneMaterial;
            }
            else if (shineElement == 1)
            {
                characterRenderer.material = earthMaterial;
            }
            else if (shineElement == 2)
            {
                characterRenderer.material = waterMaterial;
            }
            else if (shineElement == 3)
            {
                characterRenderer.material = fireMaterial;
            }
            else if (shineElement == 4)
            {
                characterRenderer.material = electricMaterial;
            }
            else if (shineElement == 5)
            {
                characterRenderer.material = healMaterial;
            }
            if (t >= 1)
            {
                isShiningUp = false;
                characterRenderer.material = baseMaterial;
                shineTimer = 0;
            }
        }
    }

    private void ChangeElement(Element element)
    {
        if (element == activeElement)
        {
            ChangeElementErrorPlayed = true;
            Invoke(nameof(ErrorPlayed), 1.5f);
        }
        bool changed = false;
        //ACtivar elemento nuevo
        if (element == Element.Earth && earthMana == 100 && activeElement != Element.Earth)
        {
            elementsHUD.EarthStartBlink();
            rightHorn.material = earthMaterial;
            leftHorn.material = earthMaterial;
            changed = true;
        }
        else if (element == Element.Water && waterMana == 100 && activeElement != Element.Water)
        {
            elementsHUD.WaterStartBlink();
            rightHorn.material = waterMaterial;
            leftHorn.material = waterMaterial;
            changed = true;
        }
        else if (element == Element.Fire && fireMana == 100 && activeElement != Element.Fire)
        {
            elementsHUD.FireStartBlink();
            rightHorn.material = fireMaterial;
            leftHorn.material = fireMaterial;
            changed = true;
        }
        else if (element == Element.Electric && electricMana == 100 && activeElement != Element.Electric)
        {
            elementsHUD.LightningStartBlink();
            rightHorn.material = electricMaterial;
            leftHorn.material = electricMaterial;

            changed = true;
        }
        //Desactivar elemento antiguo
        if (changed)
        {
            ChangeElementErrorPlayed = true;
            Invoke(nameof(ErrorPlayed), 1.5f);
            gameManager.ExitDPADHelp();
            gameManager.EnterSlashAttackHelp();
            AudioManager.Instance?.Play("ActivateElement");
            if (activeElement == Element.Earth)
            {
                if (isSpendingMana)
                {
                    earthMana = 0;
                    elementsHUD.earthReduce(earthMana);
                    earthEffect.SetActive(false);
                    elementsHUD.EarthStopBlink();
                }
                else
                {
                    elementsHUD.EarthStopBlinkPreSelected();
                }
            }
            else if (activeElement == Element.Water)
            {
                if (isSpendingMana)
                {
                    waterMana = 0;
                    elementsHUD.waterReduce(waterMana);
                    waterEffect.SetActive(false);
                    elementsHUD.WaterStopBlink();
                }
                else
                {
                    elementsHUD.WaterStopBlinkPreSelected();
                }
            }
            else if (activeElement == Element.Fire)
            {
                if (isSpendingMana)
                {
                    fireMana = 0;
                    elementsHUD.fireReduce(fireMana);
                    fireEffect.SetActive(false);
                    elementsHUD.FireStopBlink();
                }
                else
                {
                    elementsHUD.FireStopBlinkPreSelected();
                }
            }
            else if (activeElement == Element.Electric)
            {
                if (isSpendingMana)
                {
                    electricMana = 0;
                    elementsHUD.lightningReduce(electricMana);
                    electricEffect.SetActive(false);
                    elementsHUD.LightningStopBlink();
                }
                else
                {
                    elementsHUD.LightningStopBlinkPreSelected();
                }
            }
            activeElement = element;
            isSpendingMana = false;
        }
        else if (!ChangeElementErrorPlayed)
        {
            AudioManager.Instance?.Play("ChangeElementError");
            ChangeElementErrorPlayed = true;
            Invoke(nameof(ErrorPlayed), 1.5f);
        }
    }

    private void ErrorPlayed()
    {
        ChangeElementErrorPlayed = false;
    }

    //Roll
    public void Roll()
    {
        gameManager.ExitRollHelp();
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
    }
    public void StartSlashAttack()
    {

        if (activeElement == Element.Earth)
        {
            Instantiate(slashEarth, swordCollider.transform.position, Quaternion.identity, null);
            earthMana -= 35;
            elementsHUD.earthReduce(earthMana);
        }
        else if (activeElement == Element.Water)
        {
            Instantiate(slashWater, swordCollider.transform.position, Quaternion.identity, null);
            waterMana -= 35;
            elementsHUD.waterReduce(waterMana);
        }
        else if (activeElement == Element.Fire)
        {
            Instantiate(slashFire, swordCollider.transform.position, Quaternion.identity, null);
            fireMana -= 35;
            elementsHUD.fireReduce(fireMana);
        }
        else if (activeElement == Element.Electric)
        {
            Instantiate(slashElectric, swordCollider.transform.position, Quaternion.identity, null);
            electricMana -= 35;
            elementsHUD.lightningReduce(electricMana);
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
            int[] damageDeal;
            if (isBasicAttack)
            {
                if (activeElement == Element.None)
                {
                    damageDeal = gameManager.DamageCalulator(activeElement, basicAttackBasicDamage, 0, other.GetComponent<Enemy>().activeElement);
                }
                else
                {
                    damageDeal = gameManager.DamageCalulator(activeElement, basicAttackBasicDamage, basicAttackMagicDamage, other.GetComponent<Enemy>().activeElement);
                }
                other.GetComponent<Enemy>().HealthTaken(damageDeal, activeElement);
                Debug.Log("Basic Attack Damage Deal: " + damageDeal);
            }
            else
            {
                damageDeal = gameManager.DamageCalulator(activeElement, heavyAttackBasicDamage, 0, other.GetComponent<Enemy>().activeElement);
                other.GetComponent<Enemy>().HealthTaken(damageDeal, activeElement);
                Debug.Log("Heavy Attack Damage Deal: " + damageDeal);
            }
        }
        //else if (other.CompareTag(Constants.seta))
        {

        }
    }
    public void SlashAttackEnter(Collider other, Element element)
    {
        if (other.CompareTag(Constants.enemy))
        {
            int[] damageDeal;
            damageDeal = gameManager.DamageCalulator(element, 0, heavyAttackMagicDamage, other.GetComponent<Enemy>().activeElement);
            other.GetComponent<Enemy>().HealthTaken(damageDeal, element);
            Debug.Log("Slash Attack Damage Deal: " + damageDeal);
        }
        else if (other.CompareTag(Constants.tower))
        {
            if (gameManager.ElementInteraction(other.GetComponent<Tower>().activeElement, activeElement) < 0)
            {
                if (activeElement == Element.Earth) earthMana = 0;
                else if (activeElement == Element.Water) waterMana = 0;
                else if (activeElement == Element.Fire) fireMana = 0;
                else if (activeElement == Element.Electric) electricMana = 0;
                Debug.Log("TowerHit");
                other.GetComponent<Tower>().HealthTaken(55);
            }
            else if (other.GetComponent<Tower>().activeElement == Element.None && activeElement != Element.None)
            {
                other.GetComponent<Tower>().HealthTaken(150);
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
    }
    public void HealthTaken(int[] healthTaken)
    {
        if (!isRolling)
        {
            hitParticles.SetActive(false);
            hitParticles.SetActive(true);
            AudioManager.Instance?.Play("HitMarker");
            healthPoints -= healthTaken[0] + healthTaken[1];
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
            Instantiate(deathParticle, deathParticlePosition.position, Quaternion.identity, null);
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
            shineElement = 0;
        }
        else
        {
            AudioManager.Instance.Play("PickUpOrbe");
            if (element == Element.Earth && activeElement != Element.Earth)
            {
                earthMana += mana;
                if (earthMana > 100) earthMana = 100;
                elementsHUD.earthAdd(earthMana);
                shineElement = 1;
            }
            else if (element == Element.Water && activeElement != Element.Water)
            {
                waterMana += mana;
                if (waterMana > 100) waterMana = 100;
                elementsHUD.waterAdd(waterMana);
                shineElement = 2;
            }
            else if (element == Element.Fire && activeElement != Element.Fire)
            {
                fireMana += mana;
                if (fireMana > 100) fireMana = 100;
                elementsHUD.fireAdd(fireMana);
                shineElement = 3;
            }
            else if (element == Element.Electric && activeElement != Element.Electric)
            {
                electricMana += mana;
                if (electricMana > 100) electricMana = 100;
                elementsHUD.lightningAdd(electricMana);
                shineElement = 4;
            }
        }
        isShiningUp = true;
        shineTimer = 0;
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
        shineElement = 5;
        isShiningUp = true;
        shineTimer = 0;
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
