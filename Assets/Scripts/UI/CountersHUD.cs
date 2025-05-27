using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountersHUD : MonoBehaviour
{
    public GameObject countersPanel;
    public Collider colliderPermitido;

    public void Start()
    {
        if (countersPanel != null)
        {
            countersPanel.SetActive(false);
        }  
    }
    
    // Mostrar solo el BoxText
    public void OnTriggerEnter(Collider other)
    {
        if (other == colliderPermitido)
        {
            countersPanel.SetActive(true);
        }
    }
    // Ocultar el BoxText
    public void OnTriggerExit(Collider other)
    {
        if (other == colliderPermitido)
        {
            countersPanel.SetActive(false);
        }
    }
}
