using System.Collections.Generic;
using UnityEngine;

public class NotebookController : MonoBehaviour
{
    public static NotebookController Instance { get; private set; }

    public List<int> unlockedNotesIDs { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        unlockedNotesIDs = new List<int>();
    
     
    

}

    private void Start()
    {

        AddNote(1);
        AddNote(2);
    }
    public void AddNote(int id)
    {
        if (!unlockedNotesIDs.Contains(id))
        {
            unlockedNotesIDs.Add(id);

            NoteSO newNote = NoteDictionary.Instance.GetNoteByID(id);

            if (newNote != null)
            {
                if (NotebookUIManager.Instance != null)
                {
                    NotebookUIManager.Instance.WriteNote(newNote);
                    Debug.Log($"Note '{newNote.NoteTitle}' successfully added to the notebook!");
                }
                else
                {
                    Debug.LogWarning("Error: NotebookUIManager instance not found in the scene!");
                }
            }
        }
        else
        {
            Debug.Log($"Player already unlocked note with ID: {id}");
        }
    }
}
