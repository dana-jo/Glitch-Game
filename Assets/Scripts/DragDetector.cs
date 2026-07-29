using UnityEngine;
using UnityEngine.InputSystem;

public class DragDetector : MonoBehaviour
{
    private Draggable draggableInRange = null;
    // public GameObject dragIcon;

    void Start()
    {
        // dragIcon.SetActive(false);
    }
    public void OnDrag(InputAction.CallbackContext context)
    {
        if (draggableInRange == null)
            return;

        if (context.performed)
        {
            Debug.Log("Start Drag");
            // dragIcon.SetActive(false);
            draggableInRange.PickUp();
        }

        if (context.canceled)
        {
            Debug.Log("Stop Drag");
            // dragIcon.SetActive(true);
            draggableInRange.Drop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Draggable draggable) && draggable.CanBeDragged())
        {
            if(draggableInRange && draggableInRange.IsHeld)
                return;
            draggableInRange = draggable;
            // dragIcon.SetActive(true);
            // Debug.Log("Draggable in range: " + draggable.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Draggable draggable) && draggable == draggableInRange)
        {
            draggableInRange = null;
            // dragIcon?.SetActive(false);
        }
    }
}
