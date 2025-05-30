using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    public Npc npc;

    bool isTalking = false;

    float curResponseTracker = 0;

    public GameObject dialogueUI;

    [Header("Paneles/Textos")]
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI npcDialogueBox;
    public TextMeshProUGUI playerResponse;

    void Update()
    {
        if (dialogueUI.activeSelf)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                curResponseTracker++;
                if (curResponseTracker >= npc.playerDialogue.Length - 1)
                {
                    curResponseTracker = npc.playerDialogue.Length - 1;
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                curResponseTracker--;
                if (curResponseTracker < 0)
                {
                    curResponseTracker = 0;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            if (!isTalking)
            {
                StartConversation();
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            EndDialogue();
        }
    }
    private void StartConversation()
    {
        isTalking = false;
        curResponseTracker = 0;
        dialogueUI.SetActive(true);
        npcName.text = npc.name;
        npcDialogueBox.text = npc.dialogue[0];
    }
    private void EndDialogue()
    {
        isTalking = false;
        dialogueUI.SetActive(false);
    }
}
