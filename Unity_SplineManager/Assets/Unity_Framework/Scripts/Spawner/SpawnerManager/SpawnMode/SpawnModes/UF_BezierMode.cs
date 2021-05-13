using System;
using System.Collections.Generic;
using EditoolsUnity;
using Unity_Framework.Scripts.Import.Interface;
using Unity_Framework.Scripts.Spawner.SpawnerManager.SpawnMode.SpawnModes.BezierMode;
using UnityEditor;
using UnityEngine;

namespace Unity_Framework.Scripts.Spawner.SpawnerManager.SpawnMode.SpawnModes
{
    [Serializable]
    public class UF_BezierMode : UF_SpawnMode, IIsValid
    {
        #region f/p

        [SerializeField] UF_Bezier Curve = new UF_Bezier();

        private int selectedIndex = -1;
        public bool IsValid => Curve != null;
        #endregion


        #region herited methods
        
        public override void Spawn(GameObject _agent)
        {
            int _step = Curve.CurvePoints.Length / Curve.CurveDefinition;

            for (int i = 0; i < Curve.CurvePoints.Length; i += _step)
                GameObject.Instantiate(_agent, Curve.CurvePoints[i], Quaternion.identity);
            
        }

        public override void Spawn(List<GameObject> _agents)
        {
            int _step = Curve.CurvePoints.Length / Curve.CurveDefinition;

            for (int i = 0; i < Curve.CurvePoints.Length; i += _step)
            {
                int _randomIndex = UnityEngine.Random.Range(0, _agents.Count);
                if (!_agents[_randomIndex]) continue;
                GameObject _go = GameObject.Instantiate(_agents[_randomIndex], Curve.CurvePoints[i], Quaternion.identity);
            }
        }

        public override void SpawnWithDestroyDelay(GameObject _agent)
        {
            int _step = Curve.CurvePoints.Length / Curve.CurveDefinition;

            for (int i = 0; i < Curve.CurvePoints.Length; i += _step)
            {
                GameObject _go = GameObject.Instantiate(_agent, Curve.CurvePoints[i], Quaternion.identity);
                GameObject.Destroy(_go, AutoDestroyDelay);
            }
        }

        public override void SpawnWithDestroyDelay(List<GameObject> _agents)
        {
            int _step = Curve.CurvePoints.Length / Curve.CurveDefinition;

            for (int i = 0; i < Curve.CurvePoints.Length; i += _step)
            {
                int _randomIndex = UnityEngine.Random.Range(0, _agents.Count);
                if (!_agents[_randomIndex]) continue;
                GameObject _go = GameObject.Instantiate(_agents[_randomIndex], Curve.CurvePoints[i], Quaternion.identity);
                GameObject.Destroy(_go, AutoDestroyDelay);
            }
        }

        #endregion

        
        #region UI methods
        
        #if UNITY_EDITOR
        
        public override void DrawSceneMode()
        {
            if (!IsValid || Curve.IsEmpty) return;
            Curve.SetCurve();

            
            Vector3 _lastAnchor = Curve.Anchor[Curve.Anchor.Count - 1];
            _lastAnchor = Handles.PositionHandle(_lastAnchor, Quaternion.identity);
            Curve.Anchor[Curve.Anchor.Count - 1] = _lastAnchor;

            for (int j = 0; j < Curve.Anchor.Count; j += 3)
            {
                Vector3 _handleA = Curve.Anchor[j];
                Vector3 _handleB = Curve.Anchor[j + 1];
                Vector3 _handleC = Curve.Anchor[j + 2];

                
                // markers
                Handles.DrawLine(_handleA, _handleA + Vector3.up * .5f);
                Handles.DrawLine(_handleB, _handleB + Vector3.up * .5f);

                float _sizeA = HandleUtility.GetHandleSize(_handleA);
                float _sizeB = HandleUtility.GetHandleSize(_handleB);

                // select Handle
                bool _pressA = Handles.Button(_handleA + Vector3.up * .05f, Quaternion.identity, .05f * _sizeA, .05f * _sizeA, Handles.DotHandleCap);
                bool _pressB = Handles.Button(_handleB + Vector3.up * .05f, Quaternion.identity, .05f * _sizeB, .05f * _sizeB, Handles.DotHandleCap);

                if (_pressA)
                    selectedIndex = j;
                else if (_pressB)
                    selectedIndex = j + 1;

                if (selectedIndex == j) // if select first point segment
                {
                    _handleA = Handles.PositionHandle(_handleA, Quaternion.identity);
                    if (GUI.changed)
                        Curve.SetCurve();
                }


                else if (selectedIndex == j + 1) // if select middle segment curve
                {
                    _handleB = Handles.PositionHandle(_handleB, Quaternion.identity);
                    if (GUI.changed)
                        Curve.SetCurve();
                }

                Curve.Anchor[j] = _handleA;
                Curve.Anchor[j + 1] = _handleB;
                
                // draw linked middle point
                EditoolsHandle.DrawDottedLine(_handleA, _handleB, .5f); 
                EditoolsHandle.DrawDottedLine(_handleB, _handleC, .5f);
            }

            // draw curve
            EditoolsHandle.SetColor(Curve.CurveColor);
            Vector3[] _curve = Curve.CurvePoints;
            for (int j = 0; j < Curve.CurvePoints.Length; j++)
            {
                if (j < Curve.CurvePoints.Length - 1)
                    Handles.DrawLine(Curve.CurvePoints[j], Curve.CurvePoints[j + 1]);
            }

            EditoolsHandle.SetColor(Color.white);
        }

        public override void DrawSettings()
        {
            if (!IsValid) return;
            EditoolsLayout.Space(3);
            EditoolsLayout.Horizontal(true);
            EditoolsBox.HelpBoxInfo("Curve Settings"); 
            
            EditoolsLayout.Vertical(true);
            if(!Curve.IsEmpty)
                EditoolsButton.ButtonWithConfirm("Reset Curve", Color.red, Curve.ResetCurve, "Reset Curve ?", $"Remove Curve", "Are your sure ?");
            EditoolsButton.Button("Add Segment", Color.green, Curve.AddSegment);
            EditoolsLayout.Vertical(false);
            EditoolsLayout.Horizontal(false);

            
            EditoolsLayout.Horizontal(true);
            EditoolsBox.HelpBox("Curve Color");
            EditoolsField.ColorField(Curve.CurveColor, ref Curve.CurveColor);
            EditoolsLayout.Horizontal(false);

            EditoolsLayout.Space(2);

            EditoolsField.IntSlider("Curve Definition", ref Curve.CurveDefinition, Curve.MinDefinition,
                Curve.MaxDefinition);
            
            EditoolsLayout.Space(2);
            
            DisplaySegmentSettings();
            
            if (GUI.changed)
            {
                Curve.SetCurve();
                SceneView.RepaintAll();
            }
            
            EditoolsField.Toggle("Auto Destroy Agents ?", ref AutoDestroyAgent);
            if (AutoDestroyAgent)
                AutoDestroyDelay = EditorGUILayout.Slider("Auto Destroy Delay", AutoDestroyDelay, 0, 15);

        }

        public override void DrawLinkToSpawner(Vector3 _position)
        {
            if (Curve.Anchor.Count < 1) return;
            Handles.DrawDottedLine(Curve.Anchor[0], _position, 0.5f);
        }


        private void DisplaySegmentSettings()
        {
            if (!IsValid || Curve.IsEmpty) return;
            EditoolsLayout.Foldout(ref Curve.DipslaySegments, "Curve Segments");

            if (!Curve.DipslaySegments) return;
            
            
            for (int i = 0; i < Curve.Anchor.Count; i+=3)
            {
                EditoolsLayout.Horizontal(true);
                EditoolsButton.ButtonWithConfirm("X", Color.red, Curve.RemoveSegment, i, $"Remove {i/3}", $"Remove {i/3}", "Are your sure ?");
                EditoolsBox.HelpBox($"Segment {i/3} / {(Curve.Anchor.Count-1)/3} ");
                //Curve.Anchor[i] = EditoolsField.Vector3Field("", Curve.Anchor[i]);
                EditoolsLayout.Horizontal(false);
            }
        }
#endif

        #endregion

    }
}