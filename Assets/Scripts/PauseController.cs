using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance { get; private set; }

    [SerializeField] 
    private PlayerInput playerInput;

    private InputAction move;
    private InputAction interact;
    //private InputAction inventory;
    //private InputAction quests;
    //private InputAction notes;
    //private InputAction drag;

    // ui puzzles;

    private

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        var player = playerInput.actions.FindActionMap("Player");
        if (player == null)
        {
            Debug.Log("Player action map not found");
        }

        move = player.FindAction("Move");
        interact = player.FindAction("Interact");

        if ( move == null || interact == null)
        {
            Debug.Log("Couldn't find input systems");
        }
    }
    public void OpenDialogue()
    {
        move.Disable();
        interact.Enable();
    }

    public void OpenInventory()
    {
        // ----------------------------
    }

    public void OpenQuests()
    {
        // ----------------------------
    }

    public void OpenNotebook()
    {
        // ----------------------------
    }

    public void OpenSettings()
    {
        // ----------------------------
    }

    public void OpenMap()
    {
        // ----------------------------
    }

    public void Dragging()
    {
        // ----------------------------
    }

    public void ResumeGame()
    {
        move.Enable();
        interact.Enable();
    }
}