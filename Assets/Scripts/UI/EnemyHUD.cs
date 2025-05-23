using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHUD : MonoBehaviour
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
    // public Image levelFill1; // Primera barra de nivel
    // public Image levelFill2; // Segunda barra de nivel
    // public Image levelFill3; // Tercera barra de nivel
    // public Image levelFill4; // Cuarta barra de nivel

    [Header("Health")]
    public Image healthBar; // Barra de vida
    public Image healthBarGhost; // Barra de vida fantasma

    // VARIABLES PRIVADAS
    public Enemy enemyScript; // Variable que permite el acceso al script del enemigo
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
        //enemyScript = GetComponent<Enemy>(); // Obtiene la referencia al enemigo.
        maxHealth = enemyScript.maxHealthPoints;
        healthBar.color = healthColor; // Se asigna el color de la vida
        healthBarGhost.color = healthGhostColor; // Se asigna el color de la vida fantasma
        healthBar.fillAmount = 1f;
        healthBarGhost.fillAmount = 1f;
        UpdateHUD(); // Se actualiza el hud con los valores del enemigo por defecto.
        hudPanelCanvas.SetActive(false); // Se oculta el HUD por defecto.
        
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
        if (enemyScript != null) {

            // Asignar nivel
            levelText.text = "" + enemyScript.enemyLevel.ToString(); // Texto del nivel

            // Icono del nivel
            // if(enemyScript.level == 4)
            // {
            //     levelFill1.gameObject.SetActive(true);
            //     levelFill2.gameObject.SetActive(true);
            //     levelFill3.gameObject.SetActive(true);
            //     levelFill4.gameObject.SetActive(true);

            // } else if(enemyScript.level == 3)
            // {
            //     levelFill1.gameObject.SetActive(true);
            //     levelFill2.gameObject.SetActive(true);
            //     levelFill3.gameObject.SetActive(true);
            //     levelFill4.gameObject.SetActive(false);

            // } else if(enemyScript.level == 2)
            // {
            //     levelFill1.gameObject.SetActive(true);
            //     levelFill2.gameObject.SetActive(true);
            //     levelFill3.gameObject.SetActive(false);
            //     levelFill4.gameObject.SetActive(false);

            // } else
            // {
            //     levelFill1.gameObject.SetActive(true);
            //     levelFill2.gameObject.SetActive(false);
            //     levelFill3.gameObject.SetActive(false);
            //     levelFill4.gameObject.SetActive(false);
            // }
        }

        // Asignar elemento
        if (enemyScript.activeElement == Element.Fire)
        {
            waterIcon.gameObject.SetActive(false);
            earthIcon.gameObject.SetActive(false);
            lightningIcon.gameObject.SetActive(false);
            fireIcon.gameObject.SetActive(true);
        } else if (enemyScript.activeElement == Element.Water)
        {
            fireIcon.gameObject.SetActive(false);
            earthIcon.gameObject.SetActive(false);
            lightningIcon.gameObject.SetActive(false);
            waterIcon.gameObject.SetActive(true);
        } else if (enemyScript.activeElement == Element.Earth)
        {
            fireIcon.gameObject.SetActive(false);
            waterIcon.gameObject.SetActive(false);
            lightningIcon.gameObject.SetActive(false);
            earthIcon.gameObject.SetActive(true);
        }
        else if (enemyScript.activeElement == Element.Electric)
        {
            fireIcon.gameObject.SetActive(false);
            waterIcon.gameObject.SetActive(false);
            earthIcon.gameObject.SetActive(false);
            lightningIcon.gameObject.SetActive(true);
        }

        // Asignar vida
        UpdateHealth(enemyScript.healthPoints);
    }
    

    // Actualiza solo la vida del enemigo
    public void UpdateHealth(float newHealth)
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
