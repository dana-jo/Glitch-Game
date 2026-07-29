using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragNDropPuzzle : ObjectiveBehaviour, Interactable
{
    //public string machineId { get; private set; }
    public GameObject canvas;

    private List<Transform> targetSlots = new();
    private List<Transform> puzzlePieces = new();
    private int attachedPieces = 0;

    void Start()
    {
        OnStart();

        canvas.SetActive(false);
        Debug.Log("from inside child");
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        if (!CanInteract())
            return;
        canvas.SetActive(!canvas.activeSelf);
        Debug.Log("inside interact");
    }


    public override void UpdatePuzzleState()
    {
        attachedPieces++;

        if (attachedPieces >= targetSlots.Count)
        {
            Complete();
            Debug.Log("Puzzle Finished");

            //SceneController.Instance.PlayCutscene("Intro"); // for testing

        }
    }

    public override void RestoreCompletedState()
    {
        base.RestoreCompletedState();

        Transform piecesParent = canvas.transform.Find("PuzzlePieces");

        foreach (Transform parent in piecesParent)
        {
            foreach (Transform piece in parent)
            {
                puzzlePieces.Add(piece);
                piece.GetComponent<DragHandler>().SetAttatched();

            }
        }
    }
}
