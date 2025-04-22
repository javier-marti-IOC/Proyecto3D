using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPanelHUD : MonoBehaviour
{
    public TextMeshProUGUI panelText; // TMP de BoxInfo
    public Animator animator; // Animator de BoxInfo


    public void ShowText(string newText)
    {
        panelText.text = newText;
        animator.Play("Show");
    }
}
