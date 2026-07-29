using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
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
            });
        }
    }

    public bool IsCompleted => objectives.TrueForAll(o => o.IsComplete);
}