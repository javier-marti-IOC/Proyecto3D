using System;
using System.Collections;
using System.Collections.Generic;
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

    public bool OnAction;
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
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(basicAttackAction.WasPerformedThisFrame());
        Debug.Log("Heavy: "+heavyAttackAction.ReadValue<float>());
        dpadValue = dpadAction.ReadValue<Vector2>();
        if (!OnAction && healthPoints > 0)
        {
            Debug.Log(dpadValue);
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
            if (basicAttackAction.WasPerformedThisFrame())
            {
                BasicAttack();
            }
            if (heavyAttackAction.ReadValue<float>() > 0.5f)
            {
                HeavyAttack();
            }
            if (rollAction.WasPerformedThisFrame())
            {
                Roll();
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
    }
    public void Roll()
    {
        animator.SetTrigger("Roll");
        OnAction = true;
    }
    public void HeavyAttack()
    {
        animator.SetTrigger("HardAttack");
        OnAction = true;
    }
    public void Dying()
    {
        animator.SetTrigger("Dying");
        OnAction = true;
    }

    public void BasickAttackEnter(Collider other)
    {

    }
    public void EndAction()
    {
        OnAction = false;
    }
}
