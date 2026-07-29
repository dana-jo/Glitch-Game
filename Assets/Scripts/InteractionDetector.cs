using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private Interactable interactableRange = null;
    public GameObject interactionIcon;

    void Start()
    {
        interactionIcon.SetActive(false);

    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && interactableRange != null)
        {
            Debug.Log("Calling the Interact function");
            interactableRange?.Interact(); // calling the interact function on whatever we interacted with
            if (!interactableRange.CanInteract())
            {
                interactionIcon?.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Interactable interactable) && interactable.CanInteract())
        {
            interactableRange = interactable;
            interactionIcon.SetActive(true);
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
