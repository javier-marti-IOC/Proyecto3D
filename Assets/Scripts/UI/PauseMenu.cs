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

    void Awake()
    {
        var playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        pauseAction = playerInput.actions["Pause"];
        pauseAction.performed += OnPausePerformed;
    }

    void OnDestroy()
    {
        pauseAction.performed -= OnPausePerformed;
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        // Solo permitir pausar si no estás muerto o en final de partida
        if (!deathPanel.activeInHierarchy && !endGamePanel.activeInHierarchy && !optionsPanel.activeSelf)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        
        if (!deathPanel.activeInHierarchy && !endGamePanel.activeInHierarchy && !optionsPanel.activeSelf) // NO BORRAR
        {  
            isPaused = !isPaused;  
            if (isPaused)
            {
                Time.timeScale = 0f;
                pausePanel.SetActive(true);
                selectedButton.Select();
                hudPanel.SetActive(false);
            }
            else
            {
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
