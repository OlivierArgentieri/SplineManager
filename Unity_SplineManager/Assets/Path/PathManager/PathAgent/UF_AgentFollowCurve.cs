using System;
using System.Collections.Generic;
using Unity_Framework.Scripts.Import.Interface;
using Unity_Framework.Scripts.Path.PathManager.PathAgent.PathAgentSettings;
using Unity_Framework.Scripts.Path.PathManager.PathMode;
using UnityEngine;

namespace Unity_Framework.Scripts.Path.PathManager.PathAgent
{
    public class UF_AgentFollowCurve : MonoBehaviour, IIsValid
    {
        #region f/p

        public static event Action OnIsArrived = null;

        [Header("Agent Settings")]
        public UF_PathAgentSettings agentSetting;
        private event Action OnUpdate = null;

        private List<Vector3> pathPoints = new List<Vector3>();


        private Vector3 currentPoint;
        private int currentIndex;
        private bool isAtEnd => currentPoint == lastCurvePosition;

        public float SpeedMove => agentSetting.SpeedMove;
        public float SpeedRotation => agentSetting.SpeedRotation;

        public UF_PathModeSelector CurrentPath;
        private Vector3 lastCurvePosition => pathPoints?[pathPoints.Count - 1] ?? Vector3.zero;


        public bool IsValid => pathPoints != null && CurrentPath != null && agentSetting;

        #endregion

        #region unity methods

        void Awake()
        {
            UF_PathManager.OnInit += InitPath;
        }

        void Start()
        {
            OnUpdate += MoveTo;
            if (agentSetting.TargetLookAt)
                OnUpdate += LookAt;
            else
                OnUpdate += RotateTo;
        }

        void Update()
        {
            if (pathPoints == null) return;

            OnUpdate?.Invoke();

            if (isAtEnd)
                OnIsArrived?.Invoke();
        }

        #endregion


        #region custom methods

        void RotateTo()
        {
            if (Vector3.Distance(transform.position, currentPoint) > 0.00001f) // epsilon?
            {
                Quaternion _lookRotate = Quaternion.LookRotation(currentPoint - transform.position, transform.up);
                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, _lookRotate, SpeedRotation * Time.deltaTime);
            }
        }

        void LookAt()
        {
            transform.LookAt(agentSetting.TargetLookAt.transform);
        }

        void MoveTo()
        {
            FollowPath();
        }
        
        void InitPath()
        {
            pathPoints = CurrentPath.Mode.PathPoints;
            if (!IsValid) return;

            currentIndex = CurrentPath.Mode.GetStartPercentIndex;
            currentPoint = CurrentPath.Mode.StartPercentPosition;
            transform.position = currentPoint;
        }

        private void FollowPath()
        {
            if (Vector3.Distance(transform.position, currentPoint) < 0.00001f) // epsilon?

                currentPoint = GetNextPoint();

            transform.position = Vector3.MoveTowards(transform.position, currentPoint, SpeedMove * Time.deltaTime);
        }

        private Vector3 GetNextPoint()
        {
            if (CurrentPath == null)
            {
                currentIndex = 0;
                return Vector3.zero;
            }
            
            if (isAtEnd)
            {
                OnIsArrived?.Invoke();
                return currentPoint;
            }

            if (currentIndex < pathPoints.Count)
            {
                currentIndex++;
                return pathPoints[currentIndex];
            }

 
            return CurrentPath.Mode.StartPercentPosition;
        }

        // todo need review
        /*
        private void ResetCurvePath()
        {
            //todo not fixed when point is deplaced
            currentIndex = currentCurve.GetStartAtPercent;
            currentPoint = currentCurve.StartPercentPosition;
            transform.position = currentPoint;
        }*/

        #endregion


        #region debug

        private void OnDrawGizmos()
        {
            DisplayPath();
        }


        private void DisplayPath()
        {
            Gizmos.color = CurrentPath.Mode.PathColor;

            for (int i = 0; i < pathPoints.Count - 1; i++)
            {
                Gizmos.DrawLine(pathPoints[i], pathPoints[i + 1]);
            }

            Gizmos.color = Color.white;
        }

        #endregion
    }
}