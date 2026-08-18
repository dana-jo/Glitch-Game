using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    private PlayerMovement player;

    //IEnumerator Start()
    void Start()
    {
        InitializeComponents();

        // wait one frame so InventoryController.Start() can finish first
        // yield return null;

        LoadGame();
    }

    // private void Update()
    // {

    //     //these are to check id save works 
    //     if (Input.GetKeyDown(KeyCode.F5))
    //     {
    //         SaveGame();
    //     }

    //     if (Input.GetKeyDown(KeyCode.F9))
    //     {
    //         LoadGame();
    //     }

    //     if (Input.GetKeyDown(KeyCode.P))
    //     {
    //         DeleteSave();
    //     }
    // }

    private void InitializeComponents()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindFirstObjectByType<InventoryController>();
        player = FindFirstObjectByType<PlayerMovement>();
    }

    public void SaveGame()
    {
        // inventroy 
        if (inventoryController == null)
        {
            Debug.LogError("Missing InventoryController");
            return;
        }

        SaveData saveData = new SaveData()
        {
            playerPosition = player.transform.position,
            inventorySaveData = inventoryController.GetInventoryItems(),
            hotbarSaveData = inventoryController.GetHotbarItems(),
            stateOfPuzzles = PuzzlesController.Instance.GetPuzzleStates(),
            activeQuestProgressData = QuestController.Instance.GetQuestSaveData(),
            handingQuestIDs = QuestController.Instance.handingQuestIDs,
            unlockedNotesIDs = NotebookController.Instance.unlockedNotesIDs
        };

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveLocation, json);

        Debug.Log("Game saved to: " + saveLocation);
    }

    public void LoadGame()
    {
        if (inventoryController == null)
        {
            Debug.LogError("Missing InventoryController");
            return;
        }

        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            player.transform.position = saveData.playerPosition; // SHOULD I EDIT THE CAMERA POS TOO? 
            inventoryController.SetInventoryItems(saveData.inventorySaveData);
            inventoryController.SetHotbarItems(saveData.hotbarSaveData);
            PuzzlesController.Instance.SetPuzzleStates(saveData.stateOfPuzzles);
            QuestController.Instance.LoadQuestSaveData(saveData.activeQuestProgressData);
            QuestController.Instance.handingQuestIDs = saveData.handingQuestIDs;
            NotebookController.Instance.AddListOfNotes(saveData.unlockedNotesIDs);
            Debug.Log("Game loaded from: " + saveLocation);
        }
        else
        {
            Debug.Log("No save file found. Creating new save.");

            SaveGame();
        }
    }

    public void DeleteSave()
    {
        if (File.Exists(saveLocation))
        {
            File.Delete(saveLocation);
            Debug.Log("Save file deleted.");
        }
    }
}