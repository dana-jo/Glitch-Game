using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeEffect : MonoBehaviour
{
    public float fadeDuration = 1f;

    private CanvasGroup canvasGroup;
    private Graphic uiGraphic;
    private TMP_Text tmpText;
    private SpriteRenderer spriteRenderer;

    private Coroutine fadeCoroutine;

    public bool IsFading { get; private set; }
    public bool IsFadeInDone { get; private set; }
    public bool IsFadeOutDone { get; private set; }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        uiGraphic = GetComponent<Graphic>();
        tmpText = GetComponent<TMP_Text>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FadeIn()
    {
        StartFade(1f, true);
    }

    public void FadeOut()
    {
        StartFade(0f, false);
    }

    private void StartFade(float targetAlpha, bool fadingIn)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(
            FadeHandler(targetAlpha, fadingIn)
        );
    }

    private IEnumerator FadeHandler(float targetAlpha, bool fadingIn)
    {
        IsFading = true;

        IsFadeInDone = false;
        IsFadeOutDone = false;

        float startAlpha = GetAlpha();

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / fadeDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            float alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                t
            );

            SetAlpha(alpha);

            yield return null;
        }

        SetAlpha(targetAlpha);

        IsFading = false;

        if (fadingIn)
        {
            IsFadeInDone = true;
        }
        else
        {
            IsFadeOutDone = true;
        }

        fadeCoroutine = null;
    }

    private float GetAlpha()
    {
        if (canvasGroup != null)
            return canvasGroup.alpha;

        if (uiGraphic != null)
            return uiGraphic.color.a;

        if (tmpText != null)
            return tmpText.color.a;

        if (spriteRenderer != null)
            return spriteRenderer.color.a;

        return 1f;
    }

    private void SetAlpha(float alpha)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
            return;
        }

        if (uiGraphic != null)
        {
            Color color = uiGraphic.color;
            color.a = alpha;
            uiGraphic.color = color;
            return;
        }

        if (tmpText != null)
        {
            Color color = tmpText.color;
            color.a = alpha;
            tmpText.color = color;
            return;
        }

        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}