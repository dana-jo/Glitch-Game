using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

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
            director.time = GetLoopStartTime();
            director.Evaluate();
            director.Play();
        }
        else
        {
            sc.EndCutscene();
        }
    }

    private double GetLoopStartTime()
    {
        TimelineAsset timeline = director.playableAsset as TimelineAsset;

        if (timeline == null)
            return 0;

        foreach (var track in timeline.GetRootTracks())
        {
            foreach (var marker in track.GetMarkers())
            {
                if (marker is LoopStartMarker)
                    return marker.time;
            }
        }

        return 0;
    }
}