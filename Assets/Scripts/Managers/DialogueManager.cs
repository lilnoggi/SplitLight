using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image portraitImage;

    private string[] dialogueLines;
    private int currentLine = 0;
    private bool isActive = false;

    public static DialogueManager Instance { get; private set; }
    public bool IsDialogueActive => isActive;
    private bool isTyping = false;

private void Awake()
{
    if (Instance == null)
        Instance = this;
    else
        Destroy(gameObject);
}


    void Update()
    {
            if (isActive && Input.GetKeyDown(KeyCode.Space))
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = dialogueLines[currentLine - 1];
            isTyping = false;
        }
        else
        {
            DisplayNextLine();
        }
    }
    }

    public void StartDialogue(string speakerName, Sprite portrait, string[] lines)
    {
        dialogueLines = lines;
        currentLine = 0;
        isActive = true;

        dialoguePanel.SetActive(true);
        nameText.text = speakerName;
        portraitImage.sprite = portrait;

        DisplayNextLine();
    }

    void DisplayNextLine()
    {
        if (currentLine < dialogueLines.Length)
        {
                    StopAllCoroutines();
        StartCoroutine(TypeLine(dialogueLines[currentLine]));
        currentLine++;
        }
        else
        {
            EndDialogue();
        }
    }
IEnumerator TypeLine(string line)
{
    isTyping = true;
    dialogueText.text = "";
    foreach (char c in line.ToCharArray())
    {
        dialogueText.text += c;
        yield return new WaitForSeconds(0.02f);
    }
    isTyping = false;
}

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isActive = false;
    }
}
