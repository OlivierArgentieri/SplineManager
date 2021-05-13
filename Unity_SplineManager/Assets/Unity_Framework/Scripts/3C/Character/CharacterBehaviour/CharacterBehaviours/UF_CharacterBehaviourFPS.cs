using Unity_Framework.Scripts._3C.Character.CharacterSettings;
using Unity_Framework.Scripts._3C.Input.InputManager;
using Unity_Framework.Scripts.Import.Interface;
using UnityEngine;

namespace Unity_Framework.Scripts._3C.Character.CharacterBehaviour.CharacterBehaviours
{
    public class UF_CharacterBehaviourFPS : UF_CharacterBehaviour, IIsValid, IEnable
    {
        #region f/p
        [SerializeField, Header("Enable")] private bool isEnable = true;
        [SerializeField, Header("Gravity"), Range(0,50)] private float gravity= 20f;

    

        public bool IsEnable => isEnable;

        public bool IsValid => CharacterSettings != null;
        #endregion


        #region unity methods
        #endregion


        #region custom methods
        public override void InitCharacterBehaviour(UF_CharacterSettings _characterSettings)
        {
            base.InitCharacterBehaviour(_characterSettings);

            UF_InputManager.OnMoveFPS += OnMoveFPS;
        }


        void OnMoveFPS(Vector2 _moveAxis)
        {
            if(!IsValid || !IsEnable) return;
            transform.position += _moveAxis.y * Time.deltaTime * CharacterSettings.MoveSpeed * transform.forward ;
            transform.position += _moveAxis.x * Time.deltaTime * CharacterSettings.MoveSpeed * transform.right;
        }
    
        public void SetEnable(bool _value) => isEnable = _value;

        #endregion
    }
}