using System.Collections.Generic;
using UnityEngine;

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
                //name = obj.name,
                description = obj.description,
                type = obj.type,

                puzzleID = obj.puzzleID,

                currentAmount = obj.currentAmount,
                requiredAmount = obj.requiredAmount,
                itemID = obj.itemID,
                countFrom0 = obj.countFrom0,

                npcID = obj.npcID,
                doneTalking = obj.doneTalking,
            });
        }

        ActivatePuzzles();
        Subscriptions();
    }

    public bool IsCompleted() => objectives.TrueForAll(o => IsObjectiveCompleted(o));

    private bool IsObjectiveCompleted(Objective obj)
    {
        if (obj.type == ObjectiveType.Puzzle || obj.type == ObjectiveType.ReachLocation)
        {
            return PuzzlesDictionary.Instance.GetPuzzleOB(obj.puzzleID) != null && PuzzlesDictionary.Instance.GetPuzzleOB(obj.puzzleID).IsCompleted;
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

    private void ActivatePuzzles()
    {
        Debug.Log("Activating");
        foreach (Objective objective in objectives)
        {
            if (objective.type == ObjectiveType.ReachLocation)
            {
                PuzzlesDictionary.Instance.GetPuzzleOB(objective.puzzleID).isActive = true;
                Debug.Log("puzzle activated");
            }
        }
    }

    private void Subscriptions()
    {
        InventoryController.Instance.OnItemAdded += OnItemAdded;
        InventoryController.Instance.OnItemRemoved += OnItemRemoved;
        DialogueController.Instance.OnFinishedDialogue += OnFinishedDialogue;
        PuzzlesController.Instance.OnPuzzleFinished += OnPuzzleFinished;
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

    private void OnPuzzleFinished(int puzzleID)
    {
        foreach (Objective objective in objectives)
        {
            if (objective.type == ObjectiveType.Puzzle || objective.type == ObjectiveType.ReachLocation)
            {
                if(objective.puzzleID == puzzleID)
                {
                    QuestController.Instance.HandInQuest(questID);
                    //return;        // ??
                }
            }
        }
    }
}