using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    public bool loop { get; private set; } // if loop = true the scene loops from the scene manager

    private string currentCutscene;
    private bool isPlayingCutscene;
    public GameObject cutsceneBg;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        if(cutsceneBg != null)
            cutsceneBg.SetActive(false);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        // needs more work, keeps items in inventory, notebook n stuff
        // maybe we can save and then load save file into another scene, we then need to save the name of the scene we're at
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