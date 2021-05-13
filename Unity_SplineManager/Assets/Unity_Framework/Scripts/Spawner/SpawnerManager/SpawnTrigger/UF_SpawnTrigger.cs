using System;
using Unity_Framework.Scripts.Import.Util;
using Unity_Framework.Scripts.Spawner.SpawnerManager.SpawnPoint;
using Unity_Framework.Scripts.Spawner.SpawnerManager.SpawnMode;
using UnityEditor;
using UnityEngine;

namespace Unity_Framework.Scripts.Spawner.SpawnerManager.SpawnTrigger
{
    [Serializable]
    public class UF_SpawnTrigger : MonoBehaviour
    {
        #region f/p
        [SerializeField] private BoxCollider triggerZone = null;
        [SerializeField] private UF_SpawnPoint data = null;
        public bool Triggered { get; set; } = false;
        #endregion



        #region unity methods
        private void Start()
        {
            if (data == null || data.UseTrigger || Triggered) return;
            StartCoroutine(Util.DelayedCallback(data.SpawnDelay, TriggerSpawn));
        }

        private void OnTriggerEnter()
        {
            if (data == null || !data.UseTrigger || Triggered) return;
            StartCoroutine(Util.DelayedCallback(data.SpawnDelay, TriggerSpawn));
        }
        #endregion
        
        
        
        #region custom methods
        void TriggerSpawn()
        {
            if (data == null || Triggered) return;
        
            for (int i = 0; i < data.SpawnModes.Count; i++)
            {
                UF_SpawnModeSelector _mode = data.SpawnModes[i];
            
                if(data.IsMonoAgent && _mode.Mode.AutoDestroyAgent)
                    _mode.Mode.SpawnWithDestroyDelay(data.MonoAgent);
                else if(_mode.Mode.AutoDestroyAgent)
                    _mode.Mode.SpawnWithDestroyDelay(data.Agents);
            
                else if(data.IsMonoAgent)
                    _mode.Mode.Spawn(data.MonoAgent);
                else
                    _mode.Mode.Spawn(data.Agents);
            }
            Triggered = true;
        }
        
        public void SetData(UF_SpawnPoint _data)
        {
            data = _data;
            transform.position = _data.Position;
            if (triggerZone) triggerZone.size = data.Size;
        }
        #endregion
        
        
#if UNITY_EDITOR
        #region debug

        private void OnDrawGizmos()
        {
            DebugTriggerBox();
        }

        private void DebugTriggerBox()
        {
            if (!data.UseTrigger) return;
            Handles.color = Color.green;
            Handles.DrawWireCube(transform.position, transform.localScale);
            Handles.color = Color.white;
        }
        #endregion
#endif
    }
    
    
}