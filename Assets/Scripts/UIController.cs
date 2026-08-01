using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject inventoryGroup;
    public GameObject questsPage;
    void Start()
    {
        inventoryGroup.SetActive(false);
        questsPage.SetActive(false);
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

        if (Input.GetKeyDown(KeyCode.J))
        {
            //if (!questsPage.activeSelf && PauseController.IsGamePause)
            //{
            //    return;
            //}
            questsPage.SetActive(!questsPage.activeSelf);
            //PauseController.Setpause(questsPage.activeSelf);

        }
    }
}




