using System;
using UnityEngine;

namespace Unity_Framework.Scripts.Path.PathManager.PathAgent.PathAgentSettings
{
    [Serializable][CreateAssetMenu(fileName = "New Path Agent Setting", menuName = "PathTool/PathAgentSetting")]
    public class UF_PathAgentSettings : ScriptableObject
    {
        #region f/p
        public float SpeedMove = 1;
        public float SpeedRotation = 1;
        public GameObject TargetLookAt = null;
        #endregion

        #region ui f/p
        public bool UseLookAt = false;
        #endregion
    }
}