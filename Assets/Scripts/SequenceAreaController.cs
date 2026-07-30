using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SequenceAreaController : MonoBehaviour
{
    public int slotCount = 4;
    public float blockSize = 1f;
    public float spacing = 0.2f; // gap between blocks
    public float verticalPadding = 0.3f; // extra height around the blocks

    private SpriteRenderer sr;
    private BoxCollider2D solidCollider;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        solidCollider = GetComponent<BoxCollider2D>();
        ResizeArea();
    }

    void ResizeArea()
    {
        float width = (slotCount * blockSize) + ((slotCount + 1) * spacing);
        float height = blockSize + verticalPadding;
        
        sr.size = new Vector2(width, height);

        if (solidCollider != null)
        {
            solidCollider.size = new Vector2(width, height);
        }
    }

    // Returns the local position (relative to this object) for a given slot index
    public Vector3 GetSlotLocalPosition(int index)
    {
        float width = (slotCount * blockSize) + ((slotCount + 1) * spacing);
        float startX = -width / 2f + spacing + (blockSize / 2f);
        float x = startX + index * (blockSize + spacing);
        return new Vector3(x, 0, 0);
    }
}