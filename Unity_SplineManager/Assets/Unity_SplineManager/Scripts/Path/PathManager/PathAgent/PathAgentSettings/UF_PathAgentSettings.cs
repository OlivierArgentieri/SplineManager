using System;
using UnityEngine;

namespace Unity_SplineManager.Scripts.Path.PathManager.PathAgent.PathAgentSettings
{
    [Serializable][CreateAssetMenu(fileName = "New Path Agent Setting", menuName = "UF/Path/PathAgentSetting")]
    public class UF_PathAgentSettings : ScriptableObject
    {
        public float SpeedMove = 1;
        public float SpeedRotation = 1;
    }
}