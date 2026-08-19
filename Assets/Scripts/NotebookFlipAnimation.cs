using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NotebookFlipAnimation : MonoBehaviour
{
    [SerializeField] private Image bookImage;
    [SerializeField] private Sprite[] flipFrames;

    [SerializeField] private float frameTime = 0.05f;

    private Coroutine flipCoroutine;

    public bool IsFlipping { get; private set; }

    public void FlipForward()
    {
        if (IsFlipping)
            return;

        flipCoroutine = StartCoroutine(PlayForward());
    }

    public void FlipBackward()
    {
        if (IsFlipping)
            return;

        flipCoroutine = StartCoroutine(PlayBackward());
    }

    private IEnumerator PlayForward()
    {
        IsFlipping = true;

        for (int i = 0; i < flipFrames.Length; i++)
        {
            bookImage.sprite = flipFrames[i];
            yield return new WaitForSeconds(frameTime);
        }

        IsFlipping = false;
    }

    private IEnumerator PlayBackward()
    {
        IsFlipping = true;

        for (int i = flipFrames.Length - 1; i >= 0; i--)
        {
            bookImage.sprite = flipFrames[i];
            yield return new WaitForSeconds(frameTime);
        }

        IsFlipping = false;
    }
}