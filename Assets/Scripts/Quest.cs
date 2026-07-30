using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class Quest
{
    //private int ID {  get; set; }           // i dont know if we need this
    public string questName;
    [TextArea]
    public string description;
    public List<Objective> objectives;
    // public List<Reward> rewards;
}

public class QuestProgress
{
    public Quest quest;
    public int questID;
    public List<Objective> objectives;


    public QuestProgress(int questId)
    {
        Quest quest = QuestDictionary.Instance.GetQuest(questId);

        this.quest = quest;
        questID = questId;
        objectives = new List<Objective>();

        foreach (var obj in quest.objectives)
        {
            objectives.Add(new Objective
            {
                name = obj.name,
                description = obj.description,
                type = obj.type,

                target = obj.target,

                currentAmount = obj.currentAmount,
                requiredAmount = obj.requiredAmount,
                itemID = obj.itemID,
                countFrom0 = obj.countFrom0,

                npcID = obj.npcID,
                doneTalking = obj.doneTalking,
            });
        }

        Subscriptions();
    }

    public bool IsCompleted() => objectives.TrueForAll(o => IsObjectiveCompleted(o));

    private bool IsObjectiveCompleted(Objective obj)
    {
        if (obj.type == ObjectiveType.Puzzle || obj.type == ObjectiveType.ReachLocation)
        {
            return obj.getBehaviour() != null && obj.getBehaviour().IsCompleted;
        }
        else if (obj.type == ObjectiveType.CollectItem)
        {
            return obj.currentAmount == obj.requiredAmount;
        }
        else
        {
            return obj.doneTalking;
        }
    }

    private void Subscriptions()
    {
        InventoryController.Instance.OnItemAdded += OnItemAdded;
        InventoryController.Instance.OnItemRemoved += OnItemRemoved;
        DialogueController.Instance.OnFinishedDialogue += OnFinishedDialogue;
    }

    private void OnItemAdded(int itemID, int amount)
    {
        foreach (Objective objective in objectives)
        {
            if (objective.type == ObjectiveType.CollectItem && objective.itemID == itemID)
            {
                if (objective.countFrom0)
                {
                    //objective.currentAmount = Mathf.Min(objective.currentAmount + amount - objective.previousAmount, objective.requiredAmount);
                    //objective.previousAmount += objective.currentAmount;
                }
                else
                    objective.currentAmount = Mathf.Min(objective.currentAmount + amount, objective.requiredAmount);

                Debug.Log($"item added catched {itemID}      -       current amount {objective.currentAmount}");
                QuestController.Instance.UpdateUI();
                //return;        // ??
            }
        }
    }

    private void OnItemRemoved(int itemID, int amount)
    {
        foreach (Objective objective in objectives)
        {
            if (objective.type == ObjectiveType.CollectItem && !objective.countFrom0 && objective.itemID == itemID)
            {
                objective.currentAmount = Mathf.Max(objective.currentAmount - amount, 0);
                Debug.Log($"item removed catched {itemID}      -       current amount {objective.currentAmount}");
                QuestController.Instance.UpdateUI();
                //return;        // ??
            }
        }
    }

    private void OnFinishedDialogue(int npcID)
    {
        foreach (Objective objective in objectives)
        {
            if (objective.type == ObjectiveType.TalkNPC && objective.npcID == npcID)
            {
                objective.doneTalking = true;
                Debug.Log("Done talking catched");
                //return;        // ??
            }
        }
    }
}