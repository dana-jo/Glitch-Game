using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }

    // UI stuff
    public GameObject dialoguePanel;
    public Image portraitImage;

    // Normal dialogue layout
    public GameObject normalDialogueLayout;
    public TMP_Text normalNameText;
    public TMP_Text normalDialogueText;

    // Choices layout
    public GameObject choicesLayout;
    public TMP_Text questionText;
    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;

    private TMP_Text currentDialogueText;

    public event Action<int> OnFinishedDialogue;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        currentDialogueText = normalDialogueText; // so it doesn't stay null (-_-)

        normalDialogueLayout.SetActive(false);
        choicesLayout.SetActive(false);
        ShowDialogueUI(false);
    }

    public void ShowDialogueUI(bool show)
    {
        dialoguePanel.SetActive(show);
        if (show)
            PauseController.Instance.OpenDialogue();
        else
            PauseController.Instance.ResumeGame();
    }

    public void SetNPCInfo(string npcName, Sprite portrait)
    {
        normalNameText.text = npcName;
        portraitImage.sprite = portrait;
    }

    public void SetDialogueText(string text)
    {
        currentDialogueText.text = text;
    }

    public void ClearChoices()
    {
        foreach (Transform child in choiceContainer)
            Destroy(child.gameObject);
    }

    public void CreateChoiceButton(string choiceText, UnityEngine.Events.UnityAction onClick)
    {
        GameObject choiceButton = Instantiate(choiceButtonPrefab, choiceContainer);
        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButton.GetComponentInChildren<Button>().onClick.AddListener(onClick);
    }

    public void ShowNormalDialogueLayout()
    {
        normalDialogueLayout.SetActive(true);
        choicesLayout.SetActive(false);
        currentDialogueText = normalDialogueText;
    }

    public void ShowChoicesLayout()
    {
        normalDialogueLayout.SetActive(false);
        choicesLayout.SetActive(true);
        currentDialogueText = questionText;
    }

    public void ClearDialogueText()
    {
        normalDialogueText.text = "";
        questionText.text = "";
    }

    public void OnFinishDialogue(int npcID)
    {
        OnFinishedDialogue?.Invoke(npcID);
        Debug.Log("Done talking invoke");
    }
}
