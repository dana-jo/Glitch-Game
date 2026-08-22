using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance { get; private set; }

    [SerializeField] 
    private PlayerInput playerInput;

    private InputAction move;
    private InputAction interact;
    private InputAction drag;
    private InputAction inventory;
    private InputAction quests;
    private InputAction notebook;
    //private InputAction settings;
    private InputAction chat;
    // ui puzzles;


   
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

        move = player.FindAction("Move", true);
        interact = player.FindAction("Interact", true);
        drag = player.FindAction("Drag", true);
        inventory = player.FindAction("Inventory", true);
        quests = player.FindAction("Quests", true);
        notebook = player.FindAction("Notebook", true);
      //  settings = player.FindAction("Settings", true);
        chat = player.FindAction("Chat", true);
    }
    public void OpenDialogue(bool opened)
    {
        if (opened)
        {
            move.Disable();
            interact.Enable();
            drag.Disable();
            inventory.Disable();
            quests.Disable();
            notebook.Disable();
           // settings.Disable();
            chat.Disable();
        }
        else
        {
            ResumeGame();
        }
        
    }

    public void OpenInventory(bool opened)
    {
        if (opened)
        {
            move.Disable();
            interact.Disable();
            drag.Disable();
            inventory.Enable();
            quests.Disable();
            notebook.Disable();
          //  settings.Disable();
            chat.Disable();
        }
        else
        {
            ResumeGame();
        }
        
    }

    public void OpenQuests(bool opened)
    {
        if (opened)
        {
            move.Disable();
            interact.Disable();
            drag.Disable();
            inventory.Disable();
            quests.Enable();
            notebook.Disable();
        //    settings.Disable();
            chat.Disable();
        }
        else
        {
            ResumeGame();
        }
        
    }

    public void OpenNotebook(bool opened)
    {
        if (opened)
        {
            move.Disable();
            interact.Disable();
            drag.Disable();
            inventory.Disable();
            quests.Disable();
            notebook.Enable();
           // settings.Disable();
            chat.Disable();
        }
        else
        {
            ResumeGame();
        }
        
    }

    public void OpenSettings(bool opened)
    {
        if (opened)
        {
            move.Disable();
            interact.Disable();
            drag.Disable();
            inventory.Disable();
            quests.Disable();
            notebook.Disable();
            //settings.Enable();
            chat.Disable();
        }
        else
        {
            ResumeGame();
        }
        
    }
    public void OpenChat(bool opened)
    {
        if (opened)
        {
            move.Disable();
            interact.Disable();
            drag.Disable();
            inventory.Disable();
            quests.Disable();
            notebook.Disable();
         //   settings.Disable();
            chat.Enable(); 
        }
        else
        {
            ResumeGame();
        }
    }

    public void OpenUI(bool opened)
    {
        if (opened)
        {
            move.Disable();
            interact.Enable();
            drag.Disable();
            inventory.Disable();
            quests.Disable();
            notebook.Disable();
            //   settings.Disable();
            chat.Disable();
        }
        else
        {
            ResumeGame();
        }
    }

    public void OpenMap()
    {
        // ----------------------------
    }

    public void Dragging(bool opened)
    {
        if (opened)
        {
            move.Enable();
            interact.Disable();
            drag.Enable();
            inventory.Disable();
            quests.Disable();
            notebook.Disable();
           // settings.Disable();
            chat.Disable();
        }
        else
        {
            ResumeGame();
        }
        
    }

    public void ResumeGame()
    {
        move.Enable();
        interact.Enable();
        drag.Enable();
        inventory.Enable();
        quests.Enable();
        notebook.Enable();
       // settings.Enable();
     chat.Enable();
    }
}