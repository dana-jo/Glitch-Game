using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    public bool loop { get; private set; }
    private string currentCutscene;
    private bool isPlayingCutscene;
    public GameObject cutsceneBg;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        cutsceneBg.SetActive(false);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void PlayCutscene(string sceneName, bool l = false)
    {
        Debug.Log("Play cutscene");

        if (isPlayingCutscene)
            return;
        
        currentCutscene = sceneName;
        isPlayingCutscene = true;
        loop = l;

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        cutsceneBg.SetActive(true);
    }

    public void EndCutscene()
    {
        Debug.Log("EndCutscene");

        if (!isPlayingCutscene)
            return;

        SceneManager.UnloadSceneAsync(currentCutscene);
        cutsceneBg.SetActive(false);

        currentCutscene = "";
        isPlayingCutscene = false;
        loop = false;
    }
}