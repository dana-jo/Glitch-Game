using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Samples.RebindUI;
public class ResetAllBindings : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputAction;
    [SerializeField]
    private MovementSelector movementSelector;
    public void ResetBindings()
    {
        foreach (InputActionMap map in inputAction.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }
        PlayerPrefs.DeleteKey("rebinds");
        if (movementSelector != null)
        {
            movementSelector.UseArrowKeys();
        }
    }
}
