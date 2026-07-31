using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupView : MonoBehaviour
{
    
    [SerializeField] private RectTransform popupRoot;
    [SerializeField] private GameObject iconPlace;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private CanvasGroup canvasGroup;

    private string baseTitle;
    private float visibleUntil;

    public bool CanCombine { get; private set; }

    public void Setup(
        string title,
        string description,
        Sprite icon,
        int amount = 1)
    {
        baseTitle = title;

        UpdateAmount(amount);

        bool hasDescription =
            !string.IsNullOrWhiteSpace(description);

        descriptionText.gameObject.SetActive(hasDescription);

        if (hasDescription)
        {
            descriptionText.text = description;
        }

        bool hasIcon = icon != null;

        iconPlace.SetActive(hasIcon);

        if (hasIcon)
        {
            iconImage.sprite = icon;
        }

        Canvas.ForceUpdateCanvases();

        if (popupRoot != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(
                popupRoot
            );
        }
    }

    public void UpdateAmount(int amount)
    {
        titleText.text = amount > 1
            ? $"{baseTitle} x{amount}"
            : baseTitle;

        Canvas.ForceUpdateCanvases();

        if (popupRoot != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(
                popupRoot
            );
        }
    }

    public void RefreshDuration(float displayDuration)
    {
        visibleUntil = Mathf.Max(
            visibleUntil,
            Time.unscaledTime + displayDuration
        );
    }

    public IEnumerator Play(
        float displayDuration,
        float fadeDuration)
    {
        canvasGroup.alpha = 0f;

        CanCombine = true;

        visibleUntil =
            Time.unscaledTime +
            fadeDuration +
            displayDuration;

        yield return Fade(0f, 1f, fadeDuration);

        while (Time.unscaledTime < visibleUntil)
        {
            yield return null;
        }

        CanCombine = false;

        yield return Fade(1f, 0f, fadeDuration);
    }

    private IEnumerator Fade(
        float startAlpha,
        float endAlpha,
        float duration)
    {
        if (duration <= 0f)
        {
            canvasGroup.alpha = endAlpha;
            yield break;
        }

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float percentage =
                Mathf.Clamp01(elapsedTime / duration);

            canvasGroup.alpha = Mathf.Lerp(
                startAlpha,
                endAlpha,
                percentage
            );

            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}