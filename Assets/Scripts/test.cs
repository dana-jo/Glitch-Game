using UnityEngine;

public class test : MonoBehaviour
{
    // this is a class only for testing stuff like dictionaries
    QuestDictionary qd;
    void Start()
    {
    }

    private void Update()
    {
        QuestDictionary qd = QuestDictionary.Instance;
        Debug.Log(qd.GetQuest(0).objectives[0].IsComplete);
    }

}
