using UnityEngine;

public class test : MonoBehaviour
{
    // this is a class only for testing stuff like dictionaries
    QuestDictionary qd;
    SceneController sc;
    void Start()
    {
        qd = QuestDictionary.Instance;
        sc = SceneController.Instance;
    }

    private void Update()
    {
        Debug.Log("0" + qd.GetQuest(0).objectives[0].IsComplete);
        Debug.Log("1" + qd.GetQuest(0).objectives[1].IsComplete);
    }

}
