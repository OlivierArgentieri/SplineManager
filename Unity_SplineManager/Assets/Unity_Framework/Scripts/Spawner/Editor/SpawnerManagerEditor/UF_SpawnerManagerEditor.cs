using System;
using System.Collections.Generic;
using EditoolsUnity;
using Unity_Framework.Scripts.Import.Util;
using Unity_Framework.Scripts.Spawner.SpawnerManager;
using Unity_Framework.Scripts.Spawner.SpawnerManager.SpawnMode;
using Unity_Framework.Scripts.Spawner.SpawnerManager.SpawnPoint;
using Unity_Framework.Scripts.Spawner.SpawnerManager.SpawnTrigger;
using UnityEditor;
using UnityEngine;

namespace Unity_Framework.Scripts.Spawner.Editor.SpawnerManagerEditor
{
    [CustomEditor(typeof(UF_SpawnerManager))]
    public class UF_SpawnerManagerEditor : EditorCustom<UF_SpawnerManager>
    {
        #region const

        private const string triggerSpawnAssetName = "SpawnCollider/BoxCollider";
        private static readonly Version version = new Version(1, 3, 0);

        #endregion


        #region reflections
        private UF_SpawnTrigger TriggerZonePrefab
        {
            get => (UF_SpawnTrigger) Util.GetField("triggerZonePrefab", eTarget).GetValue(eTarget);
            set => Util.GetField("triggerZonePrefab", eTarget).SetValue(eTarget, value);
        }

        private List<UF_SpawnPoint> SpawnPoints
        {
            get => (List<UF_SpawnPoint>) Util.GetField("spawnPoints", eTarget).GetValue(eTarget);
            set => Util.GetField("spawnPoints", eTarget).SetValue(eTarget, value);
        }
        #endregion


        #region unity methods

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!TriggerZonePrefab)
            {
                UF_SpawnTrigger _triggerAsset = Resources.Load<UF_SpawnTrigger>(triggerSpawnAssetName);
                if (_triggerAsset)
                    TriggerZonePrefab = _triggerAsset;
            }

            Tools.current = Tool.None;
        }

        public override void OnInspectorGUI()
        {
            EditoolsBox.HelpBoxInfo($"SPAWN TOOL V{version}");
            TriggerZonePrefab =
                (UF_SpawnTrigger) EditoolsField.ObjectField(TriggerZonePrefab, typeof(UF_SpawnTrigger), false);
            if (!TriggerZonePrefab) return;

            EditoolsLayout.Space(1);
            DrawnSpawnPointsUI();

            SceneView.RepaintAll();
        }

        private void OnSceneGUI()
        {
            if (!TriggerZonePrefab) return;

            DrawSpawnPointScene();
        }

        #endregion


        #region UI methods

        void DrawnSpawnPointsUI()
        {
            EditoolsLayout.Horizontal(true);
            EditoolsBox.HelpBoxInfo("Add Spawn Point");

            EditoolsLayout.Vertical(true);
            EditoolsButton.Button("+", Color.green, eTarget.AddPoint);
            EditoolsButton.ButtonWithConfirm("Clear Points", Color.red, eTarget.Clear, "Remove All ?",
                "Remove All Point ?", _showCondition: SpawnPoints.Count > 0);
            EditoolsLayout.Vertical(false);

            EditoolsLayout.Horizontal(false);


            EditoolsLayout.Space(2);

            for (int i = 0; i < SpawnPoints.Count; i++)
            {
                EditoolsLayout.Horizontal(true);
                EditoolsBox.HelpBox($"SpawnPoint {i + 1}");
                EditoolsButton.ButtonWithConfirm("X", Color.red, eTarget.Remove, i, "Remove Point ?",
                    "Remove This Point ?");

                EditoolsLayout.Horizontal(false);

                if (i > SpawnPoints.Count - 1) return;

                UF_SpawnPoint _point = SpawnPoints[i];
                EditoolsLayout.Foldout(ref _point.IsVisible, "Show/Hide");

                if (!_point.IsVisible) continue;

                EditoolsField.Vector3Field("Position", ref _point.Position);


                EditoolsField.Toggle("Use Delay ?", ref _point.UseDelay);
                if (_point.UseDelay)
                    _point.SpawnDelay = EditorGUILayout.Slider("Spawn Delay", _point.SpawnDelay, 0, 15);
                else
                    _point.SpawnDelay = 0;

                EditoolsField.Toggle("Use Trigger ?", ref _point.UseTrigger);
                if (_point.UseTrigger)
                    EditoolsField.Vector3Field("Size", ref _point.Size);
                else
                    _point.Size = Vector3.one;

                EditoolsLayout.Space(2);

                DrawSpawnModeUI(_point);
                DrawnAgentUI(_point);
            }
        }

        void DrawnAgentUI(UF_SpawnPoint _point)
        {
            EditoolsField.Toggle("Unique Agent? ", ref _point.IsMonoAgent);
            EditoolsLayout.Space(5);

            if (_point.IsMonoAgent)
            {
                EditoolsLayout.Horizontal(true);
                _point.MonoAgent = (GameObject) EditoolsField.ObjectField(_point.MonoAgent, typeof(GameObject), false);
                EditoolsButton.ButtonWithConfirm("X", Color.red, _point.RemoveAgent, "Remove Agent", "Remove Agent?");
                EditoolsLayout.Horizontal(false);
            }
            else
            {
                EditoolsLayout.Horizontal(true);
                EditoolsBox.HelpBoxInfo($"Add agent to spawn");
                EditoolsLayout.Vertical(true);
                EditoolsButton.Button("Add Agent", Color.cyan, _point.AddAgent);
                EditoolsButton.ButtonWithConfirm("Clear", Color.red, _point.ClearAgents, "Clear Agents",
                    "Clear All Agents ?",
                    _showCondition: _point.Agents.Count > 0);
                EditoolsLayout.Vertical(false);
                EditoolsLayout.Horizontal(false);

                for (int j = 0; j < _point.Agents.Count; j++)
                {
                    EditoolsLayout.Horizontal(true);
                    _point.Agents[j] =
                        (GameObject) EditoolsField.ObjectField(_point.Agents[j], typeof(GameObject), false);
                    EditoolsButton.ButtonWithConfirm("X", Color.red, _point.RemoveAgent, j, "Remove Agent ?",
                        "Remove This Agent ?");
                    EditoolsLayout.Horizontal(false);
                }
            }
        }

        void DrawModeSettingsUI(UF_SpawnModeSelector _mode)
        {
            _mode.Mode.DrawSettings();
        }

        void DrawSpawnModeUI(UF_SpawnPoint _point)
        {
            EditoolsLayout.Horizontal(true);
            EditoolsBox.HelpBoxInfo("Add Spawn Mode");

            EditoolsLayout.Vertical(true);
            EditoolsButton.Button("+", Color.green, _point.AddMode);
            EditoolsButton.ButtonWithConfirm("Clear all", Color.red, _point.ClearModes, "Remove All ?",
                "Remove All Mode ?", _showCondition: _point.SpawnModes.Count > 0);
            EditoolsLayout.Vertical(false);

            EditoolsLayout.Horizontal(false);
            for (int i = 0; i < _point.SpawnModes.Count; i++)
            {
                UF_SpawnModeSelector _mode = _point.SpawnModes[i];

                EditoolsLayout.Horizontal(true);
                _mode.Type = (UF_SpawnType) EditoolsField.EnumPopup("Mode Type", _mode.Type);
                EditoolsButton.ButtonWithConfirm("X", Color.red, _point.RemoveMode, i, "Remove Mode ?",
                    "Remove This Mode ?");
                EditoolsLayout.Horizontal(false);
                DrawModeSettingsUI(_mode);
                EditoolsLayout.Space(2);
            }
        }

        #endregion


        #region scene methods

        void DrawSpawnPointScene()
        {
            for (int i = 0; i < SpawnPoints.Count; i++)
            {
                UF_SpawnPoint _point = SpawnPoints[i];

                EditoolsHandle.SetColor(Color.green);
                if (_point.UseTrigger)
                    EditoolsHandle.DrawWireCube(_point.Position, _point.Size);
                EditoolsHandle.SetColor(Color.white);

                EditoolsHandle.PositionHandle(ref _point.Position, Quaternion.identity);
                if (_point.UseTrigger)
                    EditoolsHandle.ScaleHandle(ref _point.Size, _point.Position, Quaternion.identity, 2);

                EditoolsLayout.Space();

                GetModeScene(_point);
            }
        }


        void GetModeScene(UF_SpawnPoint _point)
        {
            for (int i = 0; i < _point.SpawnModes.Count; i++)
            {
                UF_SpawnModeSelector _mode = _point.SpawnModes[i];
                DrawModeScene(_mode, _point);
            }
        }

        void DrawModeScene(UF_SpawnModeSelector _mode, UF_SpawnPoint _point)
        {
            _mode.Mode.DrawLinkToSpawner(_point.Position);
            _mode.Mode.DrawSceneMode();
        }

        #endregion
    }
}