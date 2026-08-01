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

    }

}
