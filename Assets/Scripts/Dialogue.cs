using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public DialogueLine[] dialogueLines;

    [Space(15)]
    [Header("Speed")]
    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 0.05f;

    [Space(15)]
    [Header("Sound")]
    public AudioClip voiceSound;
    public float voicePitch = 1f;

    [Space(15)]
    [Header("Indexes")]
    public int questInProgressIndex;
    public int questCompletedIndex;
    public int questHandedInIndex;

    [Space(15)]
    [Header("Quest related")]
    public int questID;
    public bool isQuestHolder = true;
}

[System.Serializable]
public class DialogueChoice
{
    public string line;
    public int nextDialogueIndex;
    public bool givesQuest;
}

[System.Serializable]
public class DialogueLine
{
    public string line;
    public bool autoProgress;
    public bool endProgress;
    public DialogueChoice[] choices;
}
