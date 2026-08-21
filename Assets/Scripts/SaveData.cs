using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    //public string mapBoundary; //map boundary name
    public List<InventorySaveData> inventorySaveData;
    public List<InventorySaveData> hotbarSaveData;
    public List<PuzzleSaveData> stateOfPuzzles; //true if puzzle is solved, false if not
    public List<SequenceStep> playerSequenceCBPuzzle; // code blocks puzzle solved sequence
    public List<ChestSaveData> chestSaveData;
    public List<QuestSaveData> activeQuestProgressData;
    public List<int> handingQuestIDs;
    public List<int> unlockedNotesIDs;
}