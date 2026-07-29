using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public string npcname;
    public Sprite portrait;
    public bool isKnown;

    public DialogueLine[] dialogueLines;

    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 0.5f;

    public AudioClip voiceSound;
    public float voicePitch = 1f;

    public int questInProgressIndex;
    public int questCompletedIndex;
    public int questID;
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
