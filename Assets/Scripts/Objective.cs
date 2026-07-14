using System;
using UnityEngine;

[Serializable]
public class Objective
{
    public string name;
    public string description;
    public ObjectiveType type;
    public ObjectiveBehaviour ob;

    public bool IsComplete => ob.IsCompleted;
}
public enum ObjectiveType { Puzzle, CollectItem, ReachLocation, TalkNPC }