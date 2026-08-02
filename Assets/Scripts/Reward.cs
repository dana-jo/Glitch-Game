using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Reward
{
    public RewardType rewardType;

    [Space(15)]
    [Header("Note")]
    public int noteID;

    [Space(15)]
    [Header("Quest")]
    public int questID;

    [Space(15)]
    [Header("Item")]
    public int itemID;
    public int amount;

    [Space(15)]
    [Header("Cutscene")]
    public string sceneName;

    [Space(15)]
    [Header("Dialogue")]
    public Dialogue dialogue;
    public int npcID;

}

public enum RewardType { Note, Cutscene, Item, Map, Quest, Dialogue }