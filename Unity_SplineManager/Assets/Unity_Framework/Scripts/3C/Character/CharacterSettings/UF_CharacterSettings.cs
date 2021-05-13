using System;
using Unity_Framework.Scripts._3C.Character.CharacterComponent;
using UnityEngine;

namespace Unity_Framework.Scripts._3C.Character.CharacterSettings
{
    [Serializable]
    public class UF_CharacterSettings
    {
        #region f/p
        [SerializeField, Header("Move Speed "), Range(0,100)]private float moveSpeed = 10.0f;
        private UF_CharacterComponent localPlayer = null;

    
        public float MoveSpeed => moveSpeed;

        public UF_CharacterComponent LocalPlayer => localPlayer;
        #endregion

    
    
        #region custom methods
        public void InitSettings(UF_CharacterComponent _character)
        {
            SetPlayer(_character);
        }

        private void SetPlayer(UF_CharacterComponent _character) => localPlayer = _character;

        #endregion
    }
}