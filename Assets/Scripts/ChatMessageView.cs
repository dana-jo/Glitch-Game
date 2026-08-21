using TMPro;
using UnityEngine;

public class ChatMessageView : MonoBehaviour
{
    [SerializeField] private TMP_Text _messageText;

    public void SetMessage(string text)
    {
        if (_messageText != null)
        {
            _messageText.text = text;
        }
    }
}