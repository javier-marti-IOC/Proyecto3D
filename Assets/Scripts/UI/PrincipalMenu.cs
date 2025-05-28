using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PrincipalMenu : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject optionsPanel;
    public GameObject exitPanel;
    public GameObject newGamePanel;

    private InputAction cancelAction;

    void Awake()
    {
        var playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        cancelAction = playerInput.actions["Cancel"];
        cancelAction.performed += OnCancelPerformed;
    }

    void OnDestroy()
    {
        cancelAction.performed -= OnCancelPerformed;
    }

    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Cancel en PrincipalMenu");

        if (optionsPanel != null && optionsPanel.activeInHierarchy)
        {
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
        if (newGamePanel.activeInHierarchy)
        {
            // Llamar manualmente a la función GoBack del BackToPanel
            BackToPanel backScript = newGamePanel.GetComponent<BackToPanel>();
            if (backScript != null)
            {
                backScript.GoBack();
                return;
            }
        }
    }

    public void quitGame()
    {
        Debug.Log("Sortir del joc");
        Application.Quit();
    }
}
