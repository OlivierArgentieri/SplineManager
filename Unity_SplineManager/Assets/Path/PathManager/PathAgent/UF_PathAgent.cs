using System;
using System.Collections.Generic;
using Unity_Framework.Scripts.Path.PathManager.PathAgent.PathAgentSettings;
using UnityEngine;

namespace Unity_Framework.Scripts.Path.PathManager.PathAgent
{
    [Serializable]
    public class UF_PathAgent
    {
        #region f/p
        public UF_PathAgentSettings AgentSettings;
        public GameObject AgentToMove = null;
        
        public bool IsValid => AgentSettings && AgentToMove;

        public bool Show = true;
        private List<Vector3> pathPoints = null;

        public string PathId; // todo select
        public int PathIndex = 0;
        public int PathLength => pathPoints?.Count ?? 0 ;
        
        #endregion
    }
}