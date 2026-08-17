using UnityEngine;
using UnityEngine.UI;

public class Cauldron : MonoBehaviour , Interactable
{
    [SerializeField] public string Liquid; 
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite waterSprite;
    [SerializeField] private Sprite mudSprite;
    private float waterWeight = 5.0f , mudWeight = 25.0f , emptyCauldronWeight = 1.0f;
    private SpriteRenderer spriteRenderer;
    private Draggable draggable;
    public bool canInteract = true;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        draggable = GetComponent<Draggable>();

        if(Liquid == "")
        {
            emptyCauldron();
        }
        else
        {
            fillCauldron(Liquid);
        }
    }

    public void Interact()
    {
        Item current = InventoryController.Instance.CurrentItemInUse;

        if (current == null || !CanInteract())//  no interaction if the cauldron is being dragged
            return;

        Bucket bucket = current as Bucket;

        if (bucket == null)
            return;

        if (bucket.IsEmpty())
        {
            if(Liquid == "")
            {                
                Debug.Log($"cauldron is empty too.");
                return;
            }
            
            bucket.Fill(Liquid);
            Debug.Log($"Bucket filled with {Liquid}.");
            
            emptyCauldron();
            Debug.Log($"cauldron is empty.");
        }
        else
        {
            // bucket is not empty
            if(Liquid == "")
            {
                fillCauldron(bucket.currentLiquid);
                bucket.EmptyBucket();
                Debug.Log($"Bucket is empty now. cauldron has {Liquid}.");
            }
            else
            {                
                Debug.Log($"Bucket already contains {bucket.currentLiquid}, empty it first ");
            }
        }
    }

    public bool CanInteract()
    {
        return canInteract && ( draggable == null || (draggable != null && !draggable.IsHeld));
    }
    private void emptyCauldron()
    {
        Liquid = "";
        spriteRenderer.sprite = emptySprite;
    }
    public void fillCauldron(string liquid)
    {
        Liquid = liquid;
        if(liquid == "Water")
        {
            spriteRenderer.sprite = waterSprite;           
        }
        else if(liquid == "Mud")
        {
            spriteRenderer.sprite = mudSprite;
        }
    }
    public float GetWeight()
    {
        if(Liquid == "Water")
        {
            return waterWeight + emptyCauldronWeight;
        }
        else if(Liquid == "Mud")
        {
            return mudWeight + emptyCauldronWeight;
        }
        else
        {
            return emptyCauldronWeight;
        }
    }
}
