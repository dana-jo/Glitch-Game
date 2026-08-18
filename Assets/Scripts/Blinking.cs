using UnityEngine;

public class Blinking : MonoBehaviour
{
    private FadeEffect effect;
    void Start()
    {
        effect = GetComponent<FadeEffect>();
        effect.FadeOut();
    }
    void Update()
    {
        if (effect.IsFading)
            return;

        if (effect.IsFadeOutDone)
        {
            effect.FadeIn();
        }
        else if (effect.IsFadeInDone)
        {
            effect.FadeOut();
        }
    }
}
