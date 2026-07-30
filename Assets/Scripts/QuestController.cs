using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance { get; private set; }

    public List<QuestProgress> activeQuests = new();
    [HideInInspector]
    public List<int> handingQuestIDs = new();

    private QuestUIController questUI;
    private QuestDictionary questDictionary;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        questUI = FindFirstObjectByType<QuestUIController>();
        questDictionary = QuestDictionary.Instance;

        //InventoryController.Instance.OnInventoryChanged += CheckInventoryForQuests;
    }

    public void AcceptQuest(int questID)
    {
        if (IsQuestActive(questID))
            return;

        activeQuests.Add(new QuestProgress(questID));

        //CheckInventoryForQuests();
        InitiateQuestObjectives(questID);

        questUI.UpdateQuestUI();
    }

    public bool IsQuestActive(int questID) => activeQuests.Exists(q => q.questID == questID);

    public void InitiateQuestObjectives(int questID)
    {
        Dictionary<int, int> itemCounts = InventoryController.Instance.GetItemCounts();

        QuestProgress quest = activeQuests.FirstOrDefault(q => q.questID == questID);
        if(quest == null)
        {
            Debug.LogError("Quest is not found after being accepted. an error happened.");
            return;
        }

        foreach (Objective objective in quest.objectives)
        {
            if(objective.type == ObjectiveType.CollectItem && !objective.countFrom0)
            {
                int newAmount = itemCounts.TryGetValue(objective.itemID, out int count) ? Mathf.Min(count, objective.requiredAmount) : 0;
                objective.currentAmount = newAmount;
                Debug.Log($"Objective current amount initiated    {newAmount}");
            }
        }
    }
    public void CheckInventoryForQuests()
    {
        Dictionary<int, int> itemCounts = InventoryController.Instance.GetItemCounts();

        foreach (QuestProgress quest in activeQuests)
        {
            foreach (Objective questObjective in quest.objectives)
            {
                // update the objective state depending on the type
                // -------------------------------------------------------------------------------------- collect item
                //if (questObjective.type != ObjectiveType.CollectItem)
                //    continue;
                //if (!int.TryParse(questObjective.objectiveID, out int itemID))
                //    continue;

                //int newAmount = itemCounts.TryGetValue(itemID, out int count) ? Mathf.Min(count, questObjective.requiredAmount) : 0;

                //if (questObjective.currentAmount != newAmount)
                //{
                //    questObjective.currentAmount = newAmount;
                //}
            }
        }

        questUI.UpdateQuestUI();
    }

    public bool IsQuestCompleted(int questID)
    {
        QuestProgress quest = activeQuests.Find(q => q.questID == questID);
        return quest != null && quest.IsCompleted();
    }

    public void HandInQuest(int questID)
    {
        // -------------------------------------------------------------------------------------- collect item
        //if (!RemoveRequiredItemsFromInventory(questID))
        //{
        //    return; // thee quest is not completed => nothing happens
        //}

        QuestProgress quest = activeQuests.Find(q => q.questID == questID);
        if (quest != null)
        {
            handingQuestIDs.Add(questID);
            activeQuests.Remove(quest);
            questUI.UpdateQuestUI();
        }

    }

    public bool IsQuestHandedIn(int questID)
    {
        return handingQuestIDs.Contains(questID);
    }

    public bool RemoveRequiredItemsFromInventory(int questID)
    {
        QuestProgress quest = activeQuests.Find(q => q.questID == questID);
        if (quest == null)
            return false;

        Dictionary<int, int> requiredItems = new();
        foreach (Objective objective in quest.objectives)
        {
            //if (objective.type == ObjectiveType.CollectItem && int.TryParse(objective.objectiveID, out int itemID))
            //{
            //    requiredItems[itemID] = objective.requiredAmount;
            //}
        }

        // make sure we have the items
        Dictionary<int, int> itemCounts = InventoryController.Instance.GetItemCounts();
        foreach (var item in requiredItems)
        {
            if (itemCounts.GetValueOrDefault(item.Key) < item.Value)
            {
                return false; // means the quest is not completed yet
            }
        }

        // we have the items => remove them from the inventory
        foreach (var itemRequirement in requiredItems)
        {
            InventoryController.Instance.RemoveItemsFromInventory(itemRequirement.Key, itemRequirement.Value);
        }

        return true;
    }

    public void LoadQuestProgress(List<QuestProgress> savedQuests)
    {
        activeQuests = savedQuests ?? new();

        CheckInventoryForQuests();
        questUI.UpdateQuestUI();
    }
}
