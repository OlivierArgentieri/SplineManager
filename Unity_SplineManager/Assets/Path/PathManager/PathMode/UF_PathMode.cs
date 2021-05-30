using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity_Framework.Scripts.Path.PathManager.PathMode
{
    [Serializable]
    public abstract class UF_PathMode
    {
        #region f/p

        public string Id = "Path 1";

        public bool ShowPath = true;
        public bool ShowPoint = true;
        
        public Color PathColor = Color.white;
        
        public abstract List<Vector3> PathPoints { get; }
        public abstract Vector3 StartPercentPosition { get; }
        public abstract int GetStartPercentIndex { get; }

        public Vector3 Position = Vector3.zero;
        
        #endregion


        #region custom methods
        
        #if UNITY_EDITOR
        public abstract void DrawSceneMode();
        public abstract void DrawGizmosMode();
        public abstract void DrawSettings();
        #endif

        #endregion
    }
}