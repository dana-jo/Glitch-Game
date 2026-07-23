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
    }

    public Quest GetQuest(int id)
    {
        return quests[id];
    }
    
}
