using System;
using System.Collections.Generic;
using System.Linq;
using EditoolsUnity;
using Unity_Framework.Scripts.Path.PathManager.PathAgent;
using Unity_Framework.Scripts.Path.PathManager.PathMode;
using UnityEngine;

namespace Unity_Framework.Scripts.Path.PathManager
{
    public class UF_PathManager : MonoBehaviour
    {
        #region f/p
        public static event Action OnInit = null; 
        // todo refletcions
        public List<UF_PathModeSelector> Paths = new List<UF_PathModeSelector>();
        public List<UF_PathAgent> Agents = new List<UF_PathAgent>();

        public bool IsEmpty => Paths != null && Paths.Count <1;
        #endregion

        
        #region unity methods

        private void Awake() 
        {
            for (int i = 0; i < Agents.Count; i++)
            {
                if (!Agents[i].IsValid) continue;
                
                // GameObject _temp = Instantiate(Agents[i].AgentToMove);
                GameObject _temp = Agents[i].AgentToMove;
                UF_AgentFollowCurve _script = _temp.AddComponent<UF_AgentFollowCurve>();
                _script.agentSetting = Agents[i].AgentSettings;/*
                _script.SpeedMove = Agents[i].AgentSettings.SpeedMove;
                _script.SpeedRotation = Agents[i].AgentSettings.SpeedRotation;*/
                _script.CurrentPath = Paths.FirstOrDefault(p => p.Mode.Id == Agents[i].PathId);
            }
        }

        private void Start()
        {
            OnInit?.Invoke();
        }
        
        #endregion
        
        #region cutstom methods
        public void AddPath() => Paths.Add(new UF_PathModeSelector());
        public void RemovePath(int _index) => Paths.RemoveAt(_index);
        public void ClearPath() => Paths.Clear(); 
        
        public void AddAgent() => Agents.Add(null);
        public void RemoveAgent(int _index) => Agents.RemoveAt(_index);
        public void ClearAgents() => Agents.Clear();
        #endregion

        #region debug

        // to show path always
        
        private void OnDrawGizmos()
        {
            for (int i = 0; i < Paths.Count; i++)
            {
                Gizmos.color = Color.white;
                Paths[i].Mode.DrawGizmosMode();
            }
        }
        #endregion
    }
}