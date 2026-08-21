using System.Collections;
using UnityEngine;

public class StartGameSave : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null;

        SaveController.Instance.LoadGame(); 
    }
}
