using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest")]
public class Quest : ScriptableObject
{
    public int ID;     // i dont know if we need this
    public string questName;
    [TextArea]
    public string description;
    public List<Objective> objectives;
    public List<Reward> rewards;
}