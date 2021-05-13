using Unity_Framework.Scripts.Path.PathManager;
using Unity_Framework.Scripts.Spawner.SpawnerManager;
using UnityEditor;
using UnityEngine;

namespace Unity_Framework.Scripts.Path.Editor.PathManagerMenu
{
    public class UF_PathManagerMenu
    {
        #region custom methods
        [MenuItem("UF/PathTool/PathManager", false, 1)]
        public static void Init()
        {
            UF_PathManager[] _spawnerManagers = Object.FindObjectsOfType<UF_PathManager>();

            if (_spawnerManagers.Length > 0) return;
        
            GameObject _pathManager = new GameObject("PathManager", typeof(UF_PathManager));
            
        }
        #endregion
    }
}