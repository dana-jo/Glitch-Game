using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questName;
    [TextArea]
    public string description;
    public List<Objective> objectives;
    // public List<Reward> rewards;
}
