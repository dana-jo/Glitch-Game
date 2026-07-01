using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventoryController : MonoBehaviour
{
    [Header("general")]
    private ItemDictionary itemDictionary;
    public GameObject inventoryPanel;
    public GameObject hotbarPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public int hotbarSlotCount;
    private Key[] hotbarKeys;
    public GameObject[] itemPrefabs;

    [Header("item details UI")]
    public Image itemPreviewImage;
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;

    public static InventoryController Instance { get; private set; }
    Dictionary<int, int> itemsCountCache = new();
    public event Action OnInventoryChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        hotbarKeys = new Key[hotbarSlotCount];
        for (int i = 0; i < hotbarSlotCount; i++)
        {
            hotbarKeys[i] = Key.Digit1 + i;
        }
    }
    void Start()
    {
        itemDictionary = FindAnyObjectByType<ItemDictionary>();
        for (int i=0;i<slotCount-hotbarSlotCount;i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;

            }
        }

        for (int i = 0; i < hotbarSlotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, hotbarPanel.transform).GetComponent<Slot>();
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;

            }
        }
        RebuildItemCounts();
        ClearItemDetails();



    }

    void Update()
    {
        if (Keyboard.current != null)
        {
            for (int i = 0; i < hotbarSlotCount; i++)
            {
                if (Keyboard.current[hotbarKeys[i]].wasPressedThisFrame)
                {
                    UseItemInHotbarSlot(i);
                }
            }
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!IsPointerOverItem())
            {
                ClearItemDetails();
            }
        }
    }

    void UseItemInHotbarSlot(int index)
    {
        if (hotbarPanel == null) return;
        if (index >= hotbarPanel.transform.childCount) return;

        Slot slot = hotbarPanel.transform.GetChild(index).GetComponent<Slot>();
        if (slot == null) return;

        // no item in this hotbar slot
        if (slot.currentItem == null) return;

        // fixes error after dragging item out of hotbar
        if (slot.currentItem.transform.parent != slot.transform)
        {
            slot.currentItem = null;
            return;
        }

        Item item = slot.currentItem.GetComponent<Item>();
        if (item == null) return;

        item.UseItem();
    }

    //modified Rebuild Item Counts-------------------------------------
    public void RebuildItemCounts()
    {
        itemsCountCache.Clear();

        CountItemsInPanel(inventoryPanel);
        CountItemsInPanel(hotbarPanel);

        OnInventoryChanged?.Invoke();
    }

    void CountItemsInPanel(GameObject panel)
    {
        if (panel == null) return;

        foreach (Transform slotTransform in panel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot != null && slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();

                if (item != null)
                {
                    itemsCountCache[item.ID] =
                        itemsCountCache.GetValueOrDefault(item.ID, 0) + item.quantity;
                }
            }
        }
    }


    public Dictionary<int, int> GetItemCounts() => itemsCountCache;

    //new Add Item:-------------------------------------
    public bool AddItem(GameObject itemPrefab)
    {
        Item itemToAdd = itemPrefab.GetComponent<Item>();
        if (itemToAdd == null) return false;

        // 1.try stacking in normal inventory
        if (TryStackItemInPanel(inventoryPanel, itemToAdd))
        {
            RebuildItemCounts();
            return true;
        }

        // 2. try stacking in hotbar
        if (TryStackItemInPanel(hotbarPanel, itemToAdd))
        {
            RebuildItemCounts();
            return true;
        }

        // 3. If no matching stack exists, put the new item only in normal inventory
        if (TryPlaceNewItemInPanel(inventoryPanel, itemPrefab))
        {
            RebuildItemCounts();
            return true;
        }

        // 4. if inventory is full, use empty hotbar slot as backup
        if (TryPlaceNewItemInPanel(hotbarPanel, itemPrefab))
        {
            RebuildItemCounts();
            return true;
        }

        Debug.Log("Inventory and hotbar is full");
        return false;
    }

    private bool TryStackItemInPanel(GameObject panel, Item itemToAdd)
    {
        if (panel == null) return false;

        foreach (Transform slotTransform in panel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot != null && slot.currentItem != null)
            {
                Item slotItem = slot.currentItem.GetComponent<Item>();

                if (slotItem != null && slotItem.ID == itemToAdd.ID)
                {
                    slotItem.AddToStack();
                    return true;
                }
            }
        }

        return false;
    }

    private bool TryPlaceNewItemInPanel(GameObject panel, GameObject itemPrefab)
    {
        if (panel == null) return false;

        foreach (Transform slotTransform in panel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;

                return true;
            }
        }

        return false;
    }

    //new get set ---------------------------------------
    public List<InventorySaveData> GetInventoryItems()
    {
        return GetItemsFromPanel(inventoryPanel);
    }

    public List<InventorySaveData> GetHotbarItems()
    {
        return GetItemsFromPanel(hotbarPanel);
    }

    private List<InventorySaveData> GetItemsFromPanel(GameObject panel)
    {
        List<InventorySaveData> dataList = new List<InventorySaveData>();

        foreach (Transform slotTransform in panel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot != null && slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();

                if (item != null)
                {
                    dataList.Add(new InventorySaveData
                    {
                        itemID = item.ID,
                        slotIndex = slotTransform.GetSiblingIndex(),
                        quantity = item.quantity
                    });
                }
            }
        }

        return dataList;
    }

    public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
    {
        int inventorySlotCount = slotCount - hotbarSlotCount;

        SetItemsToPanel(inventorySaveData, inventoryPanel, inventorySlotCount);
    }

    public void SetHotbarItems(List<InventorySaveData> hotbarSaveData)
    {
        SetItemsToPanel(hotbarSaveData, hotbarPanel, hotbarSlotCount);
    }

    private void SetItemsToPanel(List<InventorySaveData> saveData, GameObject panel, int panelSlotCount)
    {
        if (panel == null) return;
        while (panel.transform.childCount < panelSlotCount)
        {
            Instantiate(slotPrefab, panel.transform);
        }

        for (int i = 0; i < panelSlotCount; i++)
        {
            Slot slot = panel.transform.GetChild(i).GetComponent<Slot>();

            if (slot != null && slot.currentItem != null)
            {
                Destroy(slot.currentItem);
                slot.currentItem = null;
            }
        }
        foreach (InventorySaveData data in saveData)
        {
            if (data.slotIndex < panelSlotCount)
            {
                Slot slot = panel.transform.GetChild(data.slotIndex).GetComponent<Slot>();

                if (slot == null) continue;

                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);

                if (itemPrefab == null)
                {
                    Debug.LogWarning("No prefab found for item ID: " + data.itemID);
                    continue;
                }

                GameObject item = Instantiate(itemPrefab, slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                Item itemComponent = item.GetComponent<Item>();

                if (itemComponent != null)
                {
                    itemComponent.quantity = data.quantity;
                    itemComponent.UpdateQuantityDisplay();
                }

                slot.currentItem = item;
            }
        }

        RebuildItemCounts();
    }

    //new remove mthod ---------------------------------------------------------
    public void RemoveItemsFromInventory(int itemID, int amountToRemove)
    {
        RemoveItemsFromPanel(inventoryPanel, itemID, ref amountToRemove);
        RemoveItemsFromPanel(hotbarPanel, itemID, ref amountToRemove);

        RebuildItemCounts();
    }

    private void RemoveItemsFromPanel(GameObject panel, int itemID, ref int amountToRemove)
    {
        //removes from both hotbar and inv panels
        if (panel == null) return;

        foreach (Transform slotTransform in panel.transform)
        {
            if (amountToRemove <= 0) break;

            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot?.currentItem?.GetComponent<Item>() is Item item && item.ID == itemID)
            {
                int removed = Mathf.Min(amountToRemove, item.quantity);

                item.RemoveFromStack(removed);
                amountToRemove -= removed;

                if (item.quantity == 0)
                {
                    Destroy(slot.currentItem);
                    slot.currentItem = null;
                }
            }
        }
    }



    public void SelectItem(GameObject itemObject)
    {
        if (itemObject == null) return;

        Item item = itemObject.GetComponent<Item>();
        if (item == null) return;

        if (itemPreviewImage != null)
        {
            if (item.itemIcon != null)
            {
                itemPreviewImage.sprite = item.itemIcon;
            }
            else
            {
                Image itemImage = itemObject.GetComponent<Image>();

                if (itemImage != null)
                {
                    itemPreviewImage.sprite = itemImage.sprite;
                }
            }

            itemPreviewImage.enabled = true;
        }

        if (itemNameText != null)
        {
            itemNameText.text = item.Name;
        }

        if (itemDescriptionText != null)
        {
            itemDescriptionText.text = item.itemDescription;
        }
    }

    public void ClearItemDetails()
    {
        if (itemPreviewImage != null)
        {
            itemPreviewImage.sprite = null;
            itemPreviewImage.enabled = false;
        }

        if (itemNameText != null)
        {
            itemNameText.text = "";
        }

        if (itemDescriptionText != null)
        {
            itemDescriptionText.text = "";
        }
    }

    private bool IsPointerOverItem()
    {
        if (EventSystem.current == null) return false;
        if (Mouse.current == null) return false;

        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Mouse.current.position.ReadValue();

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponentInParent<ItemDragHandler>() != null)
            {
                return true;
            }
        }

        return false;
    }

}
