using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ElectricEnemyTest : MonoBehaviour
{
    
    public Transform hand;
    public LineRenderer lightningLine;

    public float attackRange = 10f;
    public float chargeTime = 2.5f;
    public float timeBetweenAttacks = 2f;
    public int damage = 20;
    public Light attackLight;

    //private float attackTimer;

    private Transform player;
    private bool isPlayerInRange = false;
    private Coroutine attackCoroutine;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        lightningLine.enabled = false;
        attackLight.enabled = false;
        //attackTimer = timeBetweenAttacks + chargeTime;
    }

    void Update()
    {
        if (player == null) return;

        //attackTimer -= Time.deltaTime;
        //Debug.Log(attackTimer);

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            if (!isPlayerInRange)
            {
                isPlayerInRange = true;
                attackCoroutine = StartCoroutine(AttackLoop());
            }
        }
        else
        {
            if (isPlayerInRange)
            {
                isPlayerInRange = false;
                if (attackCoroutine != null)
                    StopCoroutine(attackCoroutine);
            }
        }
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return StartCoroutine(ChargeAndShoot());
            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

    IEnumerator ChargeAndShoot()
    {
        attackLight.enabled = true; 
        yield return new WaitForSeconds(chargeTime);
        ShootLightning();
        attackLight.enabled = false;
    }

    void ShootLightning()
    {
        if (player == null) return;

        Vector3 direction = (player.position - hand.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(hand.position, direction, out hit, attackRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                TakeDamage(damage);
            }

            StartCoroutine(ShowLightningEffect(hit.point));
        }
        else
        {

            Vector3 endPoint = hand.position + direction * attackRange;
            StartCoroutine(ShowLightningEffect(endPoint));
        }
    }

    IEnumerator ShowLightningEffect(Vector3 hitPoint)
    {
        lightningLine.SetPosition(0, hand.position);
        lightningLine.SetPosition(1, hitPoint);
        lightningLine.enabled = true;

        yield return new WaitForSeconds(0.1f);

        lightningLine.enabled = false;
    }

    public void TakeDamage(int dmg)
    {
        Debug.Log(dmg + " damage taken");
    }
}
