using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzlesController : MonoBehaviour
{
    public static PuzzlesController Instance { get; private set; }
    public Action<int> OnPuzzleFinished;

    void Awake()
    {
        if (Instance == null)
        {    
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return; 
        }
    }

    public void FinishPuzzle(int puzzleID)
    {
        OnPuzzleFinished?.Invoke(puzzleID);
    }
     public List<PuzzleSaveData> GetPuzzleStates()
    {
        List<PuzzleSaveData> puzzleStates = new List<PuzzleSaveData>();

        for (int i = 0; i < PuzzlesDictionary.Instance.puzzles.Count; i++)
        {
            GameObject puzzle = PuzzlesDictionary.Instance.puzzles[i];

            if (puzzle == null)
            {
                Debug.LogWarning($"Puzzle {i + 1} is null");
                continue;
            }

            ObjectiveBehaviour ob = puzzle.GetComponentInChildren<ObjectiveBehaviour>();

            if (ob == null)
            {
                Debug.LogWarning($"ObjectiveBehaviour not found in puzzle {i + 1}");
                continue;
            }

            puzzleStates.Add(new PuzzleSaveData
            {
                puzzleID = ob.puzzleID,
                isCompleted = ob.IsCompleted
            });
        }

        return puzzleStates;
    }

    public void SetPuzzleStates(List<PuzzleSaveData> puzzleStates)
    {
        Debug.Log($"Setting puzzle states for {puzzleStates.Count} puzzles");
        if (puzzleStates == null)
            return;

        foreach (PuzzleSaveData puzzleState in puzzleStates)
        {
            Debug.Log($"Setting puzzle {puzzleState.puzzleID} isCompleted to {puzzleState.isCompleted}");
            ObjectiveBehaviour ob = PuzzlesDictionary.Instance.GetPuzzleOB(puzzleState.puzzleID);

            if (ob == null)
                continue;

            ob.IsCompleted = puzzleState.isCompleted;

            if (puzzleState.isCompleted)
            {
                ob.RestoreCompletedState();
            }

            Debug.Log($"Puzzle {puzzleState.puzzleID} " + $"isCompleted set to {puzzleState.isCompleted}");
        }
    }
}