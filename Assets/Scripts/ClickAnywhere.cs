using UnityEngine;

public class ClickAnywhere : MonoBehaviour
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
        else if (effect.IsFadeInDone)
            effect.FadeOut();
        else
            effect.FadeIn();
    }
}
