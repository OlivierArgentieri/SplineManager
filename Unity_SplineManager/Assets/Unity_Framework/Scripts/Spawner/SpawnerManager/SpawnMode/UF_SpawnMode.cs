using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity_Framework.Scripts.Spawner.SpawnerManager.SpawnMode
{
    [Serializable]
    public abstract class UF_SpawnMode
    {
        #region f/p
        
        public Vector3 Position = Vector3.zero;
        public bool AutoDestroyAgent = false;
        public float AutoDestroyDelay = 0;
        
        #endregion



        #region custom methods
        public abstract void Spawn(GameObject _agent);
        public abstract void Spawn(List<GameObject> _agent);
        public abstract void SpawnWithDestroyDelay(GameObject _agent);
        public abstract void SpawnWithDestroyDelay(List<GameObject> _agents);
        
        #if UNITY_EDITOR
            public abstract void DrawSceneMode();
            public abstract void DrawSettings();
            public abstract void DrawLinkToSpawner(Vector3 Position);
        #endif
        
        #endregion
    }
}