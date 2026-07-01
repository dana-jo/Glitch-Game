using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneSceneLoader : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName = "Sarah'sScene";

    public void LoadGameplayScene()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }
}