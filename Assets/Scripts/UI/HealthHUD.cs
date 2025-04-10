using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    Color32 healthColor = new Color32(166, 24, 22, 255); //rgb(166, 24, 22) Rojo para la vida
    Color32 healthGhostColor = new Color32(192, 162, 14, 255); //rgb(192, 162, 14) Amarillo para la barra fantasma

    public Image healthBar; // Barra de vida normal
    public Image healthBarGhost; // Barra fantasma que mantiene por un tiempo la vida anterior respecto a la nueva al recibir daño

    public float maxHealth = 100f; // Valor maximo de la barra de vida
    public float reductionSpeed = 2f; // Velocidad de reduccion de la barra de vida.
    public float ghostDelay = 0.5f;      // Tiempo antes de que la barra fantasma empiece a bajar


    private float actualHealth; // Vida actual
    private float ghostDelayTimer = 0f;  // Temporizador para el retraso
    private bool waitingToReduce = false;


    private void Start()
    {
        healthBar.color = healthColor;
        healthBarGhost.color = healthGhostColor;
        actualHealth = maxHealth;
        healthBar.fillAmount = 1f;
        healthBarGhost.fillAmount = 1f;
    }

    // Funcion para modificar por parametro la barra de vida (Llamar desde manager del Jugador)
    public void SetHealth(float newHealth)
    {
        newHealth = Mathf.Clamp(newHealth, 0f, maxHealth);
        float newFill = newHealth / maxHealth;
        float currentFill = actualHealth / maxHealth;

        // Si perdemos vida, activamos el delay antes de reducir la barra fantasma
        if (newFill < currentFill)
        {
            ghostDelayTimer = ghostDelay;
            waitingToReduce = true;
        }
        // Si ganamos vida, ambas barras se actualizan al instante
        else if (newFill > healthBarGhost.fillAmount)
        {
            healthBarGhost.fillAmount = newFill;
        }

        actualHealth = newHealth;
        healthBar.fillAmount = newFill;
    }

    private void Update()
    {
        float targetFill = actualHealth / maxHealth;

        // Si estamos esperando para reducir la barra fantasma
        if (waitingToReduce)
        {
            ghostDelayTimer -= Time.deltaTime;
            if (ghostDelayTimer <= 0f)
            {
                waitingToReduce = false;
            }
        }
        // Reducir la barra fantasma de forma progresiva
        else if (healthBarGhost.fillAmount > targetFill)
        {
            healthBarGhost.fillAmount = Mathf.Lerp(healthBarGhost.fillAmount, targetFill, Time.deltaTime * reductionSpeed);
        }
    }
}
