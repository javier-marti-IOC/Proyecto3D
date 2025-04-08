using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy : MonoBehaviour
{

    protected Element activeElement;
    protected int healthPoints;
    protected GameObject player;
    protected GameObject tower;


    //Booleans
    protected bool towerCalling; // Booleana para saber cuando la torre nos esta llamando
    protected bool onAction; // Esta realizando alguna accion
    protected bool onCombat; // El enemigo esta en combate
    protected bool onHealZone; // Esta el enemigo en zona de cura de la torre
    protected bool playerDetected; // Detecto al player
    protected bool playerInAttackRange; // Esta el player en mi zona de ataque
    protected bool towerInRange; // Tengo la torre en rango para patrullar

    // Ranges
    [SerializeField]
    protected int minCooldownTimeInclusive; /* Tiempo minimo inclusivo del rango 
                                            (este numero si entra en el rango)*/
    [SerializeField]
    protected int maxCooldownTimeExclusive; /* Tiempo maximo exclusivo del rango 
                                            (este numero no entra en el rango)*/

    //Cooldowns
    protected float cooldownHeavyAttack; // Cooldown para volver a realizar ataque fuerte

    // Start is called before the first frame update
    void Start()
    {
        healthPoints = 100;
        player = GameObject.FindGameObjectWithTag(Constants.player);
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void Heal()
    {

    }

    protected void Patrol()
    {

    }

    protected void TowerPatrol()
    {

    }
}
