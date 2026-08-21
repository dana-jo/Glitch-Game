using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject inventoryGroup;
    public GameObject questsPage;
    public GameObject notebook;
    public GameObject chat;
    void Start()
    {
        inventoryGroup.SetActive(false);
        questsPage.SetActive(false);
        notebook.SetActive(false);
        chat.SetActive(false);
    }

  
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        //if (!inventoryGroup.activeSelf && PauseController.IsGamePause)
    //        //{
    //        //    return;
    //        //}
    //        inventoryGroup.SetActive(!inventoryGroup.activeSelf);
    //        //PauseController.Setpause(inventoryGroup.activeSelf);

    //    }

    //    if (Input.GetKeyDown(KeyCode.J))
    //    {
    //        //if (!questsPage.activeSelf && PauseController.IsGamePause)
    //        //{
    //        //    return;
    //        //}
    //        questsPage.SetActive(!questsPage.activeSelf);
    //        //PauseController.Setpause(questsPage.activeSelf);

    //    }
    //}
}




