using UnityEngine;

public class MainMenuBGObject : MonoBehaviour
{
    public void OnEndGlitch()
    {
        GetComponent<Animator>().SetFloat("state", 0f); // normal
        // audio
    }
}
