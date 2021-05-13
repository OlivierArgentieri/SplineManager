using Unity_Framework.Scripts._3C.Camera.CameraSetting;
using Unity_Framework.Scripts.Import.Util;
using UnityEngine;

namespace Unity_Framework.Scripts._3C.Camera.Behaviour.Behaviours
{
    public class UF_CameraBehaviourRTS : UF_CameraBehaviour
    {
        #region f/p
        [SerializeField, Header("Target")] private Transform target = null;

        public Transform Target => target;

        private float roll = 0; //x
        private float pitch = 0; //y


        #endregion
    
    
        public override void InitBehaviour(UF_CameraSetting _cameraSetting)
        {
            base.InitBehaviour(_cameraSetting);
            OnUpdateBehaviour += FollowTarget;
            OnUpdateBehaviour += LookAtTarget;
            SetEnable(true);
        }


        protected override void FollowTarget()
        {
            if(!IsValid) return;
            base.FollowTarget();
            Vector3 _offset = new Vector3(CameraSetting.OffsetX, CameraSetting.OffsetY, CameraSetting.OffsetZ);
            CameraSetting.LocalCamera.transform.position = Vector3.Lerp(CameraSetting.LocalCamera.transform.position,Target.position + _offset ,Time.deltaTime * CameraSetting.FollowSpeed);
        }

        protected override void LookAtTarget()
        {
            if (!IsEnable) return;

        
            if (CameraSetting.ClampX)
                roll = Util.ClampRotation(roll, CameraSetting.ClampXValueMin, CameraSetting.ClampXValueMax);
            else
                roll = roll % 360;

            if (CameraSetting.ClampY)
                pitch = Util.ClampRotation(pitch, CameraSetting.ClampYValueMin, CameraSetting.ClampYValueMax);
            else
                pitch = pitch % 360;
        
            CameraSetting.LocalCamera.transform.eulerAngles = new Vector3(roll, pitch, transform.eulerAngles.z);
        }
        protected override bool TestValid() => base.TestValid() && target;

    }
}