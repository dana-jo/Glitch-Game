using UnityEngine;

public class ReachLocationObjective : ObjectiveBehaviour
{
    private void Start()
    {
        OnStart();
    }

    private void OnTriggerEnter2D(Collider2D x)
    {
        if (IsCompleted || !x.CompareTag("Player"))
            return;

        Complete();
        Debug.Log("done reach location");
    }

    public override void RestoreCompletedState()
    {
        base.RestoreCompletedState();

        // we should delete the object or something
    }
}