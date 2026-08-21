using UnityEngine;

public class PauseUIController : MonoBehaviour
{

    public GameObject pauseMenu;
   
    public void TogglePauseMenu()
    {
        if (SceneController.Instance.EndCutscene())
            return;

        if (pauseMenu == null) return; 

        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Debug.Log("Toggling pause menu");

        PauseController.Instance.OpenSettings(pauseMenu.activeSelf);
    }

    public void QuitToMainMenu()
    {
        SceneController.Instance.ChangeScene("Start Menu");
    }

}
