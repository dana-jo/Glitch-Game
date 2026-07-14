using System.Collections;
using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    public float shakeDuration = 0.2f;
    public float shakeRange = 30f;
    public float shakeCount = 7f;

    public void StartShake()
    {
        Debug.Log("start shake");
        StartCoroutine(ShakeHandler());
    }

    private IEnumerator ShakeHandler()
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0f;
        float singleShakeDurtion = shakeDuration / shakeCount;

        while (elapsed < shakeDuration)
        {
            Vector2 offset = Random.insideUnitCircle * shakeRange;
            Vector3 target = startPosition + new Vector3(offset.x, offset.y, 0);

            float t = 0f;
            while (t < singleShakeDurtion)
            {
                transform.position = Vector3.Lerp(transform.position, target, t / singleShakeDurtion);
                t += Time.deltaTime;
                yield return null;
            }

            elapsed += singleShakeDurtion;
        }

        transform.position = startPosition;
    }

    public void StopShake()
    {
        Debug.Log("force stop shake");
        StopAllCoroutines();
    }
}
