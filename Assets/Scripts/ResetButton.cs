using UnityEngine;

public class ResetButton : MonoBehaviour, Interactable
{
    private CodeBlocksPuzzleController controller;

    void Awake()
    {
        controller = GetComponentInParent<CodeBlocksPuzzleController>();
    }
    public void Interact()
    {
        controller.ResetSequence();
    }

    public bool CanInteract()
    {
        return !controller.IsLocked();
    }
}