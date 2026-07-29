using UnityEngine;
using UnityEngine.UI;

public class SettingsUIManager : MonoBehaviour
{
    public static SettingsUIManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject modalBlocker;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        settingsPanel.SetActive(false);
        modalBlocker.SetActive(false);
    }

    public void ToggleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
        modalBlocker.SetActive(settingsPanel.activeSelf);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        modalBlocker.SetActive(false);
    }

 
}
