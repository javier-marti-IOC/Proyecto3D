using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public Npc npc;

    public GameObject dialogueUI;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI npcDialogueText;

    public Button[] optionButtons; // Asigna en inspector los 3 botones fijos
    public TextMeshProUGUI[] optionButtonTexts; // Asigna los textos hijos de los botones en inspector

    private int currentLineIndex = 0;
    private bool waitingForAnswer = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowDialogue();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideDialogue();
        }
    }

    void Update()
    {
        if (!dialogueUI.activeSelf || waitingForAnswer)
            return;

        if (optionButtons.Length > 0 && Input.GetKeyDown(KeyCode.H))
            SelectOption(0);
        if (optionButtons.Length > 1 && Input.GetKeyDown(KeyCode.J))
            SelectOption(1);
        if (optionButtons.Length > 2 && Input.GetKeyDown(KeyCode.K))
            SelectOption(2);
    }

    void ShowDialogue()
    {
        dialogueUI.SetActive(true);
        currentLineIndex = 0;
        waitingForAnswer = false;
        npcNameText.text = npc.npcName;
        DisplayLine(currentLineIndex);
    }

    void HideDialogue()
    {
        dialogueUI.SetActive(false);
        ClearOptions();
    }

    void DisplayLine(int lineIndex)
    {
        if (lineIndex >= npc.lines.Length)
        {
            HideDialogue();
            return;
        }

        waitingForAnswer = false;

        DialogueLine line = npc.lines[lineIndex];
        npcDialogueText.text = line.npcIntroText;

        int optionsToShow = Mathf.Min(line.options.Length, optionButtons.Length);

        string[] keyHints = { "H", "J", "K" }; // letras para cada botón

        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(i < optionsToShow);

            if (i < optionsToShow)
            {
                int optionIndex = i;

                optionButtons[i].onClick.RemoveAllListeners();

                // Añadimos la letra + ": " antes de la pregunta
                optionButtonTexts[i].text = $"({keyHints[i]}) {line.options[i].question}";

                optionButtons[i].onClick.AddListener(() => SelectOption(optionIndex));
            }
        }
    }

    void ClearOptions()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].gameObject.SetActive(false);
        }
    }

    void SelectOption(int optionIndex)
    {
        if (waitingForAnswer)
            return;

        waitingForAnswer = true;
        ClearOptions();

        DialogueLine line = npc.lines[currentLineIndex];

        string answer = line.options[optionIndex].answer;

        npcDialogueText.text = answer;

        StartCoroutine(ReturnToOptionsAfterDelay(3f));
    }

    IEnumerator ReturnToOptionsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        waitingForAnswer = false;
        DisplayLine(currentLineIndex);
    }
}
