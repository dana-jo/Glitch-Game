using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    public GameObject inventoryGroup;
    void Start()
    {
        inventoryGroup.SetActive(false);
    }

  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //if (!inventoryGroup.activeSelf && PauseController.IsGamePause)
            //{
            //    return;
            //}
            inventoryGroup.SetActive(!inventoryGroup.activeSelf);
            //PauseController.Setpause(inventoryGroup.activeSelf);

        }
    }
}




