using System.Collections.Generic;
using UnityEngine;

public class CodeBlocksPuzzleController : MonoBehaviour
{
    private Transform sequenceArea;
    private SequenceAreaController sequenceAreaController;
    public GameObject sequenceVisualPrefab;
    private RobotController robotController;

    private List<SequenceStep> playerSequence = new List<SequenceStep>();
    private List<GameObject> spawnedVisuals = new List<GameObject>();
    private bool isRunning = false;

    void Awake()
    {
        sequenceAreaController = GetComponentInChildren<SequenceAreaController>();
        sequenceArea = sequenceAreaController.transform;
        robotController = GetComponentInChildren<RobotController>();
    }

    public void AddBlock(BlockType type, Sprite blockSprite, int repeatCount = 0)
    {
        if (isRunning)
        {
            Debug.Log("Robot is running - reset before editing.");
            return;
        }

        if (playerSequence.Count >= sequenceAreaController.slotCount)
        {
            Debug.Log("Sequence is full - reset before adding more.");
            return;
        }

        playerSequence.Add(new SequenceStep(type, repeatCount));
        Debug.Log("Sequence so far: " + string.Join(", ", playerSequence));

        Vector3 localPos = sequenceAreaController.GetSlotLocalPosition(playerSequence.Count - 1);
        Vector3 worldPos = sequenceArea.TransformPoint(localPos);

        GameObject visual = Instantiate(sequenceVisualPrefab, worldPos, Quaternion.identity, sequenceArea);
        visual.GetComponent<SpriteRenderer>().sprite = blockSprite;
        spawnedVisuals.Add(visual);
    }

    public void ResetSequence()
    {
        playerSequence.Clear();

        robotController.ResetTouched();

        foreach (GameObject visual in spawnedVisuals)
        {
            Destroy(visual);
        }
        spawnedVisuals.Clear();
        isRunning = false;

        Debug.Log("Sequence reset.");
    }

    public void RunSequence()
    {
        if (isRunning)
        {
            Debug.Log("Already running.");
            return;
        }

        if (playerSequence.Count == 0)
        {
            Debug.Log("Sequence is empty - add blocks first.");
            return;
        }

        isRunning = true;
        Debug.Log("Running sequence: " + string.Join(", ", playerSequence));

        robotController.ExecuteSequence(playerSequence, OnRobotFinished);

        // Robot execution will go here later
    }

    private void OnRobotFinished(bool success)
    {
        if (success)
        {
            Debug.Log("Puzzle solved!");
            // stays isRunning = true, locked, as you specified
        }
        else
        {
            Debug.Log("Puzzle failed - reset required.");
            // isRunning stays true until player hits Reset, also as you specified
        }
    }
}
