using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AIChatController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private NotebookController _notebookController;
    private IAIChatService _aiService;

    private void Awake()
    {
        
        _aiService = GetComponent<IAIChatService>();

        if (_aiService == null)
        {
            Debug.LogError("[AIChatController] No component implementing IAIChatService found on this GameObject!");
        }

        if (_notebookController == null)
        {
            _notebookController = NotebookController.Instance != null
                ? NotebookController.Instance
                : GetComponentInParent<NotebookController>();
        }

        if (_notebookController == null)
        {
            Debug.LogWarning("[AIChatController] NotebookController reference is missing!");
        }
    }

 
    public void SubmitQuestion(string question, Action<string> onAnswerReceived)
    {
        if (string.IsNullOrWhiteSpace(question)) return;

        Debug.Log($"[AIChatController] Received question: {question}");

        if (_aiService != null)
        {
           
            string rawPrompt = BuildAntiSpoilerPrompt(question);

            Debug.Log($"<color=cyan>[AIChatController Sent Prompt]:</color>\n{rawPrompt}");

            _aiService.AskQuestion(rawPrompt, onAnswerReceived);
        }
        else
        {
            Debug.LogError("[AIChatController] IAIChatService reference is missing!");
        }
    }


  

   
    private string BuildAntiSpoilerPrompt(string userQuestion)
    {
        StringBuilder sb = new StringBuilder();

       
        sb.AppendLine("System Instruction: You are an immersive in-game AI assistant designed to help the player.");
        sb.AppendLine("OUTPUT RULE: Provide ONLY the final answer to the player. NEVER output your thinking process, internal logic, analysis, or introductory commentary (e.g., do NOT say 'Based on your notes...'). Speak directly to the player in-character.");
        sb.AppendLine("CRITICAL RULE: You must NEVER spoil puzzles, reveal locations, or mention information unless it is explicitly listed in the 'Unlocked Knowledge' section below.");
        sb.AppendLine("FALLBACK RULE: If the player asks about something NOT in the 'Unlocked Knowledge', you MUST reply with a VERY SHORT, mysterious message (max 1 or 2 sentences) stating that this information is 'Classified', 'Top Secret', or 'Unknown', and tell them to find more clues.\n");

        sb.AppendLine("--- UNLOCKED KNOWLEDGE ---");
       
        Debug.Log($"[Check Data]: NotebookController is null? {_notebookController == null} | IDs Count: {(_notebookController != null ? _notebookController.unlockedNotesIDs.Count.ToString() : "N/A")} | Dictionary Instance null? {NoteDictionary.Instance == null}");
        if (_notebookController != null && _notebookController.unlockedNotesIDs != null && _notebookController.unlockedNotesIDs.Count > 0)
        {
            foreach (int noteID in _notebookController.unlockedNotesIDs)
            {
                NoteSO noteData = NoteDictionary.Instance != null ? NoteDictionary.Instance.GetNoteByID(noteID) : null;

                if (noteData != null)
                {
                    StringBuilder noteContentBuilder = new StringBuilder();
                    noteContentBuilder.Append($"[Note: {noteData.NoteTitle}] ");

                    if (noteData.Elements != null)
                    {
                        foreach (var element in noteData.Elements)
                        {
                            if (element.Type == NoteElementType.Text)
                            {
                                noteContentBuilder.Append(element.TextContent + " ");
                            }
                        }
                    }

                    sb.AppendLine($"- {noteContentBuilder.ToString().Trim()}");
                }
            }
        }
        else
        {
            sb.AppendLine("(No clues discovered yet. The player is at the very beginning of the game.)");
        }

      
        sb.AppendLine("--------------------------\n");
        sb.AppendLine($"Player Question: \"{userQuestion}\"");

        return sb.ToString();
    }
}
/*
using System;
using UnityEngine;

[RequireComponent(typeof(IAIChatService))]
public class AIChatController : MonoBehaviour
{
    private IAIChatService _aiService;

    private void Awake()
    {
        _aiService = GetComponent<IAIChatService>();
    }

    public void SubmitQuestion(string userQuestion, Action<string> onResponseReady)
    {
        if (string.IsNullOrWhiteSpace(userQuestion)) return;

        // Build a simple context without the Notebook logic for now
        string systemPrompt = "System Instruction: You are an immersive in-game AI assistant. Be helpful and brief.\n\nPlayer Question: " + userQuestion;

        Debug.Log("[AIChatController] Submitting question to Service.");
        _aiService.AskQuestion(systemPrompt, onResponseReady);
    }
} */