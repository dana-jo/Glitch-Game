using System;
using UnityEngine;

[Serializable]
public class Objective
{
    public string name;
    public string description;
    public ObjectiveType type;

    [Space(15)]
    [Header("For puzzles and locations")]
    public GameObject target;

    [Space(15)]
    [Header("For collecting items")]
    public int requiredAmount;
    [HideInInspector]
    public int currentAmount;
    public int itemID;
    public bool countFrom0;

    [Space(15)]
    [Header("For talk to npc")]
    public int npcID;
    [HideInInspector]
    public bool doneTalking;


    public ObjectiveBehaviour getBehaviour() {

        if (target == null)
        {
            Debug.LogWarning($"no gameobject attached to puzzle {name}");
        }

        ObjectiveBehaviour ob = target.GetComponentInChildren<ObjectiveBehaviour>();
        if (ob == null)
        {
            Debug.LogWarning($"objective behaviour not found in puzzle {name}");
        }

        return ob;
    }
}
public enum ObjectiveType { Puzzle, CollectItem, ReachLocation, TalkNPC }