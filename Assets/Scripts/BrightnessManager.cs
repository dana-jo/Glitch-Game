using UnityEngine;
using UnityEngine.UI;

public class BrightnessManager : MonoBehaviour
{
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Image brightnessPanel;
    [SerializeField] private float maxAlpha = 0.7f;

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
    }
}