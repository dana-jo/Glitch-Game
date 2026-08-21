using System.Collections.Generic;
using UnityEngine;

public class CodeBlocksPuzzleController : MonoBehaviour
{
    private Transform sequenceArea;
    private SequenceAreaController sequenceAreaController;
    public GameObject sequenceVisualPrefab;
    private RobotController robotController;
    private CodeBlocksPuzzle objective;

    private List<SequenceStep> playerSequence = new List<SequenceStep>();
    private List<GameObject> spawnedVisuals = new List<GameObject>();
    private bool isRunning = false;
    private bool isSolved = false;

    void Awake()
    {
        sequenceAreaController = GetComponentInChildren<SequenceAreaController>();
        sequenceArea = sequenceAreaController.transform;
        robotController = GetComponentInChildren<RobotController>();
        objective = GetComponentInChildren<CodeBlocksPuzzle>();
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
        visual.GetComponent<SpriteRenderer>().sortingLayerName = "Decor";
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
            isSolved = true;
            objective?.UpdatePuzzleState();
            // stays isRunning = true, locked, as you specified
        }
        else
        {
            Debug.Log("Puzzle failed - reset required.");
            isRunning = false; // unlock so the player can Reset or rebuild
        }
    }

    private Sprite GetSpriteForType(BlockType type)
    {
        PaletteBlock[] blocks = GetComponentsInChildren<PaletteBlock>(true);
        foreach (PaletteBlock block in blocks)
        {
            if (block.blockType == type)
                return block.GetComponent<SpriteRenderer>().sprite;
        }

        // Check LoopStart separately, since it's a different script
        LoopStartBlock[] loopBlocks = GetComponentsInChildren<LoopStartBlock>(true);
        foreach (LoopStartBlock block in loopBlocks)
        {
            if (block.blockType == type)
                return block.GetComponent<SpriteRenderer>().sprite;
        }

        Debug.LogWarning("No palette block found for type: " + type);
        return null;
    }

    public void RestoreVisualSequence(List<SequenceStep> steps)
    {
        if(steps == null || steps.Count == 0)
        {
            Debug.Log("No sequence to restore.");
            return;
        }
        for (int i = 0; i < steps.Count; i++)
        {
            Vector3 localPos = sequenceAreaController.GetSlotLocalPosition(i);
            Vector3 worldPos = sequenceArea.TransformPoint(localPos);

            GameObject visual = Instantiate(sequenceVisualPrefab, worldPos, Quaternion.identity, sequenceArea);
            visual.GetComponent<SpriteRenderer>().sprite = GetSpriteForType(steps[i].type);
            spawnedVisuals.Add(visual);
        }
        objective?.UpdatePuzzleState();
    }

    public List<SequenceStep> GetSolvedSequence()
    {
        if (isSolved)
        {
            return playerSequence;
        }
        else
        {
            Debug.LogWarning("Puzzle not solved yet - returning empty sequence.");
            return new List<SequenceStep>();
        }
    }

    public bool IsLocked()
    {
        return isRunning || isSolved;
    }

    public void LockAsSolved()
    {
        isSolved = true;
        isRunning = false;
    }
}
