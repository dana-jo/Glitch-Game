using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzlesDictionary : MonoBehaviour
{
    public static PuzzlesDictionary Instance { get; private set; }
    public List<GameObject> puzzles;

    public Action<int> OnPuzzleFinished;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        for (int i = 0; i < puzzles.Count; i++)
        {
            if (puzzles[i] != null)
            {
                ObjectiveBehaviour ob = puzzles[i].GetComponentInChildren<ObjectiveBehaviour>();
                if (ob == null)
                {
                    Debug.LogWarning($"objective behaviour not found in puzzle {i}");
                }

                ob.puzzleID = i + 1;
            }
        }
    }

    public ObjectiveBehaviour GetPuzzleOB(int id) // OB = ObjectiveBehaviour
    {
        if (id < 1)
        {
            Debug.LogWarning($"no gameobject attached to puzzle {id}");
            return null;
        }

        ObjectiveBehaviour ob = puzzles[id - 1].GetComponentInChildren<ObjectiveBehaviour>();
        if (ob == null)
        {
            Debug.LogWarning($"objective behaviour not found in puzzle {id}");
            return null;
        }

        return ob;
    }

    public void FinishPuzzle(int puzzleID)
    {
        OnPuzzleFinished?.Invoke(puzzleID);
    }
}
