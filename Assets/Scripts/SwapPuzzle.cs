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
        Debug.Log("SwapPuzzle Start");
        if(IsCompleted)
        {
            // Debug.Log("SwapPuzzle already completed");
            CompleteState();
        }
        else
        {
            // Debug.Log("SwapPuzzle not completed yet");
            weightText.text = currentWeight.ToString("F2") +" / \n" + targetWeight.ToString("F2")+" KG"; 
        }
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
            // test adding to notebook :  
            NotebookController.Instance.AddNote(1);    
            NotebookController.Instance.AddNote(2);    
            NotebookController.Instance.AddNote(3);    
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
        cauldron.fillCauldron("Mud");
        if (draggable != null)
        {
            draggable.Drop();
            draggable.canBeDragged = false;
        }
        cauldron.transform.position = transform.position - new Vector3(0, 0.5f, 0);
        cauldron.canInteract = false;
        // find Mud Cauldron and set its content to water
        Cauldron mudCauldron = GameObject.Find("MudCauldron").GetComponent<Cauldron>();
        if (mudCauldron != null && mudCauldron.Liquid == "Mud")
        {
            mudCauldron.fillCauldron("Water");
        }
        // green lights
    }

}
