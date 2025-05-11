using System;
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


    public Element activeElement;
    public Animator animator;
    public int healthPoints;
    public GameObject swordCollider;
    private bool isBasicAttack;
    public int basicAttackBasicDamage;
    public int basicAttackMagicDamage;
    public int heavyAttackBasicDamage;
    public int heavyAttackMagicDamage;
    public GameManager gameManager;

    public bool OnAction;
    public bool isRolling;
    public float rollCooldown;
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
            if (healthPoints <= 0)
            {
                Dying();
            }
        }
    }

    private void ChangeElement(Element element)
    {
        activeElement = element;
        /*
        if (element == Element.Earth && earthMana == 100)
        {
            activeElement = Element.Earth;
        }
        else if (element == Element.Water && waterMana == 100)
        {
            activeElement = Element.Earth;
        }
        else if (element == Element.Fire && fireMana == 100)
        {
            activeElement = Element.Earth;
        }
        else if (element == Element.Electric && electricMana == 100)
        {
            activeElement = Element.Electric;
        }*/
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
        if (other.tag.Equals(Constants.enemy))
        {
            int damageDeal;
            if (isBasicAttack)
            {
                damageDeal = gameManager.DamageCalulator(activeElement,basicAttackBasicDamage,basicAttackMagicDamage,other.GetComponent<Enemy>().activeElement);
                other.GetComponent<Enemy>().HealthTaken(damageDeal);
                Debug.Log("Basic Attack Damage Deal: " + damageDeal);
            }
            else
            {
                damageDeal = gameManager.DamageCalulator(activeElement,heavyAttackBasicDamage,heavyAttackMagicDamage,other.GetComponent<Enemy>().activeElement);
                other.GetComponent<Enemy>().HealthTaken(damageDeal);
                Debug.Log("Heavy Attack Damage Deal: " + damageDeal);
            }
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
        }
    }
}
