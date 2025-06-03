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
        Debug.Log("Show BoxInfo");
        animator.Play("Complete");
        panelText.text = newText;
    }

    public void EnterText(string newText)
    {
        Debug.Log("Enter BoxInfo");
        animator.SetTrigger("EnterBoxInfo");
        panelText.text = newText;
    }
    public void HideText()
    {
        Debug.Log("Exit BoxInfo");
        animator.SetTrigger("ExitBoxInfo");
    }
}
