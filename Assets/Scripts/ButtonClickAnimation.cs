using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickAnimation : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float frameTime = 0.05f;

    private bool isAnimating;

    public void PlayAnimation()
    {
        if (!isAnimating)
            StartCoroutine(AnimateButton());
    }

    private IEnumerator AnimateButton()
    {
        isAnimating = true;

        // 1 ? 2 ? 3 ? 4 ? 5
        for (int i = 0; i < frames.Length; i++)
        {
            buttonImage.sprite = frames[i];
            yield return new WaitForSecondsRealtime(frameTime);
        }

        // 4 ? 3 ? 2 ? 1
        for (int i = frames.Length - 2; i >= 0; i--)
        {
            buttonImage.sprite = frames[i];
            yield return new WaitForSecondsRealtime(frameTime);
        }

        isAnimating = false;
    }
}