using System;
using UnityEngine;

public abstract class ObjectiveBehaviour : MonoBehaviour
{
    public int puzzleID;
    public bool isActive = false;
    public bool IsCompleted { get; private set; }

    public void OnStart()
    {
        Debug.Log("from abstract");
        if (IsCompleted)
            RestoreCompletedState();
    }
    public void Complete()
    {
        Debug.Log("Completed from abstract");
        IsCompleted = true;

        PuzzlesDictionary.Instance.FinishPuzzle(puzzleID);
    }

    public virtual void UpdatePuzzleState() { }

    public virtual void RestoreCompletedState()
    {
        // "virtual" means you can override the function
        if (IsCompleted)
            return;
    }

    // ---> on Start always call OnStart() first, like this :
    //void Start()
    //{
    //    OnStart();

    //    /* our code */
    //}

    // ---> this is how to implement the other functions :
    //public override void UpdatePuzzleState()
    //{
    //    /* our code */
    //}

    //public override void RestoreCompletedState()
    //{
    //    base.RestoreCompletedState();

    //    /* our code */
    //}
}