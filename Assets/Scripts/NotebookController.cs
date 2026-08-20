using System.Collections.Generic;
using Unity.VisualScripting;
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
        //AddNote(2,true);
        //AddNote(1,true);
    }
    public void AddNote(int id , bool popup)
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

                    if(popup) ShowNotebookPopup(newNote.name);

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

    public virtual void ShowNotebookPopup(string name)
    {
        if (PopupManager.Instance == null)
        {
            Debug.LogWarning("PopupManager was not found.");
            return;
        }

        PopupManager.Instance.ShowNotebookPopup(
            name,
            ""
        );
    }
    // Loading at start
    public void LoadListOfNotes(List<int> notes) 
    {
        foreach(int note in notes)
        {
            AddNote(note,false);
        }
    }
}
