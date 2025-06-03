using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class StonePillar : MonoBehaviour
{
    public int tower_id;
    public string[] stonePillarPhrases = new string[] {};
    public TextHUD textHUD;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance?.Play("Onomatopeia");
            textHUD.ChangeText(stonePillarPhrases[tower_id]);
            textHUD.ShowBoxText();
            // fadePanel.ShowPanel(stonePillarPhrases[tower_id]);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textHUD.HideBoxText();
            //fadePanel.HidePanel();
        }
    }
}
