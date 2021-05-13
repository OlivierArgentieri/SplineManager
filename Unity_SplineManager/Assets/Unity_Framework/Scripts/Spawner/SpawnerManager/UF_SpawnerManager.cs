using System.Collections.Generic;
using Unity_Framework.Scripts.Spawner.SpawnerManager.SpawnPoint;
using Unity_Framework.Scripts.Spawner.SpawnerManager.SpawnTrigger;
using UnityEngine;

namespace Unity_Framework.Scripts.Spawner.SpawnerManager
{
    public class UF_SpawnerManager : MonoBehaviour
    {
        #region f/p
      
        [SerializeField]
        private List<UF_SpawnPoint> spawnPoints = new List<UF_SpawnPoint>();
        
        [SerializeField]
        private UF_SpawnTrigger triggerZonePrefab = null;
        #endregion

        
        #region unity methods
        
        private void Start()
        {
            SpawnAll();
        }
        #endregion


        #region custom methods
        void SpawnAll()
        {
            if (!triggerZonePrefab) return;
        
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                UF_SpawnPoint _point = spawnPoints[i];
                
                UF_SpawnTrigger _trigger = Instantiate(triggerZonePrefab);
                _trigger.transform.localScale = _point.Size;
                
                if(_trigger) _trigger.SetData(spawnPoints[i]);
            }
        }
        public void AddPoint() => spawnPoints.Add(new UF_SpawnPoint());
        public void Remove(int _index) => spawnPoints.RemoveAt(_index);
        public void Clear() => spawnPoints.Clear();
        
        #endregion
    }
}