using UnityEngine;
using UnityEngine.UI;

public class BrightnessManager : MonoBehaviour
{
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Image brightnessPanel;
    [SerializeField] private float maxAlpha = 0.7f;
    [SerializeField] private Image brightnessBarImage;
    [SerializeField] private Sprite[] brightnessBarSprites;

    void Start()
    {
        brightnessSlider.onValueChanged.AddListener(delegate { OnBrightnessChanged(); });
        OnBrightnessChanged();
    }
    public void OnBrightnessChanged()
    {
        float alpha = 1 - brightnessSlider.value;
        alpha = Mathf.Clamp(alpha, 0f, maxAlpha);
        brightnessPanel.color = new Color(0, 0, 0, alpha);

        int index = Mathf.RoundToInt(brightnessSlider.value * 10);
        brightnessBarImage.sprite = brightnessBarSprites[index];
    }

    public void IncreaseBrightness()
    {
        brightnessSlider.value = Mathf.Clamp01(brightnessSlider.value + 0.1f);
    }

    public void DecreaseBrightness()
    {
        brightnessSlider.value = Mathf.Clamp01(brightnessSlider.value - 0.1f);
    }
}