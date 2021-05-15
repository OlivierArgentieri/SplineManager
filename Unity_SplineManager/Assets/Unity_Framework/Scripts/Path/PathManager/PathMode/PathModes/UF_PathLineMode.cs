using System;
using System.Collections.Generic;
using EditoolsUnity;
using Unity_Framework.Scripts.Path.PathManager.PathMode.PathModes.LinePath;
using UnityEngine;

namespace Unity_Framework.Scripts.Path.PathManager.PathMode.PathModes
{
    [Serializable]
    public class UF_PathLineMode : UF_PathMode
    {
        #region f/p

        public UF_PathLine Path = new UF_PathLine();
        
        [SerializeField]
        private int currentPercent = 1;

        public override List<Vector3> PathPoints => Path.PathPoints;
        
        public override Vector3 StartPercentPosition => PathPoints[GetStartPercentIndex];
        public override int GetStartPercentIndex => (int) ((float) currentPercent / 100 * (PathPoints.Count - 1));
        #endregion

        
        
        #region UI Methods

#if UNITY_EDITOR
        public override void DrawSceneMode()
        {
            ShowAllPathScene();
        }

        public override void DrawSettings()
        {
            EditoolsLayout.Space(3);

            EditoolsLayout.Horizontal(true);
            EditoolsBox.HelpBox($"Path ID : {Id}");
            EditoolsField.TextField("", ref Id);
            EditoolsLayout.Horizontal(false);
            
            
            EditoolsLayout.Horizontal(true);
            EditoolsBox.HelpBoxInfo("Line Settings");
            EditoolsLayout.Vertical(true);
            EditoolsButton.ButtonWithConfirm("Remove All Points", Color.red, Path.ClearPoints, $"Suppress All Points ? ", "Are your sure ?", "Yes", "No", Path.PathPoints.Count > 0);
            EditoolsButton.Button("Add Point", Color.green, Path.AddPoint);
            EditoolsLayout.Vertical(false);
            EditoolsLayout.Horizontal(false);


            EditoolsLayout.Horizontal(true);
            EditoolsBox.HelpBox("Path Color");
            EditoolsField.ColorField(PathColor, ref PathColor);
            EditoolsLayout.Horizontal(false);
            //  EditoolsButton.Button("Editable", Path.IsEditable ? Color.green : Color.grey, SetEditable, Path);

            // New Line


            ShowPathPointUi(Path);
            EditoolsField.IntSlider("Start at percent ", ref currentPercent, 0, 100);
        }
        
        public override void DrawGizmosMode()
        {
           DrawGizmos();
        }
        
        private void ShowPathPointUi(UF_PathLine _path)
        {
            if (_path.PathPoints.Count == 0) return;
            EditoolsLayout.Foldout(ref _path.ShowPoint, $"Show/Hide points {Id}", true);
            if (_path.ShowPoint)
            {
                for (int i = 0; i < _path.PathPoints.Count; i++)
                {
                    EditoolsLayout.Horizontal(true);
                    EditoolsButton.ButtonWithConfirm("-", Color.red, _path.RemovePoint, i,$"Suppress Point {i + 1} at {_path.PathPoints[i]} ? ", "Are your sure ?");
                    EditoolsBox.HelpBox($"[{i + 1} / {_path.PathPoints.Count}]");
                    _path.PathPoints[i] = EditoolsField.Vector3Field("", _path.PathPoints[i]);
                    
                    EditoolsLayout.Horizontal(false);
                }
            }
        }

        private void ShowAllPathScene()
        {
            if (PathPoints.Count < 1) return;
            EditoolsHandle.SetColor(PathColor);

            for (int j = 0; j < Path.PathPoints.Count; j++)
            {
                Path.PathPoints[j] = EditoolsHandle.PositionHandle(Path.PathPoints[j], Quaternion.identity);
                EditoolsHandle.Label(Path.PathPoints[j] + Vector3.up * 5, $"Point {j + 1}");
                EditoolsHandle.SetColor(Color.white);
                EditoolsHandle.DrawDottedLine(Path.PathPoints[j] + Vector3.up * 5, Path.PathPoints[j], .5f);
                EditoolsHandle.SetColor(PathColor);
                EditoolsHandle.DrawSolidDisc(Path.PathPoints[j], Vector3.up, 0.1f);
                if (j < Path.PathPoints.Count - 1)
                    EditoolsHandle.DrawLine(Path.PathPoints[j], Path.PathPoints[j + 1]);
            }

            if (GetStartPercentIndex < 0 || GetStartPercentIndex > PathPoints.Count) return;
            EditoolsHandle.DrawDottedLine(PathPoints[GetStartPercentIndex], PathPoints[GetStartPercentIndex] + Vector3.up, 1);
            EditoolsHandle.Label(PathPoints[GetStartPercentIndex] + Vector3.up, $"Spawn Mark");
        }


        private void DrawGizmos()
        {
            if (PathPoints.Count < 1) return;
            Gizmos.color = PathColor;
            
            for (int j = 0; j < Path.PathPoints.Count; j++)
            {
                if (j < Path.PathPoints.Count - 1)
                    Gizmos.DrawLine(Path.PathPoints[j], Path.PathPoints[j + 1]);
            }

            Gizmos.color = Color.white;
        }
        /*void SetEditable(UF_Path _path)
        {
            for (int i = 0; i < .Count; i++)
            {
                eTarget.Paths[i].IsEditable = false;
            }

            _path.IsEditable = true;
        }*/
#endif
        #endregion
    }
}