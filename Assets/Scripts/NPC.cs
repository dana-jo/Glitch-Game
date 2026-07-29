using UnityEngine;

public class NPC : MonoBehaviour, Interactable
{
    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = GetComponent<DialogueManager>();
    }
    public bool CanInteract()
    {
        return !dialogueManager.isDialogueActive;
    }

    public void Interact()
    {
        //if (dialogueData == null || (pauseController.IsGamePaused && !isDialogueActive))
        //    return;

        if (dialogueManager.isDialogueActive)
        {
            dialogueManager.NextLine();
        }
        else
        {
            dialogueManager.StartDialogue();
        }
    }
}
