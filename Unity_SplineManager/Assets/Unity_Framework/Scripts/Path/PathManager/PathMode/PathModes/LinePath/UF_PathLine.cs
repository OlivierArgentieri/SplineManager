using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unity_Framework.Scripts.Path.PathManager.PathMode.PathModes.LinePath
{
    [Serializable]
    public class UF_PathLine
    {
        #region f/p
        // todo reflection
        public bool ShowPath = true;
        public bool ShowPoint = true;
       public bool IsEditable = false;
        
        public List<Vector3> PathPoints = new List<Vector3>();

        #endregion


        #region unity methods
        
        #endregion


        #region custom methods

        public void AddPoint()
        {
            int _count = PathPoints.Count;
            if (_count < 1)
                PathPoints.Add(Vector3.zero);
            else
                PathPoints.Add(PathPoints.Last() + Vector3.forward);
        }

        public void RemovePoint(int _index) => PathPoints.RemoveAt(_index);
        public void ClearPoints() => PathPoints.Clear();

        #endregion
    }
}