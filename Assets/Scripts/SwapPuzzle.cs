using UnityEngine;

public class SwapPuzzle : ObjectiveBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI weightText;
    [SerializeField] private float targetWeight  = 26.0f ;
    private float currentWeight = 0.0f;
    [SerializeField] private Cauldron cauldron;
    void Start()
    {
        OnStart();
        weightText.text = currentWeight.ToString("F2") +" / \n" + targetWeight.ToString("F2")+" KG"; 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(IsCompleted)
            return;
        if(collision.TryGetComponent(out Cauldron cauldron1) && cauldron == cauldron1)
        {
            Debug.Log("Cauldron detected: " + cauldron.name  + " and weight: " + cauldron.GetWeight());
            currentWeight += cauldron.GetWeight();
            cauldron.canInteract = false;
            UpdatePuzzleState();
        }      
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(IsCompleted)
            return;

        if(collision.TryGetComponent(out Cauldron cauldron1) && cauldron == cauldron1)
        {
            currentWeight -= cauldron.GetWeight();
            cauldron.canInteract = true;
            UpdatePuzzleState();
        }
    }

    public override void UpdatePuzzleState()
    {
        if(currentWeight >= targetWeight)
        {
            Complete();
            CompleteState();            
        }
        else
        {
            weightText.text = currentWeight.ToString("F2") +" / \n" + targetWeight.ToString("F2")+" KG"; 
            Debug.Log($"still need {targetWeight - currentWeight} KG ! ");
            // red lights
        }
    }

    public override void RestoreCompletedState()
    {
        base.RestoreCompletedState();
        CompleteState();
    }
    private void CompleteState()
    {
        weightText.text = targetWeight.ToString("F2") +" / \n" + targetWeight.ToString("F2")+" KG"; 
        Draggable draggable = cauldron.GetComponent<Draggable>();
        if (draggable != null)
        {
            draggable.Drop();
            draggable.canBeDragged = false;
        }
        cauldron.transform.position = transform.position - new Vector3(0, 0.5f, 0);;
        // green lights
    }

}
