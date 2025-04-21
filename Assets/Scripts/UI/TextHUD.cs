using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextHUD : MonoBehaviour
{
    public TextMeshProUGUI objectTMP; // TMP de BoxText
    public GameObject boxText; // Objeto padre BoxText

    public void Start()
    {
        hideBoxText(); // Al iniciar se desactiva el BoxText
    }

    // Cambiar solo texto
    public void changeText(string newText)
    {
        objectTMP.text = newText;
    }
    
    // Mostrar solo el BoxText
    public void showBoxText()
    {
        boxText.SetActive(true);
    }
    // Ocultar el BoxText
    public void hideBoxText()
    {
        boxText.SetActive(false);
    }
}
