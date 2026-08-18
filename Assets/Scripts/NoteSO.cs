using System;
using System.Collections.Generic;
using UnityEngine;

public enum NoteElementType
{
    Text,
    Image
}

[Serializable]
public struct NoteElement
{
    public NoteElementType Type;

    [TextArea(3, 10)]
    public string TextContent; 
    public Sprite ImageContent; 
}

[CreateAssetMenu(fileName = "NewNote", menuName = "Notebook/Note Data")]
public class NoteSO : ScriptableObject
{
    public int noteID; 
    public string NoteTitle;
    [Header("Note Content")]
    public List<NoteElement> Elements; 
}