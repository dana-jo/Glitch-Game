using UnityEngine;

public class OpenCutscene : MonoBehaviour,Interactable

{
    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        SceneController.Instance.ChangeScene("Intro");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
