using Unity_Framework.Scripts._3C.Character.CharacterComponent;
using UnityEditor;
using UnityEngine;

namespace Unity_Framework.Scripts._3C.Character.Editor.CharacterComponent
{
    [CustomEditor(typeof(UF_CharacterComponent))]
    public class UF_CharacterEditor : UnityEditor.Editor
    {
        #region init
        private static UF_CharacterComponent eTarget = null;
        private void OnEnable()
        {
            eTarget = (UF_CharacterComponent) target;
        
        }
        #endregion


        #region f/p

        private UF_CharacterBehaviourType previousBehavourType;

        #endregion
    
        #region editor methods

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            InitCharacterBehaviour();
        }

        #endregion
    

        #region custom methods

        void InitCharacterBehaviour()
        {
            if (Application.isPlaying) return;
            if (eTarget.BehaviourType.ToString() != previousBehavourType.ToString())
            {
                previousBehavourType = eTarget.BehaviourType;
                eTarget.InitBehaviour();
            }
            //if(EditorGUIUtility.isProSkin)
            Color backgroundColor = EditorGUIUtility.isProSkin ? new Color32(56, 56, 56, 255) : new Color32(194, 194, 194, 255);
        }
    

        #endregion
    
    }
}