using UnityEngine;

public class test : MonoBehaviour
{
    // this is a class only for testing stuff like dictionaries
    QuestDictionary qd;
    void Start()
    {
        qd = QuestDictionary.Instance;
        Debug.Log(qd.getObjective(0, 0).IsCompleted);
    }

    private void Update()
    {
        Debug.Log(qd.getObjective(0, 0).IsCompleted);
    }

}
