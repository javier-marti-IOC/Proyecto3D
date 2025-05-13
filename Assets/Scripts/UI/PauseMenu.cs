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

    public Button selectedButton; 
    private bool isPaused = false;

    void Update()
    {
        if(deathPanel.activeInHierarchy == false)
        {
            if(pausePanel.activeInHierarchy && Input.GetKeyDown(KeyCode.JoystickButton1))
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
           
        }
    }

    public void ResumeGame() // Script para boton continuar
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }
    public void backToMenu() { // Script para volver al menu principal
        SceneManager.LoadScene(0); 
        Debug.Log("Sortir al ménu");
    }
}
