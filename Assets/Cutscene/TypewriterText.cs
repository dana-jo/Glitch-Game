using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterText : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private float charactersPerSecond = 35f;

    private Coroutine typingCoroutine;

    private void Awake()
    {
        ClearText();
    }

    public void TypeLine(string line)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeRoutine(line));
    }

    public void ClearText()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.text = "";
        dialogueText.maxVisibleCharacters = 0;
    }

    private IEnumerator TypeRoutine(string line)
    {
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        dialogueText.ForceMeshUpdate();

        int totalCharacters = dialogueText.textInfo.characterCount;
        float delay = 1f / charactersPerSecond;

        for (int i = 0; i <= totalCharacters; i++)
        {
            dialogueText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(delay);
        }

        typingCoroutine = null;
    }
}