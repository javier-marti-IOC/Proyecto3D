using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public Npc npc;

    public GameObject dialogueUI;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI npcDialogueText;

    public GameObject optionButtonPrefab; // Prefab del botón (debe tener Button + TextMeshProUGUI)
    public Transform optionsContainer;     // Contenedor donde se instanciarán los botones

    private int currentLineIndex = 0;
    private bool waitingForAnswer = false;
    private List<GameObject> instantiatedButtons = new List<GameObject>();

    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }

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

    void ShowDialogue()
    {
        playerInput.actions.FindActionMap("Player").Disable();   
        playerInput.actions.FindActionMap("UI").Enable();
        dialogueUI.SetActive(true);
        currentLineIndex = 0;
        waitingForAnswer = false;
        npcNameText.text = npc.npcName;
        DisplayLine(currentLineIndex);
    }

    void HideDialogue()
    {
        dialogueUI.SetActive(false);
        playerInput.actions.FindActionMap("UI").Disable();
        playerInput.actions.FindActionMap("Player").Enable();   
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
        ClearOptions();

        DialogueLine line = npc.lines[lineIndex];
        npcDialogueText.text = line.npcIntroText;

        bool first = true;

        // Crear botones de opciones normales
        foreach (DialogueOption option in line.options)
        {
            GameObject buttonObj = Instantiate(optionButtonPrefab, optionsContainer);
            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = option.question;

            int optionIndex = instantiatedButtons.Count;
            button.onClick.AddListener(() => SelectOption(optionIndex));

            instantiatedButtons.Add(buttonObj);

            if (first)
            {
                first = false;
                EventSystem.current.SetSelectedGameObject(buttonObj);
            }
        }

        // Añadir botón adicional "Cerrar"
        GameObject closeButtonObj = Instantiate(optionButtonPrefab, optionsContainer);
        Button closeButton = closeButtonObj.GetComponent<Button>();
        TextMeshProUGUI closeText = closeButtonObj.GetComponentInChildren<TextMeshProUGUI>();

        closeText.text = "Tancar"; // Puedes cambiar el texto aquí

        closeButton.onClick.AddListener(() => HideDialogue());

        instantiatedButtons.Add(closeButtonObj);
    }

    void ClearOptions()
    {
        foreach (GameObject button in instantiatedButtons)
        {
            Destroy(button);
        }

        instantiatedButtons.Clear();
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

    // Añadir botón "Cerrar" tras mostrar la respuesta
    GameObject closeButtonObj = Instantiate(optionButtonPrefab, optionsContainer);
    Button closeButton = closeButtonObj.GetComponent<Button>();
    TextMeshProUGUI closeText = closeButtonObj.GetComponentInChildren<TextMeshProUGUI>();

    closeText.text = "Tancar";
    closeButton.onClick.AddListener(() => HideDialogue());

    instantiatedButtons.Add(closeButtonObj);
    EventSystem.current.SetSelectedGameObject(closeButtonObj); // Opcional: selecciona el botón "Cerrar"
}


    void ResetDialogueOptions()
    {
        waitingForAnswer = false;
        DisplayLine(currentLineIndex);
    }
}
