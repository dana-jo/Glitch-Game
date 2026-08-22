using System.Collections;
using UnityEngine;

public class StartGameSave : MonoBehaviour
{
    public PlayerMovement pm;
    IEnumerator Start()
    {
        yield return null;

        SaveController.Instance.LoadGame();

        yield return null;

        if (!pm.isSceneDone)
        {
            SceneController.Instance.PlayCutscene("Intro");
            pm.isSceneDone = true;
        }
    }
}
