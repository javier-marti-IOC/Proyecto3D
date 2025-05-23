using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject hudPanel;
    public GameObject optionsPanel;
    [Header("Panel del menú de pausa")]
    public GameObject pausePanel;
    public GameObject exitPanel;
    public GameObject deathPanel;
    public GameObject endGamePanel;

    public Button selectedButton;
    public Button selectedDeathButton;
    public Button endGameButton;
    private bool isPaused = false;

    void Update()
    {
        if (deathPanel.activeInHierarchy == false && endGamePanel.activeInHierarchy == false)
        {
            if (pausePanel.activeInHierarchy && Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                TogglePause();
            }
            // Botón Start de mando Xbox o tecla Escape
            if (Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.Escape) && optionsPanel.activeSelf == false)
            {
                TogglePause();
            }
        }
    }

    public void TogglePause()
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

    public void ResumeGame() // Script para boton continuar
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        hudPanel.SetActive(true);
    }
    public void backToMenu()
    { // Script para volver al menu principal
        SceneManager.LoadScene(0);
        Debug.Log("Sortir al ménu");
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
        isPaused = !isPaused;

        Time.timeScale = 0f;
        hudPanel.SetActive(false);
        endGamePanel.SetActive(true);
        endGameButton.Select();
    }
}
