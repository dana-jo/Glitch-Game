using UnityEngine;
using UnityEngine.InputSystem;
namespace UnityEngine.InputSystem.Samples.RebindUI
{
    public class MovementSelector : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference m_MoveAction;
        [SerializeField]
        private RebindSaveLoad m_SaveLoad;
        private void Start()
        {
            UseArrowKeys();
        }
        public void UseArrowKeys()
        {
            ApplyScheme(
                up: "<Keyboard>/upArrow",
                down: "<Keyboard>/downArrow",
                left: "<Keyboard>/leftArrow",
                right: "<Keyboard>/rightArrow");
        }
        public void UseWASD()
        {
            ApplyScheme(
                up: "<Keyboard>/w",
                down: "<Keyboard>/s",
                left: "<Keyboard>/a",
                right: "<Keyboard>/d");
        }
        private void ApplyScheme(string up, string down, string left, string right)
        {
            var action = m_MoveAction != null ? m_MoveAction.action : null;
            if (action == null)
            {
                Debug.LogWarning("MovementSchemeSelector: Move action reference is not set.", this);
                return;
            }
            for (var i = 0; i < action.bindings.Count; i++)
            {
                var binding = action.bindings[i];
                if (!binding.isPartOfComposite)
                    continue;
                switch (binding.name.ToLowerInvariant())
                {
                    case "up":
                        action.ApplyBindingOverride(i, up);
                        break;
                    case "down":
                        action.ApplyBindingOverride(i, down);
                        break;
                    case "left":
                        action.ApplyBindingOverride(i, left);
                        break;
                    case "right":
                        action.ApplyBindingOverride(i, right);
                        break;
                }
            }
            if (m_SaveLoad != null)
                m_SaveLoad.Save();
        }
    }
}