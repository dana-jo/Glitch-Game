using UnityEngine;

public class NPC : MonoBehaviour, Interactable
{
    public int npcID;

    public string npcName;
    public Sprite portrait;
    //public bool isKnown;    // for later, to show ??? or the npc name

    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = GetComponent<DialogueManager>();
    }
    public bool CanInteract()
    {
        return !dialogueManager.disableDialogue;
    }

    public void Interact()
    {
        //if (dialogueData == null || (pauseController.IsGamePaused && !isDialogueActive))
        if (dialogueManager.dialogueData == null)
            return;

        if (dialogueManager.disableDialogue)
        {
            dialogueManager.NextLine();
        }
        else
        {
            dialogueManager.StartDialogue();
        }
    }
}
