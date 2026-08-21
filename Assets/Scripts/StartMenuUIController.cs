using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartMenuUIController : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject notebookPanel;
    public GameObject settingsPanel;
    public Button continueButton;
    public string gameSceneName;

    //private bool menuOn, notesOn, settingsOn;
    private void Start()
    {
        menuPanel.SetActive(false);
        notebookPanel.SetActive(false);
        settingsPanel.SetActive(false);


        if (!SaveController.Instance.HasSaveData())
        {
            continueButton.interactable = SaveController.Instance.HasSaveData();
            continueButton.GetComponentInChildren<TMP_Text>().alpha = 0.5f;
            continueButton.GetComponent<EventTrigger>().enabled = false; 
        }
    }

    public void StartGame()
    {
        Debug.Log("Start game");
        SaveController.Instance.SaveSettings();
        SaveController.Instance.DeleteSave();
        SceneController.Instance.ChangeScene(gameSceneName);
    }

    public void ContinueGame()
    {
        Debug.Log("Continue game");
        SaveController.Instance.SaveSettings();
        SceneController.Instance.ChangeScene(gameSceneName);
        //SaveController.Instance.LoadGame();
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
