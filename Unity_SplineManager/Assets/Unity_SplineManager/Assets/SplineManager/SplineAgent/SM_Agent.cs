using System;
using System.Collections.Generic;
using Unity_SplineManager.Assets.SplineManager.Curve;
using UnityEngine;

// todo to profile : Scriptable Object
namespace Unity_SplineManager.Assets.SplineManager.Agent
{
    [Serializable]
    public class SM_Agent
    {
        #region f/p
        public int MaxSpeedMove = 100;
        public int MinSpeedMove = 0;
        public int SpeedMove = 0;

        public int MaxSpeedRotation = 500;
        public int MinSpeedRotation = 0;
        public int SpeedRotation = 0;

        public bool Show = true;
        private List<SM_Curve> curves = null;

        public int CurveID; // todo select
        public GameObject AgentToMove = null;
        public int CurveLength => curves[CurveID]?.CurveLength ?? 0;
        public int CurveDefinition => curves[CurveID]?.CurveDefinition ?? 0;
        public int CurrentPercent = 0;
        private float StartPosition => ((float) CurrentPercent / 100) * CurveDefinition * CurveLength;
        #endregion
    }
}