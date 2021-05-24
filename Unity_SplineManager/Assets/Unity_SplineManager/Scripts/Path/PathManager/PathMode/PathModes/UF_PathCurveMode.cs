using System;
using System.Collections.Generic;
using System.Linq;
using EditoolsUnity;
using Unity_SplineManager.Scripts.Import.Interface;
using Unity_SplineManager.Scripts.Path.PathManager.PathMode.PathModes.Curve;
using UnityEditor;
using UnityEngine;

namespace Unity_SplineManager.Scripts.Path.PathManager.PathMode.PathModes
{
    [Serializable]
    public class UF_PathCurveMode : UF_PathMode, IIsValid
    {
        #region f/p

        [SerializeField] UF_PathCurve Curve = new UF_PathCurve();

        private float startAtPercent = 0;

        private int selectedIndex = -1;

        public override List<Vector3> PathPoints => Curve.CurvePoints.ToList();
        public override Vector3 StartPercentPosition => Curve.StartPercentPosition;
        public override int GetStartPercentIndex => Curve.GetStartPercentIndex;
        public bool IsValid => Curve != null;

        #endregion

        #region UI methods

#if UNITY_EDITOR
        
        public override void DrawSceneMode()
        {
            if (!IsValid || Curve.IsEmpty) return;
            Curve.SetCurve();
            
            for (int j = 0; j < Curve.Anchor.Count; j += 3)
            {
                Vector3 _handleA = Curve.Anchor[j];
                Vector3 _handleB = Curve.Anchor[j + 1];
                Vector3 _handleC = Curve.Anchor[j + 2];


                // markers
                Handles.DrawLine(_handleA, _handleA + Vector3.up * .5f);
                Handles.DrawLine(_handleB, _handleB + Vector3.up * .5f);
                Handles.DrawLine(_handleC, _handleC + Vector3.up * .5f);

                float _sizeA = HandleUtility.GetHandleSize(_handleA);
                float _sizeB = HandleUtility.GetHandleSize(_handleB);
                float _sizeC = HandleUtility.GetHandleSize(_handleC);

                // select Handle
                bool _pressA = Handles.Button(_handleA + Vector3.up * .05f, Quaternion.identity, .05f * _sizeA,
                    .05f * _sizeA, Handles.DotHandleCap);
                bool _pressB = Handles.Button(_handleB + Vector3.up * .05f, Quaternion.identity, .05f * _sizeB,
                    .05f * _sizeB, Handles.DotHandleCap);
                bool _pressC = Handles.Button(_handleC + Vector3.up * .05f, Quaternion.identity, .05f * _sizeC,
                    .05f * _sizeC, Handles.DotHandleCap);

                if (_pressA)
                    selectedIndex = j;
                if (_pressB)
                    selectedIndex = j + 1;
                if (_pressC)
                    selectedIndex = j + 2;
                
                if (selectedIndex == j) // if select first point segment
                {
                    _handleA = Handles.PositionHandle(_handleA, Quaternion.identity);
                }


                if (selectedIndex == j + 1) // if select middle segment curve
                {
                    _handleB = Handles.PositionHandle(_handleB, Quaternion.identity);
                }
                
                if (selectedIndex == j + 2) // if select last segment curve
                {
                    _handleC = Handles.PositionHandle(_handleC, Quaternion.identity);
                }

                Curve.Anchor[j] = _handleA;
                Curve.Anchor[j + 1] = _handleB;
                Curve.Anchor[j + 2] = _handleC;

                // draw linked middle point
                EditoolsHandle.DrawDottedLine(_handleA, _handleB, .5f);
                EditoolsHandle.DrawDottedLine(_handleB, _handleC, .5f);
            }

            // draw curve
            DrawCurveOnScene();

            // Draw Segment feedback
            DrawSegmentFeedbackOnScene();

            EditoolsHandle.DrawDottedLine(Curve.CurvePoints[Curve.GetStartPercentIndex], Curve.CurvePoints[Curve.GetStartPercentIndex] + Vector3.up*2 , 1);
            EditoolsHandle.Label(Curve.CurvePoints[Curve.GetStartPercentIndex] + Vector3.up*2, $"Spawn Mark");
        }

        public override void DrawGizmosMode()
        {
            DrawGizmos();
        }

        private void DrawCurveOnScene()
        {
            EditoolsHandle.SetColor(PathColor);

            Vector3 _currentPosition = Vector3.zero;
            for (int i = 0; i < Curve.CurvePoints.Length; i++)
            {
                _currentPosition = Curve.CurvePoints[i];
                if (i < Curve.CurvePoints.Length - 1)
                    Handles.DrawLine(_currentPosition, Curve.CurvePoints[i + 1]);
            }
            EditoolsHandle.SetColor(Color.white);
        }

        private void DrawGizmos()
        {
            Gizmos.color = PathColor;
            Vector3 _currentPosition = Vector3.zero;
            for (int i = 0; i < Curve.CurvePoints.Length; i++)
            {
                _currentPosition = Curve.CurvePoints[i];
                if (i < Curve.CurvePoints.Length - 1)
                    Gizmos.DrawLine(_currentPosition, Curve.CurvePoints[i + 1]); // gizmos make GameObject selectable
            }

            Gizmos.color = Color.white;
        }

        void DrawSegmentFeedbackOnScene()
        {
            Vector3 _currentPosition = Vector3.zero;
            for (int i = 0; i < Curve.Anchor.Count; i++)
            {
                if (i % 3 != 0) continue;
                _currentPosition = Curve.Anchor[i];
                EditoolsHandle.DrawDottedLine(_currentPosition, _currentPosition + Vector3.up * 1.5f, 1);
                EditoolsHandle.Label(_currentPosition + Vector3.up*1.5f, $"Segment : {i / 3}");
            }
        }

        public override void DrawSettings()
        {
            if (!IsValid) return;
            EditoolsLayout.Space(3);

            EditoolsLayout.Horizontal(true);
            EditoolsBox.HelpBox($"Path ID : {Id}");
            EditoolsField.TextField("", ref Id);
            EditoolsLayout.Horizontal(false);


            EditoolsLayout.Horizontal(true);
            EditoolsBox.HelpBoxInfo("Curve Settings");
            EditoolsLayout.Vertical(true);
            EditoolsButton.ButtonWithConfirm("Reset Curve", Color.red, Curve.ResetCurve, "Reset Curve ?",
                $"Remove Curve", "Are your sure ?", _showCondition: !Curve.IsEmpty);
            EditoolsButton.Button("Add Segment", Color.green, Curve.AddSegment);
            EditoolsLayout.Vertical(false);
            EditoolsLayout.Horizontal(false);


            EditoolsLayout.Horizontal(true);
            EditoolsBox.HelpBox("Curve Color");
            EditoolsField.ColorField(PathColor, ref PathColor);
            EditoolsLayout.Horizontal(false);


            EditoolsLayout.Space(2);

            EditoolsField.IntSlider("Start at percent ", ref Curve.CurrentPercent, 0, 100);
            EditoolsField.IntSlider("Curve Definition", ref Curve.CurveDefinition, Curve.MinDefinition,
                Curve.MaxDefinition);

            EditoolsLayout.Space(2);

            DisplaySegmentSettings();

            if (GUI.changed)
            {
                Curve.SetCurve();
                SceneView.RepaintAll();
            }
        }

        private void DisplaySegmentSettings()
        {
            if (!IsValid || Curve.IsEmpty) return;
            EditoolsLayout.Foldout(ref Curve.DipslaySegments, "Curve Segments");

            if (!Curve.DipslaySegments) return;


            for (int i = 0; i < Curve.Anchor.Count; i += 3)
            {
                EditoolsLayout.Horizontal(true);
                EditoolsButton.ButtonWithConfirm("X", Color.red, Curve.RemoveSegment, i, $"Remove {i / 3}",
                    $"Remove {i / 3}", "Are your sure ?");
                EditoolsBox.HelpBox($"Segment {i / 3} / {(Curve.Anchor.Count - 1) / 3} ");
                // todo edit anchor position
                EditoolsLayout.Horizontal(false);
            }
        }
#endif
        #endregion
    }
}