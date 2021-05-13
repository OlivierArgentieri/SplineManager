using System;
using Unity_Framework.Scripts._3C.Camera.CameraSetting;
using Unity_Framework.Scripts.Import.Interface;
using UnityEngine;

namespace Unity_Framework.Scripts._3C.Camera.Behaviour
{
    public abstract class UF_CameraBehaviour: MonoBehaviour, IIsValid, IEnable
    {
        #region f/p

        public Action OnUpdateBehaviour = null;
        
        private UF_CameraSetting cameraSetting = null;
        
        public bool IsEnable { get; protected set; }
        public UF_CameraSetting CameraSetting => cameraSetting;
        public bool IsValid => TestValid();
        #endregion
    
    
        #region custom methods
        public virtual void  InitBehaviour(UF_CameraSetting _cameraSetting)
        {
            cameraSetting = _cameraSetting;
        }
    
        protected virtual void FollowTarget()
        {
            if (!IsValid || !cameraSetting.FollowPlayer) return;

        }

        protected virtual void LookAtTarget()
        {
            if (!IsValid || !cameraSetting.FocusPlayer) return;

        }
    
        public void SetEnable(bool _value)
        {
            IsEnable = _value;
        }
    
        protected virtual bool TestValid() => cameraSetting != null;
        #endregion
    }
}