using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    public static SaveController Instance { get; private set; }

    private string saveLocation;
    private string saveSettingsLocation;
    private InventoryController inventoryController;
    private PlayerMovement player;
    private BrightnessManager brightnessManager;
    private SoundEffectManager soundEffectManager;

    //IEnumerator Start()
    IEnumerator Start()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
            Destroy(gameObject);

        InitializeComponents();

        // wait one frame so InventoryController.Start() can finish first
         yield return null;

        //LoadGame(); // loading the game moved to quest controller
        LoadMainMenu();

        //Debug.Log(saveLocation);
    }

    private void InitializeComponents()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "gameProgressData.json");
        saveSettingsLocation = Path.Combine(Application.persistentDataPath, "settingsData.json");
        inventoryController = FindFirstObjectByType<InventoryController>();
        player = FindFirstObjectByType<PlayerMovement>();
        brightnessManager = FindFirstObjectByType<BrightnessManager>();
        soundEffectManager = FindFirstObjectByType<SoundEffectManager>();
    }

    // we can save game on quit by using function OnApplicationQuit()
    public void SaveGame()
    {
        SaveGameProgress();
        SaveSettings();
    }
    private void SaveGameProgress()
    {
        if (inventoryController == null)
            Debug.LogError("inventoryController is NULL");

        if (player == null)
            Debug.LogError("player is NULL");

        if (PuzzlesController.Instance == null)
            Debug.LogError("PuzzlesController is NULL");

        if (ChestController.Instance == null)
            Debug.LogError("ChestController is NULL");

        if (QuestController.Instance == null)
            Debug.LogError("QuestController is NULL");

        if (NotebookController.Instance == null)
            Debug.LogError("NotebookController is NULL");

        // inventroy 
        if (inventoryController == null)
        {
            Debug.LogError("Missing InventoryController");
            return;
        }

        SaveData saveData = new SaveData()
        {
            playerPosition = player.transform.position,
            isSceneDone = player.isSceneDone,
            inventorySaveData = inventoryController.GetInventoryItems(),
            hotbarSaveData = inventoryController.GetHotbarItems(),
            stateOfPuzzles = PuzzlesController.Instance.GetPuzzleStates(),
            chestSaveData = ChestController.Instance.GetChestSaveData(),
            activeQuestProgressData = QuestController.Instance.GetQuestSaveData(),
            handingQuestIDs = QuestController.Instance.handingQuestIDs,
            unlockedNotesIDs = NotebookController.Instance.unlockedNotesIDs
        };

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveLocation, json);

        Debug.Log("Game progress saved to: " + saveLocation);
    }
    public void SaveSettings()
    {
        List<float> soundSettings = soundEffectManager.GetSoundSettings();
        SettingsData settingsData = new SettingsData()
        {
            sfxVolume = soundSettings[0],
            musicVolume = soundSettings[1],
            isMuted = soundSettings[2] == 1f,
            brightness = brightnessManager.GetBrightness()
        };

        string json = JsonUtility.ToJson(settingsData, true);
        File.WriteAllText(saveSettingsLocation, json);

        Debug.Log("Settings saved to: " + saveSettingsLocation);
    }

    public void LoadGame()
    {
        LoadGameProgress();
        LoadSettings();
    }
    private void LoadGameProgress()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            player.transform.position = saveData.playerPosition; // SHOULD I EDIT THE CAMERA POS TOO?
            player.isSceneDone = saveData.isSceneDone;

            inventoryController.SetInventoryItems(saveData.inventorySaveData);
            inventoryController.SetHotbarItems(saveData.hotbarSaveData);

            PuzzlesController.Instance.SetPuzzleStates(saveData.stateOfPuzzles);
            ChestController.Instance.SetChestStates(saveData.chestSaveData);
            QuestController.Instance.LoadQuestSaveData(saveData.activeQuestProgressData);
            QuestController.Instance.handingQuestIDs = saveData.handingQuestIDs;

            NotebookController.Instance.LoadListOfNotes(saveData.unlockedNotesIDs);
            Debug.Log("Game loaded from: " + saveLocation);
        }
        else
        {
            Debug.Log("No save file found. Creating new save.");

            SaveGame();
        }
    }
    private void LoadSettings()
    {
        if (File.Exists(saveSettingsLocation))
        {
            SettingsData settingsData = JsonUtility.FromJson<SettingsData>(File.ReadAllText(saveSettingsLocation));

            soundEffectManager.LoadSoundSettings(settingsData.sfxVolume, settingsData.musicVolume, settingsData.isMuted);
            brightnessManager.LoadBrightnessSettings(settingsData.brightness);

            Debug.Log("Settings loaded from: " + saveSettingsLocation);
        }
        else
        {
            Debug.Log("No settings file found. Creating new settings.");

            SaveSettings();
        }
    }

    public void DeleteSave()
    {
        Debug.Log("Deleting file");
        if (File.Exists(saveLocation))
        {
            File.Delete(saveLocation);
            Debug.Log("Save file deleted.");
        }
    }

    public void LoadMainMenu()
    {
        LoadSettings();

        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(
                File.ReadAllText(saveLocation)
            );

            NotebookController.Instance.LoadListOfNotes(
                saveData.unlockedNotesIDs
            );
        }
    }

    public bool HasSaveData()
    {
        if (!File.Exists(saveLocation))
            return false;

        string json = File.ReadAllText(saveLocation);

        if (string.IsNullOrWhiteSpace(json))
            return false;

        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        return saveData != null;
    }
}