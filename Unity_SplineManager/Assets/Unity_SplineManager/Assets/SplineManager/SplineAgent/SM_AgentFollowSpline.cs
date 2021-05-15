using System;
using System.Collections.Generic;
using Unity_SplineManager.Assets.SplineManager.Curve;
using UnityEngine;

namespace Unity_SplineManager.Assets.SplineManager.Agent
{
    public class SM_AgentFollowSpline : MonoBehaviour
    {
         #region f/p
    public static event Action OnIsArrived = null; 
    private event Action OnUpdate = null; 
    //[SerializeField, Header("Path ID")]private string pathID;
    [SerializeField, Header("Speed Move"), Range(0,100)]private float speedMove;
    [SerializeField, Header("Speed Rotation"), Range(0,500)]private float speedRotation;
    [SerializeField, Header("Frame Delay (ms)"), Range(0,2000)]private float frameDelay;
    private float totalTime;
    
    private List<SM_Curve> paths = new();
    
    private SM_Curve currentCurve;
    private Vector3 currentPoint;
    private int currentIndex;
    
    public float SpeedMove
    {
        get => speedMove;
        set { speedMove = value; }
    }

    public float SpeedRotation
    {
        get => speedRotation;
        set { speedRotation = value; }
    }

    public int CurveID = 0;
    private Vector3 LastCurvePosition => currentCurve?.Curve[currentCurve.CurveLength-1] ?? Vector3.zero;

    private bool isAtEnd => currentPoint == LastCurvePosition;
    #endregion

    #region unity methods

    void Awake()
    {
        SM_SplineManager.OnInit += InitPath;
        OnUpdate += MoveTo;
        // OnUpdate += RotateTo;
    }

    void Update()
    {
        if (paths == null) return;
        totalTime += Time.deltaTime;

        if (totalTime < frameDelay) return;
        totalTime = 0;
        OnUpdate?.Invoke();

        if (isAtEnd) 
            OnIsArrived?.Invoke();

    }
    #endregion


    #region custom methods

    #region OnUpdate
    void RotateTo()
    {
        if (speedRotation == 0) return;
        
        if (Vector3.Distance(transform.position, currentPoint) > 0)
        {
            Quaternion _lookRotate = Quaternion.LookRotation(currentPoint - transform.position, transform.up);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, _lookRotate, SpeedRotation * Time.deltaTime);
        }
    }

    void MoveTo()
    {
        
        FollowPath();
    }

    #endregion
    
    void InitPath(List<SM_Curve> _paths)
    {
        paths = _paths;
        if (paths != null && CurveID < paths.Count)
        {
            currentCurve = paths[CurveID];
            currentIndex = currentCurve.GetStartAtPercent;
            currentPoint = currentCurve.StartPercentPosition;
            transform.position = currentPoint;
        }
    }
    
    private void FollowPath()
    {
       // GetPathById(pathID);
        
       if (Vector3.Distance(transform.position, currentPoint) < 0.0001f)
       {
           currentPoint = GetNextPoint();
       }
       transform.position = Vector3.MoveTowards(transform.position, currentPoint,  SpeedMove * Time.deltaTime);
    }
    
    // todo with curve manager update
    /*
    private void GetPathById(string _pathId)
    {
        if (paths != null && !string.IsNullOrEmpty(pathID))
        {
            currentPath = paths.FirstOrDefault(p => p.Id.ToLower() == _pathId.ToLower());
        }
    }
    */
    
    private Vector3 GetNextPoint()
    {
        if (currentCurve == null)
        {
            currentIndex = 0;
            return Vector3.zero;
        }

        
        // if (currentPoint == LastCurvePosition) return currentPoint;

        if (currentIndex < currentCurve.CurveLength-1)
        {
            currentIndex++;
            return currentCurve.Curve[currentIndex];
        }

        ResetCurvePath();
        //currentIndex = currentCurve.GetStartAtPercent;
        //currentIndex = currentCurve.GetStartAtPercent;
        return currentCurve.StartPercentPosition;
    }

    private void ResetCurvePath()
    {
        //todo not fixed when point is deplaced
        currentIndex = currentCurve.GetStartAtPercent;
        currentPoint = currentCurve.StartPercentPosition;
        transform.position = currentPoint;
    }

    #endregion
    }
}