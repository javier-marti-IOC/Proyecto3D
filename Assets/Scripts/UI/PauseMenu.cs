using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject hudPanel;
    public GameObject optionsPanel;
    public GameObject pausePanel;
    public GameObject exitPanel;
    public GameObject deathPanel;
    public GameObject endGamePanel;

    [Header("Botones por defecto")]
    public Button selectedButton;
    public Button selectedDeathButton;
    public Button endGameButton;

    private bool isPaused = false;
    private InputAction pauseAction;

    private InputAction cancelAction;
    private PlayerInput playerInput;
    void Awake()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        playerInput.actions.FindActionMap("UI").Disable();
        playerInput.actions.FindActionMap("Player").Enable();

        pauseAction = playerInput.actions["Pause"];
        cancelAction = playerInput.actions["Cancel"];
        
        
        pauseAction.performed += OnPausePerformed;
        cancelAction.performed += OnCancelPerformed;
    }

    void OnDestroy()
    {
        pauseAction.performed -= OnPausePerformed;
        cancelAction.performed -= OnCancelPerformed;
    }

    private void OnCancelPerformed(InputAction.CallbackContext context)
{
    Debug.Log("Cancel");
    
    if (optionsPanel.activeInHierarchy)
    {
        // Llamar manualmente a la función GoBack del BackToPanel
        BackToPanel backScript = optionsPanel.GetComponent<BackToPanel>();
        if (backScript != null)
        {
            backScript.GoBack();
            return;
        }
    }
    if (exitPanel.activeInHierarchy)
    {
        // Llamar manualmente a la función GoBack del BackToPanel
        BackToPanel backScript = exitPanel.GetComponent<BackToPanel>();
        if (backScript != null)
        {
            backScript.GoBack();
            return;
        }
    }

    // Si no estamos en opciones, se puede cerrar el menú de pausa
        if (pausePanel.activeInHierarchy && !deathPanel.activeInHierarchy && !endGamePanel.activeInHierarchy)
        {
            TogglePause();
        }
}

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        // Solo permitir pausar si no estás muerto o en final de partida
        if (!deathPanel.activeInHierarchy && !endGamePanel.activeInHierarchy && !optionsPanel.activeSelf && !exitPanel.activeInHierarchy)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        
        if (!deathPanel.activeInHierarchy && !optionsPanel.activeSelf && !exitPanel.activeInHierarchy) // NO BORRAR
        {  
            isPaused = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0f;
                pausePanel.SetActive(true);
                selectedButton.Select();
                hudPanel.SetActive(false);
                playerInput.actions.FindActionMap("Player").Disable();
                playerInput.actions.FindActionMap("UI").Enable();
            }
            else
            {
                playerInput.actions.FindActionMap("UI").Disable();
                playerInput.actions.FindActionMap("Player").Enable();
                Time.timeScale = 1f;
                pausePanel.SetActive(false);
                hudPanel.SetActive(true);
                exitPanel.SetActive(false);
                endGamePanel.SetActive(false);
                deathPanel.SetActive(false);
            }
        }
    }

    public void ResumeGame() // Botón "Continuar"
    {
        playerInput.actions.FindActionMap("UI").Disable();
        playerInput.actions.FindActionMap("Player").Enable();
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        hudPanel.SetActive(true);
    }

    public void backToMenu() // Botón "Volver al menú principal"
    {
        SceneManager.LoadScene(0);
        Debug.Log("Sortir al menú");
        Time.timeScale = 1f;
    }

    public void ToggleDeath()
    {
        hudPanel.SetActive(false);
        deathPanel.SetActive(true);
        selectedDeathButton.Select();
    }

    public void ToggleEndgame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        hudPanel.SetActive(false);
        endGamePanel.SetActive(true);
        endGameButton.Select();
    }
}
