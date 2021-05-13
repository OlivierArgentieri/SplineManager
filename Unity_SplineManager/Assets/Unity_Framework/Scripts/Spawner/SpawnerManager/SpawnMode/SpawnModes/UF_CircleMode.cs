using System;
using System.Collections.Generic;
using EditoolsUnity;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unity_Framework.Scripts.Spawner.SpawnerManager.SpawnMode.SpawnModes
{
    [Serializable]
    public class UF_CircleMode : UF_SpawnMode
    {
        #region f/p

        public int Radius = 5;
        public int AgentNumber = 10;

        [SerializeField] private bool SpawnOnEdge = false;
        #endregion

        #region custom methods

        public override void Spawn(GameObject _agent)
        {
            if(!_agent) return;
            if (SpawnOnEdge)
                for (int i = 0; i < AgentNumber; i++)
                    GameObject.Instantiate(_agent, GetEdgedPosition(i, AgentNumber, Radius, Position), Quaternion.identity);
            
            else
                for (int i = 0; i < AgentNumber; i++)
                    GameObject.Instantiate(_agent, GetPositionInCircle(i, AgentNumber, Radius, Position), Quaternion.identity);

        }


        public override void SpawnWithDestroyDelay(GameObject _agent)
        {
            if (!_agent) return;
            if (SpawnOnEdge)
            {
                for (int i = 0; i < AgentNumber; i++)
                {
                    GameObject _go = GameObject.Instantiate(_agent, GetEdgedPosition(i, AgentNumber, Radius, Position),
                        Quaternion.identity);
                    GameObject.Destroy(_go, AutoDestroyDelay);
                }
            }

            else
            {
                for (int i = 0; i < AgentNumber; i++)
                {
                    GameObject _go = GameObject.Instantiate(_agent, GetPositionInCircle(i, AgentNumber, Radius, Position), Quaternion.identity);
                    GameObject.Destroy(_go, AutoDestroyDelay);
                }
            }
            
            
        }
        
        public override void SpawnWithDestroyDelay(List<GameObject> _agents)
        {
            if (SpawnOnEdge)
            {
                for (int i = 0; i < AgentNumber; i++)
                {
                    int _randomIndex = UnityEngine.Random.Range(0, _agents.Count);
                    if (!_agents[_randomIndex]) continue;
                    GameObject _go = GameObject.Instantiate(_agents[_randomIndex], GetEdgedPosition(i, AgentNumber, Radius, Position),
                        Quaternion.identity);
                    GameObject.Destroy(_go, AutoDestroyDelay);
                }
            }

            else
            {
                for (int i = 0; i < AgentNumber; i++)
                {
                    int _randomIndex = UnityEngine.Random.Range(0, _agents.Count);
                    if (!_agents[_randomIndex]) continue;
                    GameObject _go = GameObject.Instantiate(_agents[_randomIndex], GetPositionInCircle(i, AgentNumber, Radius, Position), Quaternion.identity);
                    GameObject.Destroy(_go, AutoDestroyDelay);
                }
            }

        }

        public override void Spawn(List<GameObject> _agents)
        {
            if (SpawnOnEdge)
            {
                for (int i = 0; i < AgentNumber; i++)
                {
                    int _randomIndex = UnityEngine.Random.Range(0, _agents.Count);
                    if (!_agents[_randomIndex]) continue;
                    GameObject _go = GameObject.Instantiate(_agents[_randomIndex], GetEdgedPosition(i, AgentNumber, Radius, Position), Quaternion.identity);
                }
            }

            else
            {
                for (int i = 0; i < AgentNumber; i++)
                {
                    int _randomIndex = UnityEngine.Random.Range(0, _agents.Count);
                    if (!_agents[_randomIndex]) continue;
                    GameObject _go = GameObject.Instantiate(_agents[_randomIndex], GetPositionInCircle(i, AgentNumber, Radius, Position), Quaternion.identity);
                }
            }
           
        }
        
        
        public Vector3 GetEdgedPosition(int _pos, int _maxPos, int _radius, Vector3 _center)
        {
            float _angle = (float) _pos / _maxPos * Mathf.PI * 2;
            
            float _x = _center.x + Mathf.Cos(_angle) * _radius;
            float _y = _center.y;
            float _z = _center.z + Mathf.Sin(_angle) * _radius;

            return new Vector3(_x, _y, _z);
        }
        
        
        public Vector3 GetPositionInCircle(int _pos, int _maxPos, int _radius, Vector3 _center)
        {
            float _angle = (float) _pos / _maxPos * Mathf.PI * 2;

            
            // todo fix collision issue !!
            float _rnd1 = Random.Range(-_radius, _radius);
            float _rnd2 = Random.Range(-_radius, _radius);
            
            float _x = _center.x + Mathf.Cos(_angle) * _rnd1;
            float _y = _center.y;
            float _z = _center.z + Mathf.Sin(_angle) * _rnd2;

            return new Vector3(_x, _y, _z);
        }

        #endregion

        #if UNITY_EDITOR
        public override void DrawSettings()
        {
            EditoolsField.IntSlider("Radius", ref Radius, 1, 100);
            EditoolsField.IntSlider("Agent Number", ref AgentNumber, 1, 50);

            EditoolsField.Toggle("Spawn on circle edge ?", ref SpawnOnEdge);
            EditoolsField.Toggle("Auto Destroy Agents ?", ref AutoDestroyAgent);
            if (AutoDestroyAgent)
                AutoDestroyDelay = EditorGUILayout.Slider("Auto Destroy Delay", AutoDestroyDelay, 0, 15);
            
            
        }

        public override void DrawLinkToSpawner(Vector3 _position) => Handles.DrawDottedLine(Position, _position, 0.5f);

        public override void DrawSceneMode()
        {
            EditoolsHandle.PositionHandle(ref Position, Quaternion.identity);

            Handles.DrawWireDisc(Position, Vector3.up, Radius);
            if(SpawnOnEdge)
                DrawHandleCapOnEdge();
            else
                DrawHandleCapInCircle();
        }


        private void DrawHandleCapOnEdge()
        {
            for (int i = 0; i < AgentNumber; i++)
            {
                Handles.CubeHandleCap(i, GetEdgedPosition(i, AgentNumber, Radius, Position), Quaternion.identity, .1f, EventType.Repaint);
            }
        }
        private void DrawHandleCapInCircle()
        {
            for (int i = 0; i < AgentNumber; i++)
            {
                Handles.CubeHandleCap(i, GetPositionInCircle(i, AgentNumber, Radius, Position), Quaternion.identity, .1f, EventType.Repaint);
            }
        }
        #endif
    }
}