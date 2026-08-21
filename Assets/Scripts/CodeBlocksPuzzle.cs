using UnityEngine;

public class CodeBlocksPuzzle : ObjectiveBehaviour
{
    private CodeBlocksPuzzleController controller;

    void Start()
    {
        controller = GetComponentInParent<CodeBlocksPuzzleController>();
        OnStart();

        if (IsCompleted)
        {
            RestoreCompletedState();
        }

    }


    public override void UpdatePuzzleState()
    {
        if (IsCompleted)
            return;

        Complete();

        controller?.LockAsSolved();
    }

    public override void RestoreCompletedState()
    {
        base.RestoreCompletedState();

        controller?.LockAsSolved();
        // TODO: once save system provides the saved sprite list,
        // call: controller.RestoreVisualSequence(savedSprites);
    }
}
