using System;
using System.Collections.Generic;
using System.Linq;
using EditoolsUnity;
using Unity_Framework.Scripts.Path.PathManager.Path;
using Unity_Framework.Scripts.Path.PathManager.PathAgent;
using UnityEngine;

namespace Unity_Framework.Scripts.Path.PathManager
{
    public class UF_PathManager : MonoBehaviour
    {
        #region f/p
        public static event Action OnInit = null; 
        // todo refletcions
        public List<UF_Path> Paths = new List<UF_Path>();
        public List<UF_PathAgent> Agents = new List<UF_PathAgent>();

        public bool IsEmpty => Paths != null && Paths.Count <1;
        #endregion

        
        #region unity methods

        private void Awake()
        {
            for (int i = 0; i < Agents.Count; i++)
            {
                GameObject _temp = Instantiate(Agents[i].AgentToMove);
                UF_AgentFollowCurve _script = _temp.AddComponent<UF_AgentFollowCurve>();
                _script.SpeedMove = Agents[i].SpeedMove;
                _script.SpeedRotation = Agents[i].SpeedRotation;
                _script.CurrentPath = Paths.FirstOrDefault(p => p.PathMode.Mode.Id == Agents[i].PathId);
            }
        }

        private void Start()
        {
            OnInit?.Invoke();
        }
        
        #endregion
        
        #region cutstom methods
        public void AddPath() => Paths.Add(new UF_Path());
        public void RemovePath(int _index) => Paths.RemoveAt(_index);
        public void ClearPath() => Paths.Clear();
        
        public void AddAgent() => Agents.Add(null);
        public void RemoveAgent(int _index) => Agents.RemoveAt(_index);
        public void ClearAgents() => Agents.Clear();
        #endregion
    }
}