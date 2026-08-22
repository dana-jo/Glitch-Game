using UnityEngine;

public class ShowSimpleUI : MonoBehaviour, Interactable
{

    public GameObject canvas;

    [Header("For cutscene")]
    public bool isCutsceneObject;
    public string cutsceneName;
    public bool repeate;
    private bool isSceneOn;

    void Start()
    {
        canvas.SetActive(false);

    }

    void Update()
    {

    }
    public bool CanInteract()
    {
        return true;
}

    public void Interact()
    {
        if (isCutsceneObject)
        {
            if (isSceneOn)
                SceneController.Instance.EndCutscene();
            else
                SceneController.Instance.PlayCutscene(cutsceneName, repeate);

            return;
        }

        canvas.SetActive(!canvas.activeSelf);
        PauseController.Instance.OpenUI(canvas.activeSelf);
    }
}

