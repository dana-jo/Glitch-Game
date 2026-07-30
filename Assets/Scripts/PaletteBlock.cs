using UnityEngine;
public enum BlockType
{
    Up,
    Down,
    Left,
    Right,
    ForLoop
}
public class PaletteBlock : MonoBehaviour, Interactable
{
    public BlockType blockType;
    private CodeBlocksPuzzleController controller; // drag this in the Inspector
    private SpriteRenderer sr;

    void Awake()
    {
        controller = GetComponentInParent<CodeBlocksPuzzleController>();
        sr = GetComponent<SpriteRenderer>();
    }
    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        Debug.Log("Block interacted: " + blockType);
        controller.AddBlock(blockType, sr.sprite);
    }
}
