using Unity_Framework.Scripts._3C.Input.InputManager;
using UnityEditor;
using UnityEngine;

namespace Unity_Framework.Scripts._3C.Input.Editor.InputManager
{
    public class UF_InputManagerEditor
    {
        #region custom methods

        [MenuItem("UF/Input/InputManager")]
        public static void Init()
        {
            UF_InputManager[] _inputManagers = Object.FindObjectsOfType<UF_InputManager>();

            if (_inputManagers.Length > 0) return;
        
            GameObject _inputManager = new GameObject("InputManager", typeof(UF_InputManager));
            Selection.activeGameObject = _inputManager;
        }
    

        #endregion
    }
}