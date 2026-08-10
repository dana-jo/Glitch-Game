using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{

    public float moveDistance = 1f;
    public float moveDuration = 0.3f;

    private List<PuzzleTarget> allTargets;

    private Vector3 startPosition;

    void Awake()
    {
        startPosition = transform.position;
        CodeBlocksPuzzleController puzzleController = GetComponentInParent<CodeBlocksPuzzleController>();
        allTargets = new List<PuzzleTarget>(puzzleController.GetComponentsInChildren<PuzzleTarget>());
    }
    public void ExecuteSequence(List<SequenceStep> sequence, System.Action<bool> onFinished)
    {
        StartCoroutine(RunSequenceCoroutine(sequence, onFinished));
    }

    private IEnumerator RunSequenceCoroutine(List<SequenceStep> sequence, System.Action<bool> onFinished)
    {
        int i = 0;

        while (i < sequence.Count)
        {
            SequenceStep step = sequence[i];

            if (step.type == BlockType.LoopStart)
            {
                int loopEndIndex = FindLoopEnd(sequence, i);

                for (int repeat = 0; repeat < step.repeatCount; repeat++)
                {
                    for (int j = i + 1; j < loopEndIndex; j++)
                    {
                        yield return StartCoroutine(MoveOneStep(GetDirection(sequence[j].type)));
                    }
                }

                i = loopEndIndex + 1; // continue after LoopEnd
            }
            else
            {
                yield return StartCoroutine(MoveOneStep(GetDirection(step.type)));
                i++;
            }
        }

        bool success = CheckSuccess();
        Debug.Log("Robot finished sequence. Success: " + success);

        yield return new WaitForSeconds(1f);

        if (!success)
        {
            transform.position = startPosition;
        }

        onFinished?.Invoke(success);
    }

    private int FindLoopEnd(List<SequenceStep> sequence, int loopStartIndex)
    {
        for (int k = loopStartIndex + 1; k < sequence.Count; k++)
        {
            if (sequence[k].type == BlockType.LoopEnd)
                return k;
        }
        return sequence.Count; // no LoopEnd found - treat rest of sequence as the loop body
    }

    private Vector3 GetDirection(BlockType type)
    {
        switch (type)
        {
            case BlockType.Up: return Vector3.up;
            case BlockType.Down: return Vector3.down;
            case BlockType.Left: return Vector3.left;
            case BlockType.Right: return Vector3.right;
            default: return Vector3.zero;
        }
    }

    private IEnumerator MoveOneStep(Vector3 direction)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + direction * moveDistance;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PuzzleTarget target))
        {
            target.MarkTouched();
        }
    }

    private bool CheckSuccess()
    {
        foreach (PuzzleTarget target in allTargets)
        {
            if (!target.IsTouched())
                return false;
        }
        return true;
    }

    public void ResetTouched()
    {
        foreach (PuzzleTarget target in allTargets)
        {
            target.ResetTouched();
        }
    }
}