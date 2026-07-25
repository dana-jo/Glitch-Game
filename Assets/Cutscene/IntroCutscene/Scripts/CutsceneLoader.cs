using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneSceneLoader : MonoBehaviour
{
    [SerializeField]
    GameObject timeLine; // attatch the object that has PlayableDirector (which is usually the timeline
    private PlayableDirector director;

    private void Awake()
    {
        director = timeLine.GetComponent<PlayableDirector>();
        if (director != null)
            Debug.Log("we got the director");
    }

    public void Finish()
    {
        Debug.Log("finish from cutscene");

        if (SceneController.Instance.loop)
        {
            director.time = 0;
            director.Evaluate();
            director.Play();
        }
        else
        {
            SceneController.Instance.EndCutscene();
        }
    }
}