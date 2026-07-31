using System.Collections.Generic;
using UnityEngine;

public class NoteDictionary : MonoBehaviour
{
    public static NoteDictionary Instance { get; private set; }
    private Dictionary<int, NoteSO> noteDictionary;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        noteDictionary = new Dictionary<int, NoteSO>();

        NoteSO[] loadedNotes = Resources.LoadAll<NoteSO>("NotesData");

        for (int i = 0; i < loadedNotes.Length; i++)
        {
            if (loadedNotes[i] != null)
            {
  
                loadedNotes[i].noteID = i + 1;
                noteDictionary[loadedNotes[i].noteID] = loadedNotes[i];
            }
        }

        Debug.Log($" {loadedNotes.Length} loaded done !");
    }

    public NoteSO GetNoteByID(int id)
    {
        if (noteDictionary.TryGetValue(id, out NoteSO note)) return note;

        Debug.LogWarning($"Note with ID {id} not found in dictionary!");
        return null;
    }
}