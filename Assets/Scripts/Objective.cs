using System;
using UnityEngine;

[Serializable]
public class Objective
{
    public string name;
    public string description;
    public ObjectiveType type;
    public GameObject target;

    private ObjectiveBehaviour getBehaviour() { 

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

    public bool IsComplete =>
        getBehaviour() != null && getBehaviour().IsCompleted;
}
public enum ObjectiveType { Puzzle, CollectItem, ReachLocation, TalkNPC }