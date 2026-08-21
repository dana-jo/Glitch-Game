using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AIChatUIManager : MonoBehaviour
{
    public static AIChatUIManager Instance { get; private set; }

    [Header("Main Panel")]
    [SerializeField] private GameObject _mainChatPanel;

    [Header("Core References")]
    [SerializeField] private AIChatController _chatController;

    [Header("UI Elements")]
    [SerializeField] private TMP_InputField _chatInputField;
    [SerializeField] private Button _sendButton;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private Transform _contentPanel;

    /*[Header("Prefabs")]
    [SerializeField] private GameObject _chatMessagePrefab;
    [Header("Message Styling")]
    [SerializeField] private Color _messageTextColor = Color.white; */
    private InputAction _chatAction;
    [Header("Prefabs")]
    [SerializeField] private ChatMessageView _userMessagePrefab;
    [SerializeField] private ChatMessageView _aiMessagePrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (_mainChatPanel != null)
            _mainChatPanel.SetActive(false);
    }

    private void Start()
    {
        if (_chatController == null)
            _chatController = FindAnyObjectByType<AIChatController>();

        if (_sendButton != null)
            _sendButton.onClick.AddListener(OnSendButtonClicked);

        if (_chatInputField != null)
            _chatInputField.onSubmit.AddListener(OnInputFieldSubmit);

      
        SetupInputSystem();
    }

    private void SetupInputSystem()
    {
        var playerInput = FindAnyObjectByType<PlayerInput>();
        if (playerInput != null)
        {
            //_chatAction = playerInput.actions.FindAction("Chat", false);
            //if (_chatAction != null)
            //{
            //    _chatAction.started -= OnChatInputTriggered; 
            //    _chatAction.started += OnChatInputTriggered;
            //    _chatAction.Enable(); 
            //}
            //else
            //{
            //    Debug.LogError("[AIChatUIManager] Action 'Chat' NOT found in PlayerInput Actions!");
            //}
        }
        else
        {
            Debug.LogError("[AIChatUIManager] PlayerInput component NOT found in Scene!");
        }
    }

    private void OnDestroy()
    {
        if (_sendButton != null)
            _sendButton.onClick.RemoveListener(OnSendButtonClicked);

        if (_chatInputField != null)
            _chatInputField.onSubmit.RemoveListener(OnInputFieldSubmit);

        if (_chatAction != null)
            _chatAction.started -= OnChatInputTriggered;
    }

    private void OnChatInputTriggered(InputAction.CallbackContext ctx)
    {

        if (_chatInputField != null && _chatInputField.isFocused) return;

        ToggleChat();
    }

    public void ToggleChat()
    {
        if (_mainChatPanel == null) return;

        bool isActive = !_mainChatPanel.activeSelf;
        _mainChatPanel.SetActive(isActive);

        if (PauseController.Instance != null)
            PauseController.Instance.OpenChat(isActive);

        if (isActive)
            StartCoroutine(FocusInputField());
    }

    private void OnInputFieldSubmit(string text)
    {
        OnSendButtonClicked();
    }

    public void OnSendButtonClicked()
    {
        if (_chatInputField == null) return;

        string userInput = _chatInputField.text.Trim();
        if (string.IsNullOrEmpty(userInput)) return;

        if (_chatController == null) return;

        _chatInputField.text = "";
        SetUIInteractable(false);

        InstantiateMessage( _userMessagePrefab, userInput);
        _chatController.SubmitQuestion(userInput, OnAIResponseReceived);
    }

    private void OnAIResponseReceived(string aiResponse)
    {
        InstantiateMessage( _userMessagePrefab, aiResponse);
        SetUIInteractable(true);
        StartCoroutine(FocusInputField());
    }

    private void InstantiateMessage(ChatMessageView messagePrefab, string messageText)
    {
        if (messagePrefab == null || _contentPanel == null) return;

        ChatMessageView newMsgView = Instantiate(messagePrefab, _contentPanel);
        newMsgView.SetMessage(messageText);

        StartCoroutine(ScrollToBottom());
    }

    private void SetUIInteractable(bool state)
    {
        if (_chatInputField != null) _chatInputField.interactable = state;
        if (_sendButton != null) _sendButton.interactable = state;
    }

    private IEnumerator FocusInputField()
    {
        yield return null;
        if (_chatInputField != null && _mainChatPanel != null && _mainChatPanel.activeSelf)
        {
            _chatInputField.Select();
            _chatInputField.ActivateInputField();
        }
    }

    private IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
        if (_scrollRect != null)
            _scrollRect.verticalNormalizedPosition = 0f;
    }
}