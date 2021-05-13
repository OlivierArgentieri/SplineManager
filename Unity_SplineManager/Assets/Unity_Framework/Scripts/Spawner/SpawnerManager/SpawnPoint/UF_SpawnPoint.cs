using System;
using System.Collections.Generic;
using Unity_Framework.Scripts.Spawner.SpawnerManager.SpawnMode;
using UnityEngine;

namespace Unity_Framework.Scripts.Spawner.SpawnerManager.SpawnPoint
{
    [Serializable]
    public class UF_SpawnPoint
    {
        #region f/p
        public bool IsVisible = true;
        public List<UF_SpawnModeSelector> SpawnModes = new List<UF_SpawnModeSelector>();

        public bool IsMonoAgent = false;
        public GameObject MonoAgent = null;
        public List<GameObject> Agents = new List<GameObject>();

        public Vector3 Position = Vector3.zero;
        public Vector3 Size = Vector3.one;
        public bool UseTrigger = true;
        public bool UseDelay = false;
        public float SpawnDelay = 0;
        #endregion
        

        #region custom methods 
        public void AddAgent() => Agents.Add(null);
        public void RemoveAgent(int _index) => Agents.RemoveAt(_index);
        public void RemoveAgent() => MonoAgent = null;
        public void ClearAgents() => Agents.Clear();
        public void AddMode() => SpawnModes.Add(new UF_SpawnModeSelector());
        public void RemoveMode(int _index) => SpawnModes.RemoveAt(_index);
        public void ClearModes() => SpawnModes.Clear();
        #endregion
    }
}