using System;
using Unity_Framework.Scripts._3C.Camera.CameraComponents;
using Unity_Framework.Scripts._3C.Camera.CameraManager;
using Unity_Framework.Scripts._3C.Character.CharacterSettings;
using Unity_Framework.Scripts._3C.Input.InputManager;
using Unity_Framework.Scripts.Import.Interface;
using Unity_Framework.Scripts.Import.Util;
using UnityEngine;

namespace Unity_Framework.Scripts._3C.Character.CharacterBehaviour.CharacterBehaviours
{
    public class UF_CharacterBehaviourRTS : UF_CharacterBehaviour, IIsValid, IEnable
    {
        #region f/p
        private event Action OnUpdate = null; 

        [SerializeField, Header("Enable")] private bool isEnable = true;

        [SerializeField, Header("Target")] private Vector3 target = Vector3.zero;
        [SerializeField, Range(0,10), Header("Stop Distance ")] private float stopDistance = 1f;
        [SerializeField, Range(0,500), Header("Rotate Speed")] private float rotateSpeed = 400;
    
        [SerializeField, Header("Navigable Layer")]private LayerMask navigableLayer = 0;
        [SerializeField, Header("Obstacle Layer")] private LayerMask obstacleLayer = 0;
        [SerializeField, Header("ID Main Camera")] private int idMainCamera = 0;

        private UF_CameraComponent mainCamera = null;
    
        public bool IsMoving => Vector3.Distance(transform.position,target) > stopDistance;
        public bool IsEnable => isEnable;
        public bool IsValid => CharacterSettings != null && mainCamera;
        #endregion


        #region unity methods

        private void Awake()
        {
            UF_CameraManager.OnRegister += SetMainCamera;
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void OnDestroy()
        {
            OnUpdate = null;
            UF_CameraManager.OnRegister -= SetMainCamera;
        }
        #endregion
    
        #region custom methods
        public override void InitCharacterBehaviour(UF_CharacterSettings _characterSettings)
        {
            base.InitCharacterBehaviour(_characterSettings);
        }
    
    
        private void OnMoveRts(bool _action, Vector2 _mousePosition)
        {
            if(!IsValid || !_action || !mainCamera || !mainCamera.CameraSettings) return;

            if (!mainCamera.CameraSettings.LocalCamera) return;
            Ray _ray = Util.ConvertCordToRay(mainCamera.CameraSettings.LocalCamera, _mousePosition, 10);
        
            RaycastHit _hit;
            bool _obstacle = Physics.Raycast(_ray, out _hit, 200, obstacleLayer);

            if (_obstacle) return;
        
            bool _navigable = Physics.Raycast(_ray, out _hit, 200, navigableLayer);
            if(!_navigable) return;

            target = _hit.point;
        }

        void OnMoveTo()
        {
            if (!IsValid || !isEnable) return;
            MoveTo(target, CharacterSettings.MoveSpeed);
        }
    
        void OnRotateTo()
        {
            if (!IsValid || !isEnable) return;

            RotateTo(target, rotateSpeed);
        }
    
        void MoveTo(Vector3 _target, float _moveSpeed)
        {
            if (!IsMoving) return;
            transform.position = Vector3.MoveTowards(transform.position, _target + transform.localScale/2, Time.deltaTime * _moveSpeed);
        }
    
        void RotateTo(Vector3 _target, float _rotateSpeed)
        {
            if ((_target + transform.localScale / 2) - transform.position == Vector3.zero) return;

            Quaternion lookAt = Quaternion.LookRotation((_target  + transform.localScale/2) - transform.position,  transform.up);
            if((lookAt.eulerAngles - Vector3.zero).magnitude > 0)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(lookAt.eulerAngles.y, Vector3.up), Time.deltaTime * _rotateSpeed);
        }

        void SetMainCamera(UF_CameraComponent _camera)
        {
            if(!_camera && _camera.ID != idMainCamera) return;
            mainCamera = _camera;
        
            if (!mainCamera) return;
            UF_InputManager.OnMoveRTS += OnMoveRts;
            OnUpdate += OnMoveTo;
            OnUpdate += OnRotateTo;
            // target = transform.position + transform.localScale/2;
        }
    
        public void SetEnable(bool _value) => isEnable = _value;
        #endregion

        #region Gizmos

        private void OnDrawGizmos()
        {
            if(!IsValid || !mainCamera || !mainCamera.CameraSettings) return;

            if (!mainCamera.CameraSettings.LocalCamera) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(mainCamera.CameraSettings.LocalCamera.transform.position, target);
            Gizmos.color = Color.white;
        }

        #endregion

    }
}