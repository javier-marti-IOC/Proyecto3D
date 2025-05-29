using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ZonePanel : MonoBehaviour
{
    public TextMeshProUGUI textIntroductory; // TMP de BoxInfo
    public TextMeshProUGUI textZoneName; // TMP de BoxInfo
    public Animator animator; // Animator de BoxInfo

    void Update()
    {
        if (Input.GetKeyDown("9"))
        {
            ShowPanel("Placeholder", "Placeholder");
        }
    }
    public void ShowPanel(string newTextIntroductory, string newTextZone)
    {
        Debug.Log("ZonePanel Animation");

        animator.Play("Show");
        textIntroductory.text = newTextIntroductory;
        textZoneName.text = newTextZone;
    }
}
