using System;
using UnityEngine;

[Serializable]
public class Objective
{
    //public string name;
    public string description;
    public ObjectiveType type;

    [Space(15)]
    [Header("For puzzles and locations")]
    public int puzzleID;

    [Space(15)]
    [Header("For collecting items")]
    public int requiredAmount;
    [HideInInspector]
    public int currentAmount, previousAmount;
    public int itemID;
    public bool removeItemsAfterFinish;
    public bool countFrom0;

    [Space(15)]
    [Header("For talk to npc")]
    public int npcID;
    [HideInInspector]
    public bool doneTalking;
}
public enum ObjectiveType { Puzzle, CollectItem, ReachLocation, TalkNPC }