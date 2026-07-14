using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class QuestDictionary : MonoBehaviour
{
    public static QuestDictionary Instance { get; private set; }
    public List<QuestObjectives> quests;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public ObjectiveBehaviour getObjective(int questID, int objID)
    {
        GameObject go = quests[questID].objectives[objID];

        if (go == null)
        {
            Debug.LogWarning($"objective with id {questID},{objID} not found in dictionary");
        }

        ObjectiveBehaviour ob = go.GetComponentInChildren<ObjectiveBehaviour>();
        if (ob == null)
        {
            Debug.LogWarning($"objective behaviour not found");
        }

        return ob;
    }
}

[System.Serializable]
public class QuestObjectives
{
    // this is a helper so the unity editor shows the list<list<>> field
    public List<GameObject> objectives;
}
