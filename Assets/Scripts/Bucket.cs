using UnityEngine;
using UnityEngine.UI;
public class Bucket : Item
{
    public string currentLiquid = "";
    public Sprite emptySprite;
    public Sprite waterSprite;
    public Sprite mudSprite;

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


        if (liquid == "Water")
        {
            spriteRenderer.sprite = waterSprite;
            img.sprite = waterSprite;
        }
        else if(liquid == "Mud")
        {
            spriteRenderer.sprite = mudSprite;
            img.sprite = mudSprite;
        }
    }

    public void EmptyBucket()
    {
        currentLiquid = "";

        spriteRenderer.sprite = emptySprite;
        img.sprite = emptySprite;
    }

    // public override void UseItem()
    // {
    //     Debug.Log("Using Bucket");
    // }
}
