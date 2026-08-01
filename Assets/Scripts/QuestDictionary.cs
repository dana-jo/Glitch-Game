using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class QuestDictionary : MonoBehaviour
{
    public static QuestDictionary Instance { get; private set; }
    public List<Quest> quests;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i] != null)
            {
                quests[i].ID = i + 1;
            }
        }
    }

    public Quest GetQuest(int id)
    {
        if(id < 1)
            return null;
        return quests[id - 1];
    }

    public int GetQuestID(Quest quest)
    {
        return quests.IndexOf(quest) + 1;
    }
}
