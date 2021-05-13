using System;
using System.Collections.Generic;
using Unity_Framework.Scripts._3C.Character.CharacterComponent;
using Unity_Framework.Scripts.Import.Interface.Manager;
using Unity_Framework.Scripts.Import.ManagerTemplate;

namespace Unity_Framework.Scripts._3C.Character.CharacterManager
{
    public class UF_CharacterManager : ManagerTemplate<UF_CharacterManager>, IHandler<int, UF_CharacterComponent>
    {
        #region f/p
        public static event Action OnReady = null;

        public Dictionary<int, UF_CharacterComponent> Handles { get; private set; } = new Dictionary<int, UF_CharacterComponent>();

        public bool IsValid => Handles != null;
        #endregion


        #region unity methods

        protected void Start()
        {
            OnReady?.Invoke();
        }

        #endregion
    
        #region custom methods
        private void Handle(bool _add, UF_CharacterComponent _component)
        {
            if (!_component || !_component.IsValid || !IsValid) return;

            bool handle = _add ? !IsExist(_component) : IsExist(_component);

            if (!handle) return; // throw new Exception("CharacterManager => Invalid Component in Handler Method");

            if (_add)
                Handles.Add(_component.ID, _component);
            else
                Handles.Remove(_component.ID);

        }
    
        public void Add(UF_CharacterComponent _item) => Handle(true, _item);
        public void Remove(UF_CharacterComponent _item) => Handle(false, _item);
    
        public bool IsExist(UF_CharacterComponent _item)
        {
            if (!IsValid) throw new Exception("CharacterManager => Invalid CharacterManager");
            return IsExist(_item.ID);
        }

        public bool IsExist(int _id) => Handles.ContainsKey(_id);

        #endregion
    }
}
