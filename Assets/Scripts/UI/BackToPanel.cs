using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script que permite utilizar "B" o "ESC para salir de un panel al anterior.
public class BackToPanel : MonoBehaviour
{
    public GameObject panelToBack; // Panel objetivo al que se quiere volver
    public Button selectedButton; // Boton seleccionado por defecto al cargar el panel anterior

    void Update()
    {
        // Detectar el botón B en el mando o Escape en teclado
        if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelToBack != null)
            {
                panelToBack.SetActive(true); // Activar el panel destino
                gameObject.SetActive(false); // Desactivar este panel
                selectedButton.Select(); // Seleccionar boton por defecto
            }
            else
            {
                Debug.LogWarning("Panel no asignado en el Inspector.");
            }
        }
    }
}
