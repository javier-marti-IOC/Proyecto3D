using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

// Script que permite utilizar "B" o "ESC para salir de un panel al anterior.
public class BackToPanel : MonoBehaviour
{
    public GameObject panelToBack; // Panel objetivo al que se quiere volver
    public PauseMenu pauseMenu; // Script menu pausa
    public Button selectedButton; // Boton seleccionado por defecto al cargar el panel anterior
    private InputAction cancelAction;

    void OnEnable()
    {
        // Obtener la acción "Cancel" del mapa de acciones activo
        PlayerInput uiInput = FindObjectOfType<PlayerInput>();
        cancelAction = uiInput.actions["Cancel"];
        cancelAction.performed += OnCancel;
        cancelAction.Enable();
    }

    void OnDisable()
    {
        if (cancelAction != null)
        {
            cancelAction.performed -= OnCancel;
            cancelAction.Disable();
        }
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        if (panelToBack != null)
        {
            panelToBack.SetActive(true); // Mostrar panel anterior
            gameObject.SetActive(false); // Ocultar panel actual
            selectedButton.Select();     // Seleccionar botón predeterminado
        }
        else if (pauseMenu != null) // Cerrar el menu de pausa
        {
            pauseMenu.TogglePause();
        }
        else
        {
            Debug.LogWarning("Panel no asignado en el Inspector.");
        }
    }
}
