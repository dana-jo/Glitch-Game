using UnityEngine;
using UnityEngine.UI;

public class BrightnessManager : MonoBehaviour
{
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Image brightnessPanel;
    [SerializeField] private float maxAlpha = 0.7f;
    [SerializeField] private Image brightnessBarImage;
    [SerializeField] private Sprite[] brightnessBarSprites;

    private float brightness = 1f;

    void Start()
    {
        if (brightnessSlider != null)
        {
            brightnessSlider.onValueChanged.AddListener(delegate { OnBrightnessChanged(); });
            brightness = brightnessSlider.value;
        }

        OnBrightnessChanged();
    }

    public void OnBrightnessChanged()
    {
        if (brightnessSlider != null)
            brightness = brightnessSlider.value;

        ApplyBrightness();
    }

    private void ApplyBrightness()
    {
        float alpha = 1 - brightness;
        alpha = Mathf.Clamp(alpha, 0f, maxAlpha);

        if (brightnessPanel != null)
            brightnessPanel.color = new Color(0, 0, 0, alpha);

        if (brightnessBarImage != null && brightnessBarSprites != null && brightnessBarSprites.Length > 0)
        {
            int index = Mathf.RoundToInt(brightness * 10);
            index = Mathf.Clamp(index, 0, brightnessBarSprites.Length - 1);

            brightnessBarImage.sprite = brightnessBarSprites[index];
        }
    }

    public void IncreaseBrightness()
    {
        brightness = Mathf.Clamp01(brightness + 0.1f);

        if (brightnessSlider != null)
            brightnessSlider.value = brightness;
        else
            ApplyBrightness();
    }

    public void DecreaseBrightness()
    {
        brightness = Mathf.Clamp01(brightness - 0.1f);

        if (brightnessSlider != null)
            brightnessSlider.value = brightness;
        else
            ApplyBrightness();
    }

    public void LoadBrightnessSettings(float value)
    {
        brightness = Mathf.Clamp01(value);

        if (brightnessSlider != null)
            brightnessSlider.value = brightness;

        ApplyBrightness();
    }

    public float GetBrightness()
    {
        return brightness;
    }
}