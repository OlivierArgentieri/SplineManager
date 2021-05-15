using System.Linq;
using EditoolsUnity;
using Unity_SplineManager.Assets.SplineManager.Agent;
using Unity_SplineManager.Assets.SplineManager.Curve;
using UnityEditor;
using UnityEngine;

namespace Unity_SplineManager.Assets.SplineManager.Editor
{
    [CustomEditor(typeof(SM_SplineManager))]
    public class SM_SplineManagerEditor : EditorCustom<SM_SplineManager>
    {
        private int selectIndex = -1;

        /*public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            UIManager();


            SceneView.RepaintAll();
        }

        private void OnSceneGUI()
        {
            if (GUI.changed)
                SceneView.RepaintAll();

            SceneCurves();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            Tools.current = Tool.None;
            for (int i = 0; i < eTarget.Curves.Count; i++)
            {
                eTarget.Curves[i].SetCurve();
            }
        }*/

        #region GUI methods

        private void UIManager()
        {
            EditoolsLayout.Horizontal(true);
            EditoolsBox.HelpBox("Curve Manager", MessageType.Info);
            EditoolsLayout.Vertical(true);

            EditoolsButton.Button("Add Curve", Color.green, eTarget.AddCurve);
            if (eTarget.Curves.Count > 0)
                EditoolsButton.ButtonWithConfirm("Clear", Color.red, eTarget.ClearCurves, "Clear Curves",
                    "Are you sure ?");
            EditoolsLayout.Vertical(false);
            EditoolsLayout.Horizontal(false);
            EditoolsLayout.Space(2);
            UICurve();


            EditoolsLayout.Horizontal(true);
            EditoolsBox.HelpBox("Agents", MessageType.Info);
            EditoolsLayout.Vertical(true);
            EditoolsButton.Button("Add Agents", Color.green, eTarget.AddAgent);
            if (eTarget.Agents.Count > 0)
                EditoolsButton.ButtonWithConfirm("Clear", Color.red, eTarget.ClearAgent, "Clear Agents",
                    "Are you sure ?");
            EditoolsLayout.Vertical(false);
            EditoolsLayout.Horizontal(false);
            UIAgent();
        }

        private void UIAgent()
        {
            if (!eTarget) return;

            for (int i = 0; i < eTarget.Agents.Count; i++)
            {
                SM_Agent _agent = eTarget.Agents[i];

                EditoolsLayout.Foldout(ref _agent.Show, $"{i + 1} / {eTarget.Agents.Count}");

                if (!_agent.Show) continue;

                EditoolsLayout.Horizontal(true);
                EditoolsBox.HelpBox($"{i + 1} / {eTarget.Agents.Count}");
                EditoolsButton.ButtonWithConfirm("-", Color.red, eTarget.RemoveAgent, i, $"Remove Agent {i}",
                    "Are your sure ?");
                EditoolsLayout.Horizontal(false);

                EditoolsField.IntSlider("Speed Move", ref _agent.SpeedMove, _agent.MinSpeedMove, _agent.MaxSpeedMove);
                EditoolsField.IntSlider("Speed Rotation", ref _agent.SpeedRotation, _agent.MinSpeedRotation,
                    _agent.MaxSpeedRotation);
                _agent.AgentToMove =
                    (GameObject) EditoolsField.ObjectField(_agent.AgentToMove, typeof(GameObject), true);

                if (eTarget.Curves.Count > 0)
                    _agent.CurveID = EditorGUILayout.Popup("Curves target", _agent.CurveID,
                        eTarget.Curves.Select(o => eTarget.Curves.IndexOf(o).ToString()).ToArray());
                else
                    EditoolsBox.HelpBox("NO CURVE FOUND !", MessageType.Error);
            }
        }

        private void UICurve()
        {
            if (!eTarget) return;
            for (int i = 0; i < eTarget.Curves.Count; i++)
            {
                SM_Curve _c = eTarget.Curves[i];
                EditoolsLayout.Foldout(ref _c.ShowCurve, $"Show/Hide {i}");

                if (!_c.ShowCurve) continue;
                EditoolsLayout.Horizontal(true);
                EditoolsBox.HelpBox($"{i} / {eTarget.Curves.Count}");
                EditoolsLayout.Vertical(true);
                EditoolsButton.ButtonWithConfirm("-", Color.red, eTarget.RemoveCurve, i, $"Remove Curve {i}",
                    "Are your sure ?");
                EditoolsButton.Button("+", Color.green, _c.AddSegment);
                EditoolsLayout.Vertical(false);
                EditoolsLayout.Horizontal(false);

                EditoolsField.IntSlider(" Curve Definition", ref _c.CurveDefinition, _c.MinPrecision, _c.MaxDefinition);
                EditoolsField.IntSlider(" Start at percent ", ref _c.CurrentPercent, 0, 100);
                if (GUI.changed)
                {
                    _c.SetCurve();
                    SceneView.RepaintAll();
                }

                EditoolsLayout.Foldout(ref _c.ShowSegments, "Show/Hide Segments");

                //UISegment(_c);
                EditoolsLayout.Space(2);
            }
        }


        private void SceneCurves()
        {
            if (!eTarget) return;
            for (int i = 0; i < eTarget.Curves.Count; i++)
            {
                SM_Curve _c = eTarget.Curves[i];

                SceneSegment(_c);
            }
        }


        private void SceneSegment(SM_Curve _c)
        {
            /*Vector3 _lastAnchor = _c.Anchor[_c.Anchor.Length - 1];
            _lastAnchor = Handles.PositionHandle(_lastAnchor, Quaternion.identity);
            _c.Anchor[_c.Anchor.Length - 1] = _lastAnchor;*/

            for (int j = 0; j < _c.Anchor.Length; j += 3)
            {
                Vector3 _handleA = _c.Anchor[j];
                Vector3 _handleB = _c.Anchor[j + 1];
                Vector3 _handleC = _c.Anchor[j + 2];

                Handles.DrawLine(_handleA, _handleA + Vector3.up * .05f);
                Handles.DrawLine(_handleB, _handleB + Vector3.up * .05f);
                Handles.DrawLine(_handleC, _handleC + Vector3.up * .05f);

                float _sizeA = HandleUtility.GetHandleSize(_handleA);
                float _sizeB = HandleUtility.GetHandleSize(_handleB);
                float _sizeC = HandleUtility.GetHandleSize(_handleC);

                bool _pressA = Handles.Button(_handleA + Vector3.up * .05f, Quaternion.identity, .05f * _sizeA,
                    .05f * _sizeA, Handles.DotHandleCap);
                bool _pressB = Handles.Button(_handleB + Vector3.up * .05f, Quaternion.identity, .05f * _sizeB,
                    .05f * _sizeB, Handles.DotHandleCap);
                bool _pressC = Handles.Button(_handleC + Vector3.up * .05f, Quaternion.identity, .05f * _sizeC,
                    .05f * _sizeC, Handles.DotHandleCap);
                
                if (_pressA)
                    selectIndex = j;
                if (_pressB)
                    selectIndex = j + 1;
                if (_pressC)
                    selectIndex = j + 2;

                if (selectIndex == j)
                {
                    _handleA = Handles.PositionHandle(_handleA, Quaternion.identity);
                    if (GUI.changed)
                        _c.SetCurve();
                }

                if (selectIndex == j + 1)
                {
                    _handleB = Handles.PositionHandle(_handleB, Quaternion.identity);
                    if (GUI.changed)
                        _c.SetCurve();
                }

                if (selectIndex == j + 2)
                {
                    _handleC = Handles.PositionHandle(_handleC, Quaternion.identity);
                    if (GUI.changed)
                        _c.SetCurve();
                }

                _c.Anchor[j] = _handleA;
                _c.Anchor[j + 1] = _handleB;
                _c.Anchor[j + 2] = _handleC;
                EditoolsHandle.DrawDottedLine(_handleA, _handleB, 1);
                EditoolsHandle.DrawDottedLine(_handleB, _handleC, 1);
            }

            Vector3[] _curve = _c.Curve;
            for (int j = 0; j < _c.Curve.Length; j++)
            {
                if (j < _curve.Length - 1)
                    Handles.DrawLine(_curve[j], _curve[j + 1]);
            }

            Handles.color = Color.white;
            EditoolsHandle.DrawDottedLine(_c.Curve[_c.GetStartAtPercent], _c.Curve[_c.GetStartAtPercent] + Vector3.up,
                1);
        }

        #endregion
    }
}