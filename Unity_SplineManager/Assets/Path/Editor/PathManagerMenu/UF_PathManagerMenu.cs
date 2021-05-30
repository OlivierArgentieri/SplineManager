using Unity_Framework.Scripts.Path.PathManager;
using Unity_Framework.Scripts.Path.PathManager.PathAgent.PathAgentSettings;
using UnityEditor;
using UnityEngine;

namespace Unity_Framework.Scripts.Path.Editor.PathManagerMenu
{
    public class UF_PathManagerMenu
    {
        #region custom methods
        [MenuItem("PathTool/PathManager", false, 1)]
        public static void Init()
        {
            UF_PathManager[] _spawnerManagers = Object.FindObjectsOfType<UF_PathManager>();

            if (_spawnerManagers.Length > 0) return;
        
            GameObject _pathManager = new GameObject("PathManager", typeof(UF_PathManager));
            
        }

        [MenuItem("PathTool/Assets/New Agent Settings")]
        public static void CreateNewAgentProfile()
        {
            UF_PathAgentSettings _profile = ScriptableObject.CreateInstance<UF_PathAgentSettings>();
            string _name = AssetDatabase.GenerateUniqueAssetPath("Assets/Unity_Framework/Scripts/Path/PathAssets/PathSettingsObject/NewPathSettings.asset");
            AssetDatabase.CreateAsset(_profile, _name);
            AssetDatabase.SaveAssets();
            
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = _profile;
        }
        #endregion
    }
}