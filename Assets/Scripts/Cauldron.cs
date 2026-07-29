using UnityEngine;
using UnityEngine.UI;

public class Cauldron : MonoBehaviour , Interactable
{
    [SerializeField] private string Liquid; 
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite waterSprite;
    [SerializeField] private Sprite mudSprite;
    private SpriteRenderer spriteRenderer;
    private Draggable draggable;
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

        if (current == null)
            return;

        if(!CanInteract()) //  no interaction if the cauldron is being dragged
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
        return ( draggable == null ||(draggable != null && !draggable.IsHeld));
    }
    private void emptyCauldron()
    {
        Liquid = "";
        spriteRenderer.sprite = emptySprite;
    }
    private void fillCauldron(string liquid)
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
}
