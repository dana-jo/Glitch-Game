using UnityEngine;

public class StartMenuUIController : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject notebookPanel;
    public GameObject settingsPanel;

    //private bool menuOn, notesOn, settingsOn;
    private void Start()
    {
        menuPanel.SetActive(false);
        notebookPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void StartGame()
    {
        Debug.Log("Start game");
        SceneController.Instance.ChangeScene("FINAL");
    }

    public void ContinueGame()
    {
        Debug.Log("Continue game");
    }

    public void ToggleNotebook()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
        notebookPanel.SetActive(!notebookPanel.activeSelf);
    }

    public void ToggleSettings()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void CloseNotebook()
    {
        menuPanel.SetActive(true);
        notebookPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        menuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
