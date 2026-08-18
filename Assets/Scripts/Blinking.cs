using System.Collections;
using UnityEngine;

public class Blinking : MonoBehaviour
{
    private FadeEffect effect;
    private Coroutine blinkCoroutine;

    private void Awake()
    {
        effect = GetComponent<FadeEffect>();
    }

    private void OnEnable()
    {
        if (effect == null)
            effect = GetComponent<FadeEffect>();

        blinkCoroutine = StartCoroutine(Blink());
    }

    private void OnDisable()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
    }

    private IEnumerator Blink()
    {
        effect.FadeOut();

        yield return new WaitUntil(() => !effect.IsFading);

        while (true)
        {
            effect.FadeIn();

            yield return new WaitUntil(() => !effect.IsFading);

            effect.FadeOut();

            yield return new WaitUntil(() => !effect.IsFading);
        }
    }
}