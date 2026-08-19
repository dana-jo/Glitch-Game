using UnityEngine;
using System.Collections.Generic;

public class ChestController : MonoBehaviour
{
    public static ChestController Instance { get; private set; }

    void Awake()
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
    }

    public List<ChestSaveData> GetChestSaveData()
    {
        List<ChestSaveData> chestSaveDataList = new List<ChestSaveData>();

        foreach (var chest in FindObjectsOfType<Chest>())
        {
            if (chest != null)
            {
                chestSaveDataList.Add(new ChestSaveData
                {
                    chestID = chest.ChestID,
                    isOpened = chest.IsOpened
                });
            }
        }

        return chestSaveDataList;
    }
    public void SetChestStates(List<ChestSaveData> chestSaveDataList)
    {
        if (chestSaveDataList == null)
        return;

        Chest[] allChests = FindObjectsOfType<Chest>();

        foreach (var chest in allChests)
        {
            if (chest == null)
                continue;
            Debug.Log($"Setting state for chest with ID: {chest.ChestID}");
            var savedData = chestSaveDataList.Find(
                data => data.chestID == chest.ChestID
            );

            if (savedData != null)
            {
                Debug.Log($"Found saved data for chest with ID: {chest.ChestID}. IsOpened: {savedData.isOpened}");
                chest.LoadChestState(savedData.isOpened);

            }
        }
    }
}