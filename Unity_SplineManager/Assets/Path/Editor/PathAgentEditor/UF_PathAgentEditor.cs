using EditoolsUnity;
using Unity_Framework.Scripts.Path.PathManager.PathAgent.PathAgentSettings;
using UnityEditor;
using UnityEngine;

namespace Unity_Framework.Scripts.Path.Editor.PathAgentEditor
{
    [CustomEditor(typeof(UF_PathAgentSettings), true)]
    public class UF_PathAgentEditor : UnityEditor.Editor // not with editool cause target it' s not GameObject
    {
        
        #region f/p

        private UF_PathAgentSettings targetSettings;
        #endregion
        
        
        #region UI Methods
        
        protected void OnEnable()
        {
            targetSettings = (UF_PathAgentSettings)target;
            Tools.current = Tool.None;
        }

        public override void OnInspectorGUI()
        {
            EditoolsLayout.Horizontal(true);
            EditoolsBox.HelpBoxInfo("Agents Settings");
            EditoolsLayout.Horizontal(false);
            
            EditoolsLayout.Space(2);
            
            EditoolsLayout.Horizontal(true);
            targetSettings.SpeedMove = EditoolsField.FloatField("Move Speed", targetSettings.SpeedMove);
            EditoolsLayout.Horizontal(false);
            
            EditoolsLayout.Horizontal(true);
            targetSettings.SpeedRotation = EditoolsField.FloatField("Rotation Speed", targetSettings.SpeedRotation);
            EditoolsLayout.Horizontal(false);
            
            
            EditoolsLayout.Horizontal(true);
            EditoolsField.Toggle("Add LookAt ?", ref targetSettings.UseLookAt);
            EditoolsLayout.Horizontal(false);
            EditoolsLayout.Horizontal(true);
            if (targetSettings.UseLookAt)
            {
                EditoolsBox.HelpBox("LookAt Target");
                targetSettings.TargetLookAt = (GameObject) EditoolsField.ObjectField(targetSettings.TargetLookAt, typeof(GameObject), true);
            }
            EditoolsLayout.Horizontal(false);
            
            
            EditorUtility.SetDirty(targetSettings); // flush value
        }
        #endregion
    }
}