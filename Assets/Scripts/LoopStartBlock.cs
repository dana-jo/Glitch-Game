using TMPro;
using UnityEngine;

public class LoopStartBlock : MonoBehaviour, Interactable
{
    public BlockType blockType = BlockType.LoopStart;
    [SerializeField] private TextMeshPro countDisplay;
    private CodeBlocksPuzzleController controller;
    private SpriteRenderer sr;
    private int currentCount = 1;

    void Awake()
    {
        controller = GetComponentInParent<CodeBlocksPuzzleController>();
        sr = GetComponent<SpriteRenderer>();
        UpdateDisplay();
    }

    public void CycleCount()
    {
        Debug.Log("CycleCount called, current: " + currentCount);
        currentCount++;
        if (currentCount > 9)
            currentCount = 1;

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (countDisplay != null)
            countDisplay.text = currentCount.ToString();
    }

    public void Interact()
    {
        controller.AddBlock(blockType, sr.sprite, currentCount);
    }

    public bool CanInteract()
    {
        return !controller.IsLocked();
    }
}