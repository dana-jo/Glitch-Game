using System.Collections;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // multiple dialogues managing
    // multiple portraits
    // PORTRAIT - find a way to add the portraits to the npc not the dialogue
    // pause + sound systems
    // rewards

    public Dialogue dialogueData;
    public bool isDialogueActive;

    private DialogueController dialogueUI;
    private int dialogueIndex;
    private QuestDictionary questDictionary;
    private bool isTyping;

    private enum QuestState { NotStarted, Inprogress, Completed, HandedIn }
    private QuestState questState = QuestState.NotStarted;

    void Start()
    {
        dialogueUI = DialogueController.Instance;
        questDictionary = QuestDictionary.Instance;
    }

    void Update()
    {
        
    }

    public void StartDialogue()
    {
        // sync with quest data
        SyncQuestState();

        Debug.Log(questState);

        // set dialogue lines
        if (questState == QuestState.NotStarted)
        {
            dialogueIndex = 0;
        }
        else if (questState == QuestState.Inprogress)
        {
            dialogueIndex = dialogueData.questInProgressIndex;
        }
        else if (questState == QuestState.Completed)
        {
            dialogueIndex = dialogueData.questCompletedIndex;
        }

        isDialogueActive = true;

        dialogueUI.SetNPCInfo(dialogueData.npcname, dialogueData.portrait);
        dialogueUI.ShowDialogueUI(true);

        //pauseController.SetPaused(true);

        DisplayCurrentLine();
    }

    private void SyncQuestState()
    {
        if (questDictionary.GetQuest(dialogueData.questID) == null)
            return;

        Debug.Log(questState);
        int questID = dialogueData.questID;

        // quest state
        if (QuestController.Instance.IsQuestCompleted(questID) || QuestController.Instance.IsQuestHandedIn(questID))
        {
            Debug.Log("if 1");
            questState = QuestState.HandedIn;
        }
        //else if (QuestController.Instance.IsQuestCompleted(questID) || !QuestController.Instance.IsQuestHandedIn(questID))
        //{
        //    Debug.Log("if 2");
        //    questState = QuestState.Completed;
        //}
        else if (QuestController.Instance.IsQuestActive(questID))
        {
            Debug.Log("if 3");
            questState = QuestState.Inprogress;
        }
        else
        {
            Debug.Log("if 4");
            questState = QuestState.NotStarted;
        }
    }

    public void NextLine()
    {
        if (isTyping)
        {
            // skip animation and show full line
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex].line);
            isTyping = false;
            return;
        }

        dialogueUI.ClearChoices();

        if (dialogueData.dialogueLines[dialogueIndex].endProgress)
        {
            EndDialogue();
            return;
        }
        if (dialogueData.dialogueLines[dialogueIndex].choices.Length > 0)
        {
            DisplayChoices(dialogueData.dialogueLines[dialogueIndex].choices);
            return;
        }

        if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            // if there's another line, type it
            DisplayCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        //isTyping = true;
        //dialogueUI.SetDialogueText("");

        //foreach (char letter in dialogueData.dialogueLines[dialogueIndex].line)
        //{
        //    dialogueUI.SetDialogueText(dialogueUI.dialogueText.text += letter);
        //    //SoundEffectManager.PlayVoice(dialogueData.voiceSound, dialogueData.voicePitch);
        //    yield return new WaitForSeconds(dialogueData.typingSpeed);
        //}

        //isTyping = false;

        //if (dialogueData.dialogueLines[dialogueIndex].autoProgress)
        //{
        //    yield return new WaitForSeconds(dialogueData.autoProgressDelay);
        //    NextLine();
        //}

        // -----------------
        isTyping = true;

        dialogueUI.SetDialogueText("");
        string currentText = "";
        string line = dialogueData.dialogueLines[dialogueIndex].line;

        //foreach (char letter in dialogueData.dialogueLines[dialogueIndex].line)
        //{
        //    currentText += letter;
        //    dialogueUI.SetDialogueText(currentText);

        //    yield return new WaitForSeconds(dialogueData.typingSpeed);
        //}

        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] == '\\' && i + 1 < line.Length && line[i + 1] == 'n')
            {
                currentText += '\n';
                i++;
            }
            else
            {
                currentText += line[i];
            }

            dialogueUI.SetDialogueText(currentText);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if (dialogueData.dialogueLines[dialogueIndex].autoProgress)
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    void DisplayChoices(DialogueChoice[] choices)
    {
        //dialogueUI.ShowChoicesLayout();
        //dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex].line);
        foreach (DialogueChoice choice in choices)
        {
            int nextIndex = choice.nextDialogueIndex;
            bool givesQuest = choice.givesQuest;
            dialogueUI.CreateChoiceButton(choice.line, () => ChooseOption(nextIndex, givesQuest));
        }
    }

    void ChooseOption(int nextIndex, bool givesQuest)
    {
        if (givesQuest)
        {
            QuestController.Instance.AcceptQuest(dialogueData.questID);
            questState = QuestState.Inprogress;
        }

        dialogueIndex = nextIndex;
        dialogueUI.ClearChoices();
        DisplayCurrentLine();
    }

    void DisplayCurrentLine()
    {
        if(dialogueData.dialogueLines[dialogueIndex].choices.Length > 0)
            dialogueUI.ShowChoicesLayout();
        else
            dialogueUI.ShowNormalDialogueLayout();

        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    public void EndDialogue()
    {
        //if (questState == QuestState.Completed && !QuestController.Instance.IsQuestHandedIn(dialogueData.questID))
        //{
        //    handleQuestCompletion(dialogueData.questID);
        //}

        StopAllCoroutines();
        isDialogueActive = false;
        dialogueUI.ClearDialogueText();
        dialogueUI.ShowDialogueUI(false);
        //pauseController.SetPaused(false);
    }

    void handleQuestCompletion(Quest quest)
    {
        //RewardsController.Instance.GiveQuestRewards(quest);
        //QuestController.Instance.HandInQuest(quest.questID);
    }
}
