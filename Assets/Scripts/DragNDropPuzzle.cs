using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragNDropPuzzle : MonoBehaviour, Interactable
{
    //public string machineId { get; private set; }
    public GameObject canvas;
    public bool isFinished = true;

    private List<Transform> targetSlots = new();
    private List<Transform> puzzlePieces = new();
    private int attachedPieces = 0;

    void Start()
    {
        //machineId ??= GlobalHelper.GenerateUniqueId(gameObject);
        canvas.SetActive(false);

        if( isFinished )
            FinishedPuzzleState();

        // to access pieces and targets
        //Transform slotsParent = canvas.transform.Find("PuzzleSlots");
        //Transform piecesParent = canvas.transform.Find("PuzzlePieces");

        //foreach (Transform child in slotsParent)
        //{
        //    targetSlots.Add(child);
        //}

        //foreach (Transform parent in piecesParent)
        //{
        //    foreach (Transform piece in parent)
        //    {
        //        Debug.Log("piece added");
        //        puzzlePieces.Add(piece);

        //        if (isFinished)
        //        {
        //            Debug.Log("inside if");
        //            piece.GetComponent<DragHandler>().SetAttatched();
        //        }
                    
        //    }
        //}
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


    public void UpdatePuzzleState()
    {
        attachedPieces++;

        if (attachedPieces >= targetSlots.Count)
        {
            isFinished = true;
            Debug.Log("Puzzle Finished");
        }
    }

    public void FinishedPuzzleState()
    {
        Transform piecesParent = canvas.transform.Find("PuzzlePieces");

        foreach (Transform parent in piecesParent)
        {
            foreach (Transform piece in parent)
            {
                puzzlePieces.Add(piece);

                if (isFinished)
                {
                    piece.GetComponent<DragHandler>().SetAttatched();
                }

            }
        }
    }
}
