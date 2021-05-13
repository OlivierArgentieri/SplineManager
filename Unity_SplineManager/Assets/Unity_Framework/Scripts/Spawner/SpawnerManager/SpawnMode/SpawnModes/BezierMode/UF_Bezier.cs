using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity_Framework.Scripts.Spawner.SpawnerManager.SpawnMode.SpawnModes.BezierMode
{
    [Serializable]
    public class UF_Bezier
    {
        #region f/p
        public bool DipslaySegments = false;
        
        public int CurveDefinition = 1;
        public int MaxDefinition = 100;
        public int MinDefinition = 1;

        public Color CurveColor = Color.white;

        public List<Vector3> Anchor = new List<Vector3>()
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 2),
        };

        [SerializeField] Vector3[] curvePoints = new Vector3[] { };
        
        public Vector3[] CurvePoints => curvePoints;
        
        public int CurrentPercent = 1;
        public int GetStartAtPercent => (int) ((float) CurrentPercent / 100 * (CurvePoints.Length - 1));

        public Vector3 StartPercentPosition => CurvePoints[GetStartAtPercent];
        public bool IsEmpty => Anchor.Count <1;
        #endregion


        #region custom methods

        public void SetCurve() => curvePoints = ComputeCurve(Anchor, CurveDefinition);


        public void AddSegment()
        {
            if (IsEmpty)
            {
                ResetCurve();
                return;
            }
            
            Vector3 _lastPoint = Anchor[Anchor.Count - 1];

            Anchor.Add(_lastPoint + Vector3.forward * 1);
            Anchor.Add(_lastPoint + Vector3.forward * 2);
            Anchor.Add(_lastPoint + Vector3.forward * 3);
            
            SetCurve();
        }

        public void ResetCurve()
        {
            Anchor = new List<Vector3>()
            {
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 2),
            };
         
            SetCurve();
        }

        public void RemoveSegment(int _index)
        {
            if (_index < 0 || _index >= Anchor.Count) return;
            
            Anchor.RemoveAt(_index + 2);
            Anchor.RemoveAt(_index + 1);
            Anchor.RemoveAt(_index);
            
            SetCurve();
        }

        Vector3[] ComputeCurve(List<Vector3> _anchors, int _definition)
        {
            Vector3[] _curve = new Vector3[_definition * (_anchors.Count / 3)];
            int _curveIndex = 0;
            for (int i = _curveIndex; i < _anchors.Count; i += 3)
            {
                Vector3 _a = _anchors[i];
                Vector3 _b = _anchors[i + 1];
                Vector3 _c = _anchors[i + 2];

                for (int j = 0; j < _definition; j++)
                {
                    float _t = (float) j / _definition;
                    Vector3 _firstPart = Vector3.Lerp(_a, _b, _t);
                    Vector3 _secondPart = Vector3.Lerp(_b, _c, _t);
                    _curve[_curveIndex] = Vector3.Lerp(_firstPart, _secondPart, _t);
                    _curveIndex++;
                }

                if (i > 1)
                    _anchors[i - 1] = _anchors[i]; // link curves
                _curve[_curveIndex - 1] = _c;
            }

            return _curve;
        }

        #endregion
        
    }
}