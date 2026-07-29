using UnityEngine;

public class ReachLocationObjective : ObjectiveBehaviour
{
    private void Start()
    {
        OnStart();
    }

    private void OnTriggerEnter2D(Collider2D x)
    {
        if (IsCompleted)
            return;

        if (!x.CompareTag("Player"))
            return;

        Complete();
        Debug.Log("done reach location");
    }
}