using System;
using UnityEngine;

namespace Unity_SplineManager.Assets.SplineManager.Curve
{
    [Serializable]
    public class SM_Curve
    {
        #region f/p

        public bool ShowSegments = false;
        public bool ShowCurve = false;

        public int CurveDefinition = 1;
        public int MaxDefinition = 100;
        public int MinPrecision = 1;

        public Color CurveColor = Color.white;

        public Vector3[] Anchor = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 2),
        };

        [SerializeField] Vector3[] curvePoints = new Vector3[] { };

        public int CurveLength => Curve.Length;
        public Vector3[] Curve => curvePoints;
        public int CurrentPercent = 1;
        public int GetStartAtPercent => (int) ((float) CurrentPercent / 100 * (Curve.Length - 1));

        public Vector3 StartPercentPosition => Curve[GetStartAtPercent];

        #endregion


        #region custom methods

        public void SetCurve() => curvePoints = ComputeCurve(Anchor, CurveDefinition);

        public void AddSegment()
        {
            Vector3 _lastPoint = Anchor[Anchor.Length - 1];
            System.Array.Resize(ref Anchor, Anchor.Length + 3);

            int _index = 0;
            for (int i = Anchor.Length - 3; i < Anchor.Length; i++)
            {
                _index++;
                Vector3 _newPoint = _lastPoint + Vector3.forward * _index;
                Anchor[i] = _newPoint;
            }

            SetCurve();
        }

        Vector3[] ComputeCurve(Vector3[] _anchors, int _definition)
        {
            Vector3[] _curve = new Vector3[_definition * (_anchors.Length / 3)];
            int _curveIndex = 0;
            for (int i = _curveIndex; i < _anchors.Length; i += 3)
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

        public void DrawGizmos()
        {
            for (int i = 0; i < curvePoints.Length; i++)
            {
                if (i < curvePoints.Length - 1)
                    Gizmos.DrawLine(curvePoints[i], curvePoints[i + 1]);
            }
        }
        #endregion
    }
}