using System.Collections;
using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    public float shakeDuration = 0.2f;
    public float shakeRange = 30f;
    public float shakeCount = 7f;

    private Vector3 startPosition;

    public void StartShake()
    {
        Debug.Log("start shake");
        StartCoroutine(ShakeHandler());
    }

    private IEnumerator ShakeHandler()
    {
        startPosition = transform.position;
        float elapsed = 0f;
        float singleShakeDuration = shakeDuration / shakeCount;

        while (elapsed < shakeDuration)
        {
            Vector2 offset = Random.insideUnitCircle * shakeRange;
            Vector3 target = startPosition + new Vector3(offset.x, offset.y, 0);

            Vector3 start = transform.position;

            float t = 0f;
            while (t < singleShakeDuration)
            {
                t += Time.deltaTime;
                float progress = t / singleShakeDuration;

                progress = Mathf.SmoothStep(0f, 1f, progress);

                transform.position = Vector3.Lerp(start, target, progress);
                yield return null;
            }

            elapsed += singleShakeDuration;
        }

        transform.position = startPosition;
    }

    public void StopShake()
    {
        Debug.Log("force stop shake");
        StopAllCoroutines();
    }

    public void StopShakeAndReset()
    {
        Debug.Log("force stop shake");
        transform.position = startPosition;
        StopAllCoroutines();
    }
}
