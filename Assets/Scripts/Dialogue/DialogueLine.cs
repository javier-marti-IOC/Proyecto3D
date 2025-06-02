using UnityEngine;

[System.Serializable]
public class DialogueOption
{
    public string question;      // Texto de la pregunta (opción)
    public string answer;        // Respuesta del NPC para esa pregunta
}

[System.Serializable]
public class DialogueLine
{
    public string npcIntroText;          // Texto inicial del NPC (ejemplo: "¿Qué quieres saber?")
    public DialogueOption[] options;     // Opciones/preguntas que el jugador puede elegir
}