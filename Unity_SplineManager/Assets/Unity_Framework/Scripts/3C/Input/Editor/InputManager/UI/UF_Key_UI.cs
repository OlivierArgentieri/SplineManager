using Unity_Framework.Scripts._3C.Input.InputManager;
using UnityEditor;
using UnityEngine;

namespace Unity_Framework.Scripts._3C.Input.Editor.InputManager.UI
{
    [CustomPropertyDrawer(typeof(UF_Key))]
    public class UF_Key_UI : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        
        
            // Don't make child fields be indented
            var _indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // rects
            Rect _rectTextBox = new Rect(position.x, position.y, 50, position.height);
            Rect _rectKeys = new Rect(position.x + 60, position.y, 100, position.height);
        
            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(_rectKeys, property.FindPropertyRelative("Key"), GUIContent.none);
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(_rectTextBox, property.FindPropertyRelative("Label"), GUIContent.none);

            if (EditorGUI.EndChangeCheck())
                property.FindPropertyRelative("Key").enumValueIndex = FindPositionByLabel(property.FindPropertyRelative("Label").stringValue);
        
            // Set indent back to what it was
            EditorGUI.indentLevel = _indent;

            EditorGUI.EndProperty();
        }
    
    
        #region custom methods
        private int FindPositionByLabel(string _label)
        {
            string[] _keys = System.Enum.GetNames(typeof(KeyCode));

            for(int i =0; i< _keys.Length; i++)
            {
                if (_label.Length == 1 && _keys[i].ToUpper().Equals(_label.ToUpper()))
                    return i;
                if (_label.Length > 1 && _keys[i].ToUpper().StartsWith(_label.ToUpper()))
                {
                    Debug.Log($"{i} {_keys[i]} {_label}");
                    return i;
                }
            }
            return 0;
        }
        #endregion
    }
}