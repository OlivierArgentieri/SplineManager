using Unity_Framework.Scripts.Import.Util;
using Unity_Framework.Scripts.Spawner.SpawnerManager;
using Unity_Framework.Scripts.Spawner.SpawnerManager.SpawnTrigger;
using UnityEditor;
using UnityEngine;

namespace Unity_Framework.Scripts.Spawner.Editor.SpawnerManagerMenu
{
    public class UF_SpawnerManagerMenu
    {
        
        #region custom methods
        [MenuItem("UF/SpawnerTool/SpawnerManager", false, 1)]
        public static void Init()
        {
            UF_SpawnerManager[] _spawnerManagers = Object.FindObjectsOfType<UF_SpawnerManager>();

            if (_spawnerManagers.Length > 0) return;

            GameObject _spawnerManager = new GameObject("SpawnerManager", typeof(UF_SpawnerManager));

            TryInitHerself(_spawnerManager);
        }


        private static void TryInitHerself(GameObject _current)
        {
            if (!_current) return;

            UF_SpawnerManager _spawnerManagerScript = _current.GetComponent<UF_SpawnerManager>();
            if (!_spawnerManagerScript) return;

            GameObject _trigger = (GameObject) Resources.Load("SpawnerCollider/BoxCollider");
            if (!_trigger) return;
            
            UF_SpawnTrigger _triggerScript = _trigger.GetComponent<UF_SpawnTrigger>();
            if (!_triggerScript) return;
            
            Util.GetField("triggerZonePrefab",_spawnerManagerScript).SetValue(_spawnerManagerScript,_triggerScript);
        }
        #endregion
    }
}