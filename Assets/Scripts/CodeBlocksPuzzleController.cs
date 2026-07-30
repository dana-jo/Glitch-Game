using System.Collections.Generic;
using UnityEngine;

public class CodeBlocksPuzzleController : MonoBehaviour
{
    private Transform sequenceArea;
    private SequenceAreaController sequenceAreaController;
    public GameObject sequenceVisualPrefab;

    private List<BlockType> playerSequence = new List<BlockType>();

    void Awake()
    {
        sequenceAreaController = GetComponentInChildren<SequenceAreaController>();
        sequenceArea = sequenceAreaController.transform;
    }

    public void AddBlock(BlockType type, Sprite blockSprite)
    {

        if (playerSequence.Count >= sequenceAreaController.slotCount)
        {
            Debug.Log("Sequence is full - reset before adding more.");
            return;
        }

        playerSequence.Add(type);
        Debug.Log("Sequence so far: " + string.Join(", ", playerSequence));

        Vector3 localPos = sequenceAreaController.GetSlotLocalPosition(playerSequence.Count - 1);
        Vector3 worldPos = sequenceArea.TransformPoint(localPos);

        GameObject visual = Instantiate(sequenceVisualPrefab, worldPos, Quaternion.identity, sequenceArea);
        visual.GetComponent<SpriteRenderer>().sprite = blockSprite;
    }
}
