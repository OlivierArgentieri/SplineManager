using Unity_Framework.Scripts._3C.Character.CharacterManager;
using Unity_Framework.Scripts._3C.Input.Editor.InputManager;
using UnityEditor;
using UnityEngine;

namespace Unity_Framework.Scripts._3C.Character.Editor.CharacterManager
{
    public class UF_CharacterManagerEditor
    {
        #region custom methods
    

        [MenuItem("UF/Character/CharacterManager")]
        public static void Init()
        {
            UF_CharacterManager[] _characterManagers = Object.FindObjectsOfType<UF_CharacterManager>();

            if (_characterManagers.Length > 0) return;
        
            GameObject _characterManager = new GameObject("CharacterManager", typeof(UF_CharacterManager));
            Selection.activeObject = _characterManager;
        
        
        
            // create InputManager 
            UF_InputManagerEditor.Init();
        }
        #endregion

    }
}