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
    private bool isSpendingMana;
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
/*     private Color shineNoneColor = Color.white;
    private Color shineEarthColor = Color.green;
    private Color shineWaterColor = Color.blue;
    private Color shineFireColor = Color.red;
    private Color shineElectricColor = Color.yellow;
    private Color shineHealColor = Color.magenta; */
    private Color shineNoneColor = new Color(0.735849f,0.6699003f,0.726575f);
    private Color shineEarthColor = new Color(0.4196078f, 0.6745098f, 0.4f);
    private Color shineWaterColor = new Color(0.2431373f, 0.4862745f, 0.6980392f);
    private Color shineFireColor = new Color(0.7490196f, 0.2666667f, 0.282353f);
    private Color shineElectricColor = new Color(0.8745098f, 0.8431373f, 0.3921569f);
    private Color shineHealColor = new Color(0.4f, 0, 0.4319372f);
    public float shineDuration;
    private int shineElement;
    private Material characterMaterial;
    private bool isShiningUp = false;
    private bool isShiningDown = false;
    private float shineTimer = 0;

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


        characterMaterial = characterRenderer.material;
        characterMaterial.EnableKeyword("_EMISSION");
        characterMaterial.SetColor("_EmissionColor", Color.black);

        CollectMana(Element.None);
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
                characterMaterial.SetColor("_EmissionColor", Color.Lerp(Color.black, shineNoneColor, t));
            }
            else if (shineElement == 1)
            {
                characterMaterial.SetColor("_EmissionColor", Color.Lerp(Color.black, shineEarthColor, t));
            }
            else if (shineElement == 2)
            {
                characterMaterial.SetColor("_EmissionColor", Color.Lerp(Color.black, shineWaterColor, t));
            }
            else if (shineElement == 3)
            {
                characterMaterial.SetColor("_EmissionColor", Color.Lerp(Color.black, shineFireColor, t));
            }
            else if (shineElement == 4)
            {
                characterMaterial.SetColor("_EmissionColor", Color.Lerp(Color.black, shineElectricColor, t));
            }
            else if (shineElement == 5)
            {
                characterMaterial.SetColor("_EmissionColor", Color.Lerp(Color.black, shineHealColor, t));
            }
            if (t >= 1)
            {
                isShiningUp = false;
                isShiningDown = true;
                if (shineElement == 0)
                {
                    characterMaterial.SetColor("_EmissionColor", shineNoneColor);
                }
                else if (shineElement == 1)
                {
                    characterMaterial.SetColor("_EmissionColor", shineEarthColor);
                }
                else if (shineElement == 2)
                {
                    characterMaterial.SetColor("_EmissionColor", shineWaterColor);
                }
                else if (shineElement == 3)
                {
                    characterMaterial.SetColor("_EmissionColor", shineFireColor);
                }
                else if (shineElement == 4)
                {
                    characterMaterial.SetColor("_EmissionColor", shineElectricColor);
                }
                else if (shineElement == 5)
                {
                    characterMaterial.SetColor("_EmissionColor", shineHealColor);
                }
                shineTimer = 0;
            }
        }
        if (isShiningDown)
        {
            shineTimer += Time.deltaTime;
            float t = shineTimer / shineDuration;
            if (shineElement == 0)
            {
                characterMaterial.SetColor("_EmissionColor", Color.Lerp(shineNoneColor, Color.black, t));
            }
            else if (shineElement == 1)
            {
                characterMaterial.SetColor("_EmissionColor", Color.Lerp(shineEarthColor, Color.black, t));
            }
            else if (shineElement == 2)
            {
                characterMaterial.SetColor("_EmissionColor", Color.Lerp(shineWaterColor, Color.black, t));
            }
            else if (shineElement == 3)
            {
                characterMaterial.SetColor("_EmissionColor", Color.Lerp(shineFireColor, Color.black, t));
            }
            else if (shineElement == 4)
            {
                characterMaterial.SetColor("_EmissionColor", Color.Lerp(shineElectricColor, Color.black, t));
            }
            else if (shineElement == 5)
            {
                characterMaterial.SetColor("_EmissionColor", Color.Lerp(shineHealColor, Color.black, t));
            }

            if (t >= 1)
            {
                isShiningDown = false;
                characterMaterial.SetColor("_EmissionColor", Color.black);
            }
            shineElement = -1;
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
            AudioManager.Instance?.Play("ActivateElement");
            if (activeElement == Element.Earth)
            {
                if (isSpendingMana)
                {
                    earthMana = 0;
                    elementsHUD.earthReduce(earthMana);
                    earthEffect.SetActive(false);
                }
                elementsHUD.EarthStopBlink();
            }
            else if (activeElement == Element.Water)
            {
                if (isSpendingMana)
                {
                    waterMana = 0;
                    elementsHUD.waterReduce(waterMana);
                    waterEffect.SetActive(false);
                }
                elementsHUD.WaterStopBlink();
            }
            else if (activeElement == Element.Fire)
            {
                if (isSpendingMana)
                {
                    fireMana = 0;
                    elementsHUD.fireReduce(fireMana);
                    fireEffect.SetActive(false);
                }
                elementsHUD.FireStopBlink();
            }
            else if (activeElement == Element.Electric)
            {
                if (isSpendingMana)
                {
                    electricMana = 0;
                    elementsHUD.lightningReduce(electricMana);
                    electricEffect.SetActive(false);
                }
                elementsHUD.LightningStopBlink();
            }
            activeElement = element;
            isSpendingMana = false;
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
    }
    public void StartSlashAttack()
    {
        if (activeElement != Element.None)
        {
            Instantiate(slash, swordCollider.transform.position, Quaternion.identity, null);
            if (activeElement == Element.Earth)
            {
                earthMana -= 35;
                elementsHUD.earthReduce(earthMana);
            }
            else if (activeElement == Element.Water)
            {
                waterMana -= 35;
                elementsHUD.waterReduce(waterMana);
            }
            else if (activeElement == Element.Fire)
            {
                fireMana -= 35;
                elementsHUD.fireReduce(fireMana);
            }
            else if (activeElement == Element.Electric)
            {
                electricMana -= 35;
                elementsHUD.lightningReduce(electricMana);
            }
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
                other.GetComponent<Tower>().HealthTaken(34);
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
        isShiningDown = false;
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
