using UnityEngine;
using UnityEngine.UI;
public class Bucket : Item
{
    [SerializeField] public string currentLiquid = "";
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite waterSprite;
    [SerializeField] private Sprite mudSprite;

    private SpriteRenderer spriteRenderer;
    private Image img ;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        img = GetComponent<Image>();
    }

    public bool IsEmpty()
    {
        return currentLiquid == "";
    }

    public void Fill(string liquid)
    {
        currentLiquid = liquid;
        updateSprite();
    }

    public void EmptyBucket()
    {
        currentLiquid = "";
        updateSprite();
    }
    private void updateSprite()
    {
        if (currentLiquid == "Water")
        {
            spriteRenderer.sprite = waterSprite;
            img.sprite = waterSprite;
        }
        else if(currentLiquid == "Mud")
        {
            spriteRenderer.sprite = mudSprite;
            img.sprite = mudSprite;
        }
        else
        {
            spriteRenderer.sprite = emptySprite;
            img.sprite = emptySprite;
        }
    }

    // public override void UseItem()
    // {
    //     Debug.Log("Using Bucket");
    // }
}
