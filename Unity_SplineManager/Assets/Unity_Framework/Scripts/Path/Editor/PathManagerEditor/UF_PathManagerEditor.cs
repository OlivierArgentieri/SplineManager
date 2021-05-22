using System;
using System.Linq;
using System.Reflection;
using EditoolsUnity;
using Unity_Framework.Scripts.Path.PathManager;
using Unity_Framework.Scripts.Path.PathManager.PathAgent.PathAgentSettings;
using Unity_Framework.Scripts.Path.PathManager.PathMode;
using UnityEditor;
using UnityEngine;

namespace Unity_Framework.Scripts.Path.Editor.PathManagerEditor
{
    [CustomEditor(typeof(UF_PathManager))]
    public class UF_PathManagerEditor : EditorCustom<UF_PathManager>
    {
        #region const

        private static readonly Version version = new Version(1, 2, 0);

        private const BindingFlags reflectionFlags =
            BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

        #endregion


        #region UI Methods

        protected override void OnEnable()
        {
            base.OnEnable();
            Tools.current = Tool.None;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GlobalSettings();
        }

        private void OnSceneGUI()
        {
            DrawPathOnScene();
        }

        #endregion


        #region custom methods

        private void GlobalSettings()
        {
            EditoolsLayout.Horizontal(true);
            EditoolsBox.HelpBoxInfo($"PATH TOOL V{version}");
            EditoolsLayout.Horizontal(false);

            EditoolsLayout.Horizontal(true);
            EditoolsButton.ButtonWithConfirm("Remove all Path", Color.red, eTarget.ClearPath, "Clear All Paths ?",
                $"Are you sure", "Yes", "No", _showCondition: !eTarget.IsEmpty);
            EditoolsButton.Button("Add Path", Color.green, eTarget.AddPath);
            EditoolsLayout.Horizontal(false);

            EditorGUILayout.Space(8);
            AllPathUI();
            DrawnAgentUI();
        }

        private void AllPathUI()
        {
            if (!eTarget) return;

            for (int i = 0; i < eTarget.Paths.Count; i++)
            {
                UF_PathModeSelector _p = eTarget.Paths[i];
                UF_PathMode _pathMethod = _p.Mode;
                EditoolsLayout.Foldout(ref _pathMethod.ShowPath, $"Show/Hide {_pathMethod.Id}", true);

                if (!_pathMethod.ShowPath) continue;

                EditoolsBox.HelpBox($"[{i}] {_pathMethod.Id} -> {_pathMethod.PathPoints.Count} total points");


                EditoolsLayout.Horizontal(true);

                EditoolsButton.ButtonWithConfirm("Remove This Path", Color.red, eTarget.RemovePath, i,
                    $"Suppress Path {i + 1} ? ", "Are your sure ?");

                UF_PathModeSelector _mode = _p;
                _mode.Type = (UF_PathType) EditoolsField.EnumPopup("Mode Type", _mode.Type);
                EditoolsLayout.Horizontal(false);

                _mode.Mode.DrawSettings();

                EditoolsLayout.Space(5);
            }
        }

        void DrawnAgentUI()
        {
            if (!eTarget) return;

            EditoolsLayout.Horizontal(true);
            EditoolsBox.HelpBoxInfo("Agents Settings");
            EditoolsLayout.Vertical(true);
            EditoolsButton.ButtonWithConfirm("Remove all Agents", Color.red, eTarget.ClearAgents, "Clear All Agents ?",
                $"Clear All Agents", "Are your sure ?", _showCondition: eTarget.Agents.Count > 0);
            EditoolsButton.Button("Add Agent", Color.green, eTarget.AddAgent);
            EditoolsLayout.Vertical(false);
            EditoolsLayout.Horizontal(false);


            for (int i = 0; i < eTarget.Agents.Count; i++)
            {
                if (eTarget.Agents[i] == null) return;
                UF_PathAgent _agent = eTarget.Agents[i];
                EditoolsLayout.Foldout(ref _agent.Show, $"{i + 1} / {eTarget.Agents.Count}");

                if (!_agent.Show) continue;

                EditoolsLayout.Horizontal(true);
                EditoolsBox.HelpBox($"{i + 1} / {eTarget.Agents.Count}");
                EditoolsButton.ButtonWithConfirm("-", Color.red, eTarget.RemoveAgent, i, $"Remove Agent {i}",
                    "Are your sure ?");
                EditoolsLayout.Horizontal(false);

                // EditoolsField.IntSlider("Speed Move", ref _agent.SpeedMove, _agent.MinSpeedMove, _agent.MaxSpeedMove); // todo remove
                // EditoolsField.IntSlider("Speed Rotation", ref _agent.SpeedRotation, _agent.MinSpeedRotation _agent.MaxSpeedRotation); // todo remove
                
                EditoolsLayout.Horizontal(true);
                _agent.AgentToMove =
                    (GameObject) EditoolsField.ObjectField(_agent.AgentToMove, typeof(GameObject), true);
                // todo input for UF_PAthAgentSettings
                _agent.AgentSettings =
                    (UF_PathAgentSettings) EditoolsField.ObjectField(_agent.AgentSettings, typeof(UF_PathAgentSettings),
                        false);
                EditoolsButton.Button("New Profile", Color.white, PathManagerMenu.UF_PathManagerMenu.CreateNewAgentProfile, !_agent.AgentSettings);
                EditoolsLayout.Horizontal(false);

                if (eTarget.Paths.Count > 0)
                {
                    string[] _pathsNames = eTarget.Paths.Select(o => o.Mode.Id).ToArray();
                    _agent.PathIndex = EditorGUILayout.Popup("Paths target", _agent.PathIndex, _pathsNames);
                    _agent.PathId = _pathsNames[_agent.PathIndex];
                }
                else
                    EditoolsBox.HelpBox("NO PATH FOUND !", MessageType.Error);
            }
        }


        private void DrawPathOnScene()
        {
            for (int i = 0; i < eTarget.Paths.Count; i++)
            {
                UF_PathModeSelector _point = eTarget.Paths[i];

                _point.Mode.DrawSceneMode();
            }
        }

        #endregion
    }
}