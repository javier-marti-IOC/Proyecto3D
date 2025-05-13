using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerHUD : MonoBehaviour
{
    // ** AÑADIR ESTE SCRIPT AL OBJETO CANVAS DEL ENEMIGO EN EL PREFAB ** \\

    [Header("Canvas")]
    public GameObject hudPanelCanvas; // Arrastrar el panel del canvas donde se encuentra el HUD del mismo.

    [Header("Icons")]
    public Image earthIcon; // Icono de Tierra
    public Image lightningIcon; // Icono de Rayo
    public Image waterIcon; // Icono de Agua
    public Image fireIcon; // Icono de fuego

    [Header("Level")]
    public TextMeshProUGUI levelText; // TextMeshPro del HUD que indica el nivel.

    [Header("Health")]
    public Image healthBar; // Barra de vida
    public Image healthBarGhost; // Barra de vida fantasma

    // VARIABLES PRIVADAS
    public Tower towerScript; // Variable que permite el acceso al script del enemigo
    private float actualHealth; // Vida actual
    private float reductionSpeed = 2f; // Velocidad de reduccion de la barra de vida.
    private float ghostDelay = 0.5f;      // Tiempo antes de que la barra fantasma empiece a bajar
    private float ghostDelayTimer = 0f;  // Temporizador para el retraso
    private bool waitingToReduce = false;
    private float maxHealth; // Vida maxima del enemigo
    
    // COLORES
    Color32 healthColor = new Color32(93, 75, 122, 255); //rgb(93, 75, 122)
    Color32 healthGhostColor = new Color32(138, 93, 170, 255); // #6a4f96

    void Start()
    {
        maxHealth = towerScript.max_life;
        healthBar.color = healthColor; // Se asigna el color de la vida
        healthBarGhost.color = healthGhostColor; // Se asigna el color de la vida fantasma
        healthBar.fillAmount = 1f;
        healthBarGhost.fillAmount = 1f;
        UpdateHUD(); // Se actualiza el hud con los valores del enemigo por defecto.        
    }

    // Mostrar el HUD
    public void ShowHUD()
    {
        hudPanelCanvas.SetActive(true);
    }

    // Ocultar el HUD
    public void HideHUD()
    {
        hudPanelCanvas.SetActive(false);
    }

    // Actualizar el HUD del enemigo al completo
    public void UpdateHUD()
    {
        if (towerScript != null) {

            // Asignar nivel
            //levelText.text = "" + towerScript.enemyLevel.ToString(); // Texto del nivel
        }

        // Asignar elemento
        if (towerScript.activeElement == Element.Fire)
        {
            waterIcon.gameObject.SetActive(false);
            earthIcon.gameObject.SetActive(false);
            lightningIcon.gameObject.SetActive(false);
            fireIcon.gameObject.SetActive(true);
        } else if (towerScript.activeElement == Element.Water)
        {
            fireIcon.gameObject.SetActive(false);
            earthIcon.gameObject.SetActive(false);
            lightningIcon.gameObject.SetActive(false);
            waterIcon.gameObject.SetActive(true);
        } else if (towerScript.activeElement == Element.Earth)
        {
            fireIcon.gameObject.SetActive(false);
            waterIcon.gameObject.SetActive(false);
            lightningIcon.gameObject.SetActive(false);
            earthIcon.gameObject.SetActive(true);
        }
        else if (towerScript.activeElement == Element.Electric)
        {
            fireIcon.gameObject.SetActive(false);
            waterIcon.gameObject.SetActive(false);
            earthIcon.gameObject.SetActive(false);
            lightningIcon.gameObject.SetActive(true);
        }

        // Asignar vida
        UpdateHealth(towerScript.life);
    }
    

    // Actualiza solo la vida del enemigo
    public void UpdateHealth(float newHealth)
    {
        hudPanelCanvas.SetActive(true);
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
