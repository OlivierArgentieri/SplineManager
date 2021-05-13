using System;
using Unity_Framework.Scripts._3C.Camera.CameraSetting;
using Unity_Framework.Scripts._3C.Input.InputManager;
using Unity_Framework.Scripts.Import.Util;
using UnityEngine;

namespace Unity_Framework.Scripts._3C.Camera.Behaviour.Behaviours
{
    public class UF_CameraBehaviourTPS : UF_CameraBehaviour
    {
        #region f/p
        [SerializeField, Header("Target")] private Transform target = null;

        private float roll = 0; // x
        private float pitch = 0; // y

        public Transform Target => target;

        #endregion

        #region custom methods

        public override void InitBehaviour(UF_CameraSetting _cameraSetting)
        {
            base.InitBehaviour(_cameraSetting);
            OnUpdateBehaviour += FollowTarget;
            SetEnable(true);
            UF_InputManager.OnMouseAxis += OnMouseAxis;
        }

        protected override void FollowTarget()
        {
            if (!IsValid || !CameraSetting.FollowPlayer || !IsEnable) return;
            Vector3 _offset = new Vector3(CameraSetting.OffsetX, CameraSetting.OffsetY, CameraSetting.OffsetZ);
            transform.position = Vector3.Lerp(transform.position, Target.transform.position - (transform.rotation *  _offset), Time.deltaTime * CameraSetting.FollowSpeed);
        }

        private void OnMouseAxis(Vector2 _mouseAxis)
        {
            if (!IsEnable || !IsValid ) return;
       

            roll += -_mouseAxis.y * CameraSetting.RotateSpeed * Time.deltaTime;
            pitch += _mouseAxis.x * CameraSetting.RotateSpeed * Time.deltaTime;

            if (CameraSetting.ClampX)
                roll = Util.ClampRotation(roll, CameraSetting.ClampXValueMax, CameraSetting.ClampXValueMin);
            else
                roll %= 360;
            
            if (CameraSetting.ClampY)
                pitch = Util.ClampRotation(pitch, CameraSetting.ClampYValueMax, CameraSetting.ClampYValueMin);
            else
                pitch %= 360;
            
            transform.rotation = Quaternion.AngleAxis(pitch, Vector3.up) * Quaternion.AngleAxis(roll, Vector3.right);
        }

        protected override bool TestValid() => base.TestValid() && target;
    

        #endregion
    }
}