using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PaletteAreaController : MonoBehaviour
{
    public float blockSize = 1f;
    public float spacing = 0.2f;
    public float verticalPadding = 0.3f;

    private SpriteRenderer sr;
    private BoxCollider2D solidCollider;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        solidCollider = GetComponent<BoxCollider2D>();
        ArrangeAndResize();
    }

    void ArrangeAndResize()
    {
        PaletteBlock[] blocks = GetComponentsInChildren<PaletteBlock>();
        int count = blocks.Length;

        float width = (count * blockSize) + ((count + 1) * spacing);
        float height = blockSize + verticalPadding;
        sr.size = new Vector2(width, height);

        if (solidCollider != null)
        {
            solidCollider.size = new Vector2(width, height);
        }

        float startX = -width / 2f + spacing + (blockSize / 2f);

        for (int i = 0; i < count; i++)
        {
            float x = startX + i * (blockSize + spacing);
            blocks[i].transform.localPosition = new Vector3(x, 0, 0);
        }
    }
}