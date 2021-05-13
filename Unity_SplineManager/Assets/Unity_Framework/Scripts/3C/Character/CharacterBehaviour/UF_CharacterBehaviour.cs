using Unity_Framework.Scripts._3C.Character.CharacterSettings;
using UnityEngine;

namespace Unity_Framework.Scripts._3C.Character.CharacterBehaviour
{
    public class UF_CharacterBehaviour : MonoBehaviour
    {
        #region f/p
        private UF_CharacterSettings characterSettings = null;
        protected UF_CharacterSettings CharacterSettings => characterSettings ;
        #endregion


        #region custom methods
        public virtual void InitCharacterBehaviour(UF_CharacterSettings _characterSettings)
        {
            characterSettings = _characterSettings;
        }
        #endregion
    }
}