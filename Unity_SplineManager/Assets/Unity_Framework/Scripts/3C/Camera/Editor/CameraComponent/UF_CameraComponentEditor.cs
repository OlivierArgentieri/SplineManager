using Unity_Framework.Scripts._3C.Camera.CameraComponents;
using UnityEditor;
using UnityEngine;

namespace Unity_Framework.Scripts._3C.Camera.Editor.CameraComponent
{
    [CustomEditor(typeof(UF_CameraComponent))]
    public class UF_CameraComponentEditor : UnityEditor.Editor
    {
        #region f/p

        private CameraTypes previousCameraType;

        #endregion
        private static UF_CameraComponent eTarget = null;

        #region unity methods

        private void OnEnable()
        {
            eTarget = (UF_CameraComponent) target;
        }
    
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            InitCameraBehaviour();
        
        }
    
        #endregion


        #region custom methods
    

        void InitCameraBehaviour()
        {
            if (Application.isPlaying) return;
            if (eTarget.CameraType.ToString() != previousCameraType.ToString())
            {
                previousCameraType = eTarget.CameraType;
                eTarget.InitBehaviour();
            }
            //if(EditorGUIUtility.isProSkin)
            Color backgroundColor = EditorGUIUtility.isProSkin ? new Color32(56, 56, 56, 255) : new Color32(194, 194, 194, 255);
        }
    

        #endregion
    }
}