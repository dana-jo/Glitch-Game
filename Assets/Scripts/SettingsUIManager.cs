using UnityEngine;
using UnityEngine.UI;

public class SettingsUIManager : MonoBehaviour
{
    public static SettingsUIManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject modalBlocker;
    [SerializeField] private GameObject Settings;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        settingsPanel.SetActive(true);
        modalBlocker.SetActive(true);
        Settings.SetActive(false);
    }

    public void ToggleSettings()
    {
        Settings.SetActive(!Settings.activeSelf);
        PauseController.Instance.OpenSettings(Settings.activeSelf);
    }

    public void CloseSettings()
    {
        Settings.SetActive(false);
    }
}
