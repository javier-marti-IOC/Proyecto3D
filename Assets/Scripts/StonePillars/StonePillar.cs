using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class StonePillar : MonoBehaviour
{
    public int tower_id;
    public string[] stonePillarPhrases = new string[] {};


    public GameObject panel;
    public FadePanel fadePanel;
    public bool readingPillar;


    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            fadePanel.ShowPanel(stonePillarPhrases[tower_id]);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            fadePanel.HidePanel();
        }
    }
}
