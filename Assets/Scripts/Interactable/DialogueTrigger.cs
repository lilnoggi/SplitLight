using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Sprite playerPortrait;

    void Start()
    {
        string[] lines = {
            "Huhh... where am I?",
            "This place is... strange.",
            "I need to find some light."
        };

        dialogueManager.StartDialogue("Player", playerPortrait, lines);
    }
}
