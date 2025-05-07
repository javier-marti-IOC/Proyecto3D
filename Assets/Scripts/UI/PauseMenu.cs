using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Panel del menú de pausa")]
    public GameObject pausePanel;
    public Button selectedButton; 
    private bool isPaused = false;

    void Update()
    {
        // Botón Start de mando Xbox o tecla Escape
        if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
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
        }
        else
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
           
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
