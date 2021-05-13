using Unity_Framework.Scripts._3C.Camera.CameraManager;
using Unity_Framework.Scripts._3C.Input.Editor.InputManager;
using UnityEditor;
using UnityEngine;

namespace Unity_Framework.Scripts._3C.Camera.Editor.CameraManager
{
    public class UF_CameraManagerEditor
    {
        #region custom methods
        [MenuItem("UF/Camera/CameraManager")]
        public static void Init()
        {
            UF_CameraManager[] _cameraManagers = Object.FindObjectsOfType<UF_CameraManager>();

            if (_cameraManagers.Length > 0) return;
        
            GameObject _cameraManager = new GameObject("CameraManager", typeof(UF_CameraManager));
            Selection.activeObject = _cameraManager;
        
            // create InputManager 
            UF_InputManagerEditor.Init();
        }
        #endregion

    }
}