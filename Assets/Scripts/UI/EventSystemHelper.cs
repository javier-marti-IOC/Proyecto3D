using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemHelper : MonoBehaviour
{
    public GameObject defaultButton;
    private bool panelActive = false;

    void OnEnable()
    {
        panelActive = true;
        StartCoroutine(CheckInitialSelection());
    }

    void OnDisable()
    {
        panelActive = false;
    }

    void Update()
    {
        if (!panelActive) return;

        // Si no hay ningún botón seleccionado en ningún momento
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(defaultButton);
        }
    }

    private System.Collections.IEnumerator CheckInitialSelection()
    {
        yield return null; // Espera un frame para que otros scripts seleccionen su botón
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(defaultButton);
        }
    }
}
