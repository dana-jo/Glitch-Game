using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PinKeyButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text keyText;

    private Button button;
    private PinPuzzle pinPuzzle;
    private string keyValue;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (keyText == null)
            keyText = GetComponentInChildren<TMP_Text>();
    }

    private void GetReferences()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (keyText == null)
            keyText = GetComponentInChildren<TMP_Text>(true);
    }

    public void Initialize(PinPuzzle puzzle, string value)
    {
        GetReferences();

        pinPuzzle = puzzle;
        keyValue = value;

        if (keyText != null)
            keyText.text = keyValue;

        button.onClick.RemoveListener(PressKey);
        button.onClick.AddListener(PressKey);
    }

    private void PressKey()
    {
        if (pinPuzzle == null)
            return;

        pinPuzzle.PressKey(keyValue);
    }

    public void SetInteractable(bool interactable)
    {
        if (button != null)
            button.interactable = interactable;
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(PressKey);
    }
}