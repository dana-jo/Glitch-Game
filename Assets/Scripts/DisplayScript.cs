using UnityEngine;
using UnityEngine.UI;

public class DisplayScript : MonoBehaviour, Interactable
{
    public GameObject canvas;

    [Header("Scroll")]
    public RectTransform content;
    public GameObject scroll;
    public float scrollStep = 0.1f;
    private ScrollRect scrollRect;

    [Header("Light")]
    public GameObject greenLight;

    [Header("Image")]
    public GameObject ourImage;
    private Image image;
    private AspectRatioFitter aspectRatioFitter;

    private void Start()
    {
        canvas.SetActive(false);

        //SetAspectRatio();
    }
    private void Awake()
    {
        scrollRect = scroll.GetComponent<ScrollRect>();
        image = ourImage.GetComponent<Image>();

        aspectRatioFitter = ourImage.GetComponent<AspectRatioFitter>();
        SetAspectRatio();

        //content.SetSizeWithCurrentAnchors(
        //    RectTransform.Axis.Vertical,
        //    ourImage.GetComponent<RectTransform>().rect.height
        //);

        //Debug.Log("hight " + ourImage.GetComponent<RectTransform>().rect.height);
    }

    private void LateUpdate()
    {
        content.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            ourImage.GetComponent<RectTransform>().rect.height
        );
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        canvas.SetActive(!canvas.activeSelf);
        PauseController.Instance.OpenUI(canvas.activeSelf);
    }

    public void ScrollUp()
    {
        if (scrollRect == null) return;

        float newPos = scrollRect.verticalNormalizedPosition + scrollStep;
        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(newPos);
    }
    public void ScrollDown()
    {
        if (scrollRect == null) return;

        float newPos = scrollRect.verticalNormalizedPosition - scrollStep;
        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(newPos);
    }

    public void ToggleDisplay()
    {
        scroll.SetActive(!scroll.activeSelf);
        greenLight.SetActive(scroll.activeSelf);
    }

    private void SetAspectRatio()
    {
        if (image.sprite == null)
            return;

        aspectRatioFitter.aspectRatio =
            image.sprite.rect.width / image.sprite.rect.height;
    }

}
