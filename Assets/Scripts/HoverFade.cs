using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverFade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float speed = 1f;

    private TMP_Text text;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        fadeRoutine = StartCoroutine(Fade());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        Color color = text.color;
        color.a = 1f;
        text.color = color;
    }

    private IEnumerator Fade()
    {
        while (true)
        {
            Color color = text.color;

            color.a = Mathf.PingPong(Time.time * speed, 1f);

            text.color = color;

            yield return null;
        }
    }
}