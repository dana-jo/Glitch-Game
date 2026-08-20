using UnityEngine;

public class ShowSimpleUI : MonoBehaviour, Interactable
{

    public GameObject canvas;
    void Start()
    {
        canvas.SetActive(false);

    }

    void Update()
    {

    }
    public bool CanInteract()
    {
        return true;
}

    public void Interact()
    {
        canvas.SetActive(!canvas.activeSelf);
    }
}

