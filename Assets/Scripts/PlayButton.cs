using UnityEngine;

public class PlayButton : MonoBehaviour, Interactable
{
    private CodeBlocksPuzzleController controller;

    void Awake()
    {
        controller = GetComponentInParent<CodeBlocksPuzzleController>();
    }

    public void Interact()
    {
        controller.RunSequence();
    }

    public bool CanInteract()
    {
        return !controller.IsLocked();
    }
}