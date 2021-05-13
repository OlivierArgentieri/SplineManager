using System;
using System.Collections.Generic;
using Unity_Framework.Scripts.Path.PathManager.PathMode;
using UnityEngine;

namespace Unity_Framework.Scripts.Path.PathManager.Path
{
    [Serializable]
    public class UF_Path
    {
        // todo remove : Because each path is a "PathModeSelector" object, so this class can be removed
        
        #region f/p
        public UF_PathModeSelector PathMode = new UF_PathModeSelector();
        #endregion
    }
}