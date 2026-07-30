using UnityEngine;

public class PopupData
{
    public string Title;
    public string Description;
    public Sprite Icon;
    public float Duration;

    public PopupData(
        string title,
        string description = "",
        Sprite icon = null,
        float duration = 2f)
    {
        Title = title;
        Description = description;
        Icon = icon;
        Duration = duration;
    }
}