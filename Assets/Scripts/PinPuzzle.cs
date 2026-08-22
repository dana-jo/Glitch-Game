using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
public class PinPuzzle : ObjectiveBehaviour, Interactable
{
    [SerializeField] private string keyboardCharacters = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    [SerializeField] private string correctAnswer = "";
    [SerializeField] private GameObject canvas;
    [SerializeField] private Transform pinSlotsContainer;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private Transform keyboardContainer;
    [SerializeField] private GameObject keyButtonPrefab;
    [SerializeField] private Button submitButton;
    [SerializeField] private Button clearButton;
    [SerializeField] private GameObject pinSlotPrefab;
    [SerializeField] private float closeDelay = 2f;
    [SerializeField] private UnityEvent onPuzzleCompleted;


    private readonly List<TMP_Text> pinSlotTexts = new();

    private readonly List<PinKeyButton> generatedKeyButtons = new();

    private string currentInput = "";

    private void Start()
    {
        GeneratePinSlots();
        GenerateKeyboard();
        ConnectSpecialButtons();
        OnStart();

        canvas.SetActive(false);

        if (!IsCompleted)
            ResetPuzzleUI();
    }

    private void OnDestroy()
    {
        if (submitButton != null)
            submitButton.onClick.RemoveListener(SubmitAnswer);

        if (clearButton != null)
            clearButton.onClick.RemoveListener(ClearInput);
    }

    private void ConnectSpecialButtons()
    {
        if (submitButton != null)
            submitButton.onClick.AddListener(SubmitAnswer);

        if (clearButton != null)
            clearButton.onClick.AddListener(ClearInput);
    }

    private void GenerateKeyboard()
    {

        generatedKeyButtons.Clear();
        HashSet<char> createdCharacters = new();//no duplicTes

        foreach (char character in keyboardCharacters)
        {
            if (char.IsWhiteSpace(character))
                continue;

            if (createdCharacters.Contains(character))
                continue;

            createdCharacters.Add(character);

            GameObject buttonObject = Instantiate(
                keyButtonPrefab,
                keyboardContainer
            );

            PinKeyButton keyButton = buttonObject.GetComponent<PinKeyButton>();

            keyButton.Initialize(this, character.ToString());
            generatedKeyButtons.Add(keyButton);
        }
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        if (!CanInteract())
            return;

        canvas.SetActive(!canvas.activeSelf);
        PauseController.Instance.OpenUI(canvas.activeSelf);
    }

    public void PressKey(string keyValue)
    {
        if (IsCompleted)
            return;

        if (string.IsNullOrEmpty(keyValue))
            return;

        if (currentInput.Length >= correctAnswer.Length)
            return;

        currentInput += keyValue;

        UpdateInputDisplay();

        if (statusText != null)
            statusText.text = "";
    }

    public void ClearInput()
    {
        if (IsCompleted)
            return;

        currentInput = "";
        UpdateInputDisplay();

        if (statusText != null)
            statusText.text = "";
    }

    public void SubmitAnswer()
    {
        if (IsCompleted)
            return;

        bool isCorrect = string.Equals(
        currentInput,
        correctAnswer,
        StringComparison.Ordinal
        );

        if (isCorrect)
        {
            UpdatePuzzleState();
        }
        else
        {
            HandleWrongAnswer();
        }
    }

    private void HandleWrongAnswer()
    {
        if (statusText != null)
            statusText.text = "INCORRECT CODE";
            currentInput = "";
            UpdateInputDisplay();
    }

    public override void UpdatePuzzleState()
    {
        if (IsCompleted)
            return;

        Complete();

        currentInput = correctAnswer;

        ApplyCompletedState();
        onPuzzleCompleted?.Invoke();

        StartCoroutine(ClosePuzzleAfterDelay());
    }

        private IEnumerator ClosePuzzleAfterDelay()
    {
        yield return new WaitForSeconds(closeDelay);

        if (canvas != null)
            canvas.SetActive(false);
    }

    public override void RestoreCompletedState()
    {
        currentInput = correctAnswer;

        ApplyCompletedState();
    }

    private void ApplyCompletedState()
    {
        UpdateInputDisplay();

        if (statusText != null)
            statusText.text = "ACCESS GRANTED";

        SetKeyboardInteractable(false);
    }

    private void SetKeyboardInteractable(bool interactable)
    {
        foreach (PinKeyButton keyButton in generatedKeyButtons)
        {
            if (keyButton != null)
                keyButton.SetInteractable(interactable);
        }

        if (submitButton != null)
            submitButton.interactable = interactable;

        if (clearButton != null)
            clearButton.interactable = interactable;
    }

    private void UpdateInputDisplay()
    {
        for (int i = 0; i < pinSlotTexts.Count; i++)
        {
            if (pinSlotTexts[i] == null)
                continue;

            if (i < currentInput.Length)
            {
                pinSlotTexts[i].text = currentInput[i].ToString();
            }
            else
            {
                pinSlotTexts[i].text = "";
            }
        }
    }

    private void GeneratePinSlots()
    {

        pinSlotTexts.Clear();

        for (int i = 0; i < correctAnswer.Length; i++)
        {
            GameObject slotObject = Instantiate(
                pinSlotPrefab,
                pinSlotsContainer
            );

            slotObject.name = $"Pin Slot {i + 1}";

            TMP_Text slotText = slotObject.GetComponentInChildren<TMP_Text>();

            slotText.text = "";
            pinSlotTexts.Add(slotText);
        }
    }
    private void ResetPuzzleUI()
    {
        currentInput = "";

        UpdateInputDisplay();
        SetKeyboardInteractable(true);

        if (statusText != null)
            statusText.text = "";
    }
}