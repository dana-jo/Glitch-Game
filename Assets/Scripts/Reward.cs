using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Reward
{
    public RewardType rewardType;

    [Space(15)]
    [Header("Note - Item - Quest")]
    public int ID;

    [Space(15)]
    [Header("Item")]
    public int amount;

    [Space(15)]
    [Header("Cutscene")]
    public string sceneName;

}

public enum RewardType { Note, Cutscene, Item, Map, Quest }