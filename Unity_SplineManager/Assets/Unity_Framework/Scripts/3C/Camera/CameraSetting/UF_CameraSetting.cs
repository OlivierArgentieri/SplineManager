using System;
using UnityEngine;

namespace Unity_Framework.Scripts._3C.Camera.CameraSetting
{
    [Serializable][CreateAssetMenu(fileName = "New Camera Setting", menuName = "UF/Camera/CameraSetting")]
    public class UF_CameraSetting : ScriptableObject
    {
        #region f/p
        [SerializeField, Header("Camera")] private UnityEngine.Camera localCamera = null;

        #region offset
        [SerializeField, Header("Offset X"), Range(-20, 20)] private float offsetX = 0;
        private float offsetXMax = 20;
        private float offsetXMin = -20;
    
        [SerializeField, Header("Offset Y"), Range(-20, 20)] private float offsetY = 0;
        private float offsetYMax = 20;
        private float offsetYMin = -20;
    
        [SerializeField, Header("Offset Z"), Range(-20, 20)] private float offsetZ = 0;
        private float offsetZMax = 20;
        private float offsetZMin = -20;
        #endregion

        #region clamp
        [SerializeField, Header("Clamp X")] private bool clampX = false;
        [SerializeField, Header("Clamp Y")] private bool clampY = false;

        [SerializeField, Header("Clamp X Value Min"), Range(-180, 180)] private float clampXValueMin = 0;
        [SerializeField, Header("Clamp X Value Max"), Range(-180, 180)] private float clampXValueMax = 0;
        [SerializeField, Header("Clamp Y Value Min"), Range(-180, 180)] private float clampYValueMin = 0;
        [SerializeField, Header("Clamp Y Value Max"), Range(-180, 180)] private float clampYValueMax = 0;
        #endregion
   
        [SerializeField, Header("Follow Player ?")] private bool followPlayer= false;
        [SerializeField, Header("Focus Player ?")] private bool focusPlayer = false;
    
        [SerializeField, Header("Rotate Speed"), Range(0, 200)] private float rotateSpeed = .1f;
        private float rotateSpeedMax = 200;
        private float rotateSpeedMin = 0;


        [SerializeField, Header("Follow Target Speed"), Range(0, 100)] private float followSpeed = .1f;
        private float followSpeedMax = 100f;
        private float followSpeedMin = 0;

    
        #region get/set

        public UnityEngine.Camera LocalCamera => localCamera;


        #region offset
        public float OffsetX
        {
            get => offsetX;
            set => offsetX = ClampValue(value, offsetXMax, offsetXMin);
        }

        public float OffsetY
        {
            get => offsetY;
            set=> offsetY = ClampValue(value, offsetYMax, offsetYMin);
        }

        public float OffsetZ
        {
            get => offsetZ;
            set=> offsetZ = ClampValue(value, offsetZMax, offsetZMin);
        }

        #endregion


        #region clamp
        public bool ClampX
        {
            get => clampX;
            set => clampX = value;
        }

        public bool ClampY
        {
            get => clampY;
            set => clampY = value;
        }
    
        public float ClampXValueMin
        {
            get => clampXValueMin;
            set => clampXValueMin = ClampValue(value, 180, -180);
        }

        public float ClampXValueMax
        {
            get => clampXValueMax;
            set => clampXValueMax = ClampValue(value, 180, -180);
        }
    
        public float ClampYValueMin
        {
            get => clampYValueMin;
            set => clampYValueMin = ClampValue(value, 180, -180);
        }

        public float ClampYValueMax
        {
            get => clampYValueMax;
            set => clampYValueMax = ClampValue(value, 180, -180);
        }
        #endregion

        public bool FollowPlayer => followPlayer;
        public bool FocusPlayer => focusPlayer;
        public float RotateSpeed
        {
            get => rotateSpeed;
            set => rotateSpeed = ClampValue(value, rotateSpeedMax, rotateSpeedMin);
        }

        public float FollowSpeed
        {
            get => followSpeed;
            set => followSpeed = ClampValue(value, followSpeedMax, followSpeedMin);
        }
    
        #endregion

        #endregion


        #region constructor

        #endregion

        #region custom methods
 
        float ClampValue(float _value, float _maxValue, float _minValue)
        {
            float _toReturn = _value;
            if (_toReturn > _maxValue)
                _toReturn = _maxValue;
            else if (_toReturn < _minValue)
                _toReturn = _minValue;

            return _toReturn;
        }

        public void SetLocalCamera(UnityEngine.Camera _camera)
        {
            localCamera = _camera;
        }
        #endregion
    }
}