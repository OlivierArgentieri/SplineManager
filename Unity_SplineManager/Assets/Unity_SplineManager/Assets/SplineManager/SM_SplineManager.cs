using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity_SplineManager.Assets.SplineManager.Agent;
using Unity_SplineManager.Assets.SplineManager.Curve;
using UnityEngine;

public class SM_SplineManager : MonoBehaviour
{
    #region f/p
    public static event Action<List<SM_Curve>> OnInit = null; 
    public List<SM_Agent> Agents = new(); 
    public List<SM_Curve> Curves = new(); 
    
    #endregion

    #region custom methods

    public void AddAgent() => Agents.Add(new SM_Agent());

    public void AddCurve()
    {
        Curves.Add(new SM_Curve());
        Curves.Last().SetCurve();
    }
    
    public void RemoveCurve(int _index) => Curves.RemoveAt(_index);
    public void RemoveAgent(int _index) => Agents.RemoveAt(_index);
    public void ClearCurves() => Curves.Clear();
    public void ClearAgent() => Agents.Clear();

    #endregion

    #region unity methods

    private void Awake()
    {
        for (int i = 0; i < Agents.Count; i++)
        {
            // GameObject _temp = Instantiate(Agents[i].AgentToMove);
            GameObject _temp = Agents[i].AgentToMove;
            SM_AgentFollowSpline _script = _temp.AddComponent<SM_AgentFollowSpline>();
            _script.SpeedMove = Agents[i].SpeedMove;
            //_script.SpeedRotation = Agents[i].SpeedRotation;
            _script.CurveID = Agents[i].CurveID;
        }
    }

    private void Start()
    {
        OnInit?.Invoke(Curves);
    }
    #endregion
    
    #region debug
    private void OnDrawGizmos()
    {
        for (int i = 0; i < Curves.Count; i++)
        {
            
            Gizmos.color = Color.white;
            Curves[i].DrawGizmos();
        }
    }
    #endregion
}
