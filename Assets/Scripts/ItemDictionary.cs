using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public static ItemDictionary Instance { get; private set; }

    public List<Item> ItemPrefabs;
    private Dictionary<int, GameObject> itemDictionary;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        itemDictionary = new Dictionary<int, GameObject>();
        for (int i = 0; i < ItemPrefabs.Count; i++)
        {
            if (ItemPrefabs[i] != null)
            {
                ItemPrefabs[i].ID = i + 1;
            }
        }

        foreach (Item item in ItemPrefabs)
        {
            itemDictionary[item.ID] = item.gameObject;
        }
    }

    public GameObject GetItemPrefab(int itemId)
    {
        itemDictionary.TryGetValue(itemId, out GameObject prefab);
        if (prefab == null)
        {
            Debug.LogWarning($"item with ID {itemId} not found in dictionary");

        }
        return prefab;

    }

}
