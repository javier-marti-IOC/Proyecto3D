using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC file", menuName = "NPC Files Archive")]
public class Npc : ScriptableObject
{
    public string npcName;
    public DialogueLine[] lines;
}