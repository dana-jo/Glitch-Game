using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Objects/Quest")]
public class Quest : ScriptableObject
{
    public int id = -1;
    public string questName;
    [TextArea]
    public string description;
    public List<Objective> objectives;
    // public List<Reward> rewards;

    private QuestDictionary qd;

    private void Awake()
    {
        qd = QuestDictionary.Instance;
    }

    // called when csriptable object is edited
    private void OnValidate()
    {
        if (id < 0) {
            objectives = new List<Objective>();
            return;
        }

        if (qd.quests == null)
        {
            return;
        }

        if (id >= qd.quests.Count)
        {
            Debug.LogWarning($"Quest ID {id} not found");
            objectives = new List<Objective>();
            return;
        }

        List<GameObject> questObjects = qd.quests[id].objectives;
        if (questObjects == null) 
            return;

        Debug.Log(questObjects.Count);
        objectives = new List<Objective>();
        for (int i = 0; i < questObjects.Count; i++)
        {
            if(qd.getObjective(id, i) == null)
            {
                Debug.Log("didnt find objective behaviour");
                continue;
            }

            Objective newObj = new Objective
            {
                name = string.Empty,
                description = string.Empty,
                type = ObjectiveType.Puzzle,
                ob = qd.getObjective(id, i)
            };

            objectives.Add(newObj);
        }

        Debug.Log("Successfully added quests");
    }

    public virtual void ShowQuestPopup()
    {
        PopupManager.Instance.ShowPopup(
            questName,
            description
        );
    }

}
