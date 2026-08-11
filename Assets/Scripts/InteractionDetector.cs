using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private Interactable interactableRange = null;
    public GameObject interactionIcon;
    private PopupManager popupManagerInstance;
    private Boolean popupShown = false;

    void Start()
    {
        interactionIcon.SetActive(false);
        popupManagerInstance = PopupManager.Instance;

    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(DragDetector.isDragging)
            return;
            
        if (context.performed && interactableRange != null)
        {
            if (!interactableRange.CanInteract())
            {
                interactionIcon?.SetActive(false);
            }
            Debug.Log("Calling the Interact function");
            interactableRange?.Interact(); // calling the interact function on whatever we interacted with
        }
    }

    public void OnCycleLoopCount(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (interactableRange is LoopStartBlock loopBlock)
        {
            loopBlock.CycleCount();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Interactable interactable) && interactable.CanInteract())
        {
            interactableRange = interactable;
            interactionIcon.SetActive(true);
            if (interactableRange is LoopStartBlock loopBlock && !popupShown)
            {
                popupManagerInstance.ShowPopup("Press 'J' to increase the loop count for this block.");
                popupShown = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Interactable interactable) && interactable == interactableRange)
        {
            interactableRange = null;
            interactionIcon.SetActive(false);
        }
    }
    private void Update()
    {
        if (interactableRange == null)
        {
            interactionIcon.SetActive(false);
            return;
        }
        interactionIcon.SetActive(interactableRange.CanInteract());
    }
}
