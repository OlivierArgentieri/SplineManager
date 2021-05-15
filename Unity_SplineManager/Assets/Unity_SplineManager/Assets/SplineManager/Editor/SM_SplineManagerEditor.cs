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
     public override void OnInspectorGUI()
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
    }

    #region GUI methods
    private void UIManager()
    {
        EditoolsLayout.Horizontal(true);
        EditoolsBox.HelpBox("Curve Manager", MessageType.Info);
            EditoolsLayout.Vertical(true);
           
            EditoolsButton.Button("Add Curve", Color.green, eTarget.AddCurve);
            if(eTarget.Curves.Count > 0)
                EditoolsButton.ButtonWithConfirm("Clear", Color.red, eTarget.ClearCurves, "Clear Curves", "Are you sure ?");
            EditoolsLayout.Vertical(false);
        EditoolsLayout.Horizontal(false);
        EditoolsLayout.Space(2);
        UICurve();

        
        EditoolsLayout.Horizontal(true);
            EditoolsBox.HelpBox("Agents", MessageType.Info);
            EditoolsLayout.Vertical(true);
            EditoolsButton.Button("Add Agents", Color.green, eTarget.AddAgent);
                if(eTarget.Agents.Count > 0)
                     EditoolsButton.ButtonWithConfirm("Clear", Color.red, eTarget.ClearAgent, "Clear Agents", "Are you sure ?");
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

            EditoolsLayout.Foldout(ref _agent.Show, $"{i+1} / {eTarget.Agents.Count}");
            
            if(!_agent.Show) continue;
            
            EditoolsLayout.Horizontal(true);
            EditoolsBox.HelpBox($"{i+1} / {eTarget.Agents.Count}");
            EditoolsButton.ButtonWithConfirm("-", Color.red, eTarget.RemoveAgent, i, $"Remove Agent {i}", "Are your sure ?");
            EditoolsLayout.Horizontal(false);
            
            EditoolsField.IntSlider("Speed Move", ref _agent.SpeedMove, _agent.MinSpeedMove, _agent.MaxSpeedMove);
            EditoolsField.IntSlider("Speed Rotation", ref _agent.SpeedRotation, _agent.MinSpeedRotation, _agent.MaxSpeedRotation);
            _agent.AgentToMove = (GameObject) EditoolsField.ObjectField(_agent.AgentToMove, typeof(GameObject), true);
            
            if(eTarget.Curves.Count > 0)
                _agent.CurveID = EditorGUILayout.Popup("Curves target", _agent.CurveID,eTarget.Curves.Select(o => eTarget.Curves.IndexOf(o).ToString()).ToArray());
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
            
            if(!_c.ShowCurve) continue;
            EditoolsLayout.Horizontal(true);
                EditoolsBox.HelpBox($"{i} / {eTarget.Curves.Count}");
                EditoolsLayout.Vertical(true);
                    EditoolsButton.ButtonWithConfirm("-", Color.red, eTarget.RemoveCurve, i, $"Remove Curve {i}", "Are your sure ?");
                    EditoolsButton.Button("+", Color.green, _c.AddSegment);
                EditoolsLayout.Vertical(false);
            EditoolsLayout.Horizontal(false);
            
            /*
            _c.Segments[0].BeginPoint = EditoolsField.Vector3Field("Begin Point",  _c.Segments[0].BeginPoint);
            _c.Segments[0].MiddlePoint = EditoolsField.Vector3Field("Middle Point",  _c.Segments[0].MiddlePoint);
            _c.Segments[0].EndPoint = EditoolsField.Vector3Field("End Point",  _c.Segments[0].EndPoint);*/
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

    private void UISegment(SM_Curve _curve)
    {
       
        /*
        if (_curve.Segments.Count <1 ) return;
        _curve.Segments[0].StartPoint = _curve.EndPoint;

        EditoolsLayout.Horizontal(true);
        EditoolsBox.HelpBox("Segment [0]");
        EditoolsField.ColorField(_curve.Segments[0].SegementColor, ref _curve.Segments[0].SegementColor);
        EditoolsButton.ButtonWithConfirm("#", Color.red, _curve.RemoveSegment, 0, $"Remove segment {0}", "Are your sure ?");
        EditoolsLayout.Horizontal(false);

        _curve.Segments[0].StartPoint = EditoolsField.Vector3Field($"Start point 0", _curve.Segments[0].StartPoint);
        _curve.Segments[0].MiddlePoint = EditoolsField.Vector3Field($"Middle point 0", _curve.Segments[0].MiddlePoint);
        _curve.Segments[0].EndPoint = EditoolsField.Vector3Field($"End point 0", _curve.Segments[0].EndPoint);
        */
        
        if (_curve.CurveLength <2 ) return;

        for (int i = 1; i < _curve.CurveLength; i++)
        {
            //CM_Segment _segment = _curve.Curve[i];
            EditoolsLayout.Horizontal(true);

            EditoolsBox.HelpBox($"Segment [{i}]");
            EditoolsField.ColorField(_curve.CurveColor, ref _curve.CurveColor);
            //EditoolsButton.ButtonWithConfirm("-", Color.red, _curve.RemoveSegment, i, $"Remove segment {0}", "Are your sure ?");
            EditoolsLayout.Horizontal(false);

            //_segment.BeginPoint = _curve.Segments[i - 1].EndPoint;
            if (!_curve.ShowSegments) continue;
           // _segment.BeginPoint = EditoolsField.Vector3Field($"begin point {i}", _segment.BeginPoint);
            //_segment.EndPoint = EditoolsField.Vector3Field($"end point {i}", _segment.EndPoint);
           // _segment.MiddlePoint = EditoolsField.Vector3Field($"middle point {i}", _segment.MiddlePoint);

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
        #region correction
        Vector3 _lastAnchor = _c.Anchor[_c.Anchor.Length - 1];
        _lastAnchor = Handles.PositionHandle(_lastAnchor, Quaternion.identity);
        _c.Anchor[_c.Anchor.Length - 1] = _lastAnchor;
        
        for (int j = 0; j < _c.Anchor.Length; j+=3)
        {
            Vector3 _handleA = _c.Anchor[j];
            Vector3 _handleB = _c.Anchor[j+1];
            Vector3 _handleC = _c.Anchor[j+2];

            Handles.DrawLine(_handleA, _handleA + Vector3.up * .5f);
            Handles.DrawLine(_handleB, _handleB + Vector3.up * .5f);
            
            float _sizeA = HandleUtility.GetHandleSize(_handleA);
            float _sizeB = HandleUtility.GetHandleSize(_handleB);

            bool _pressA = Handles.Button(_handleA + Vector3.up * .05f, Quaternion.identity, .05f * _sizeA, .05f * _sizeA, Handles.DotHandleCap);
            bool _pressB = Handles.Button(_handleB + Vector3.up * .05f, Quaternion.identity, .05f * _sizeB, .05f * _sizeB, Handles.DotHandleCap);

            if (_pressA)
                selectIndex = j;
            else if (_pressB)
                selectIndex = j + 1;

            if (selectIndex == j)
            {
                _handleA = Handles.PositionHandle(_handleA, Quaternion.identity);
                if(GUI.changed)
                    _c.SetCurve();
            }
            
            
            else if (selectIndex == j + 1)
            {
                _handleB = Handles.PositionHandle(_handleB, Quaternion.identity);
                if(GUI.changed)
                    _c.SetCurve();
            }

            _c.Anchor[j] = _handleA;
            _c.Anchor[j+1] = _handleB;
            EditoolsHandle.DrawDottedLine(_handleA, _handleB, 1);
            EditoolsHandle.DrawDottedLine(_handleB, _handleC, 1);
        }

        Vector3[] _curve = _c.Curve;
        for (int j = 0; j < _c.Curve.Length ; j++)
        {
            if(j< _curve.Length-1)
                Handles.DrawLine(_curve[j], _curve[j+1]);
        }
        
        Handles.color = Color.white;
        EditoolsHandle.DrawDottedLine(_c.Curve[_c.GetStartAtPercent], _c.Curve[_c.GetStartAtPercent] + Vector3.up, 1);

        
        #endregion
    }
    /*
    private void SceneSegment(CM_Curve _c)
    {
        if (_c.Segments.Count < 1) return;
        _c.Segments[0].BeginPoint = EditoolsHandle.PositionHandle(_c.Segments[0].BeginPoint, Quaternion.identity);

        
        for (int i = 0; i < _c.Segments.Count; i++)
        {
            CM_Segment _s = _c.Segments[i];
            EditoolsHandle.SetColor(_s.SegmentColor);
            _s.EndPoint = EditoolsHandle.PositionHandle(_s.EndPoint, Quaternion.identity);
            _s.MiddlePoint = EditoolsHandle.PositionHandle(_s.MiddlePoint, Quaternion.identity);
            
            EditoolsHandle.DrawDottedLine(_s.BeginPoint, _s.MiddlePoint, 1);
            EditoolsHandle.DrawDottedLine(_s.EndPoint, _s.MiddlePoint, 1);

            SceneDisplaySegmentPoint(_s, _c);
        }
        EditoolsHandle.SetColor(Color.white);

    }*/
    
    /*private void SceneDisplayCurvePoint(Curve _c)
    {
        Vector3[] _curve = eTarget.ComputeCurve(_c.BeginPoint, _c.MiddlePoint, _c.EndPoint, _c.CurveDefinition);

        for (int i = 0; i <_curve.Length; i++)
        { 
            Handles.SphereHandleCap(0, _curve[i], Quaternion.identity, .2f, EventType.Repaint);
            
            EditoolsHandle.DrawDottedLine(_c.BeginPoint, _c.MiddlePoint, 1);
            EditoolsHandle.DrawDottedLine(_c.EndPoint, _c.MiddlePoint, 1);
            if(i< _curve.Length -1)
                Handles.DrawLine(_curve[i], _curve[i+1]);
        }
    }*/
    /*
    private void SceneDisplaySegmentPoint(CM_Segment _s, CM_Curve _c)
    {
        Vector3[] _curve = eTarget.ComputeCurve(_s.BeginPoint, _s.MiddlePoint, _s.EndPoint, _c.CurveDefinition);
        
        EditoolsHandle.DrawDottedLine(_s.BeginPoint, _s.MiddlePoint, 1);
        EditoolsHandle.DrawDottedLine(_s.EndPoint, _s.MiddlePoint, 1);
        for (int i = 0; i <_curve.Length; i++)
        { 
            Handles.SphereHandleCap(0, _curve[i], Quaternion.identity, .2f, EventType.Repaint);
            if(i< _curve.Length -1)
                Handles.DrawLine(_curve[i], _curve[i+1]);
        }
    }
    */
    #endregion
    }
}