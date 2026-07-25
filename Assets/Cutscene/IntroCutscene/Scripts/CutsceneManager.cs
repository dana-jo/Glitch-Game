using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    private PlayableDirector director;
    private SceneController sc;

    private void Awake()
    {
        sc = SceneController.Instance;

        director = GetComponent<PlayableDirector>();
        if (director != null)
            Debug.Log("we got the director");
    }

    public void Finish()
    {
        Debug.Log("finish from cutscene");

        if (sc.loop)
        {
            director.time = 0;
            director.Evaluate();
            director.Play();
        }
        else
        {
            sc.EndCutscene();
        }
    }
}