using System;
using System.Collections.Generic;
using Unity_Framework.Scripts.Import.ManagerTemplate;
using UnityEngine;

namespace Unity_Framework.Scripts._3C.Input.InputManager
{
    public class UF_InputManager : ManagerTemplate<UF_InputManager>
    {
        #region Axis
    
        #region axis delegate
        public static event Action<Vector2> OnMouseAxis = null;
        public static event Action<Vector2> OnMoveFPS = null;
    
        #endregion
    
        #region axis label
        [SerializeField, Header("Mouse X Label")]private string mouseXLabel = "Mouse X";
        [SerializeField, Header("Mouse Y Label")]private string mouseYLabel = "Mouse Y";
    
        [SerializeField, Header("Horizontal Label")] private string moveHorizontalLabel = "Horizontal";
        [SerializeField, Header("Vertical Label")] private string moveVerticalLabel = "Vertical";

        #endregion

        #region axis Value (debug)

        [SerializeField, Header("Mouse X Value ")]private float mouseXValue = 0;
        [SerializeField, Header("Mouse Y Value ")]private float mouseYValue = 0;

        [SerializeField, Header("Mouse position")] private Vector2 mousePosition = Vector2.zero;

        [SerializeField, Header("Horizontal Axis Value")]private float moveHorizontalValue = 0;
        [SerializeField, Header("Vertical Axis Value")]private float moveVerticalValue = 0;
    
        private float MouseXValue => mouseXValue = UnityEngine.Input.GetAxis(mouseXLabel);
        private float MouseYValue => mouseYValue = UnityEngine.Input.GetAxis(mouseYLabel);
    
        public Vector2 MousePosition => mousePosition = UnityEngine.Input.mousePosition;

    
        private float MoveVerticalValue => moveVerticalValue= UnityEngine.Input.GetAxis(moveVerticalLabel);
        private float MoveHorizontalValue => moveHorizontalValue = UnityEngine.Input.GetAxis(moveHorizontalLabel);

        #endregion

        #endregion

        #region input

        #region input delegate
        public static event Action<bool, Vector2> OnMoveRTS = null;
        #endregion

        #region input value (debug)
    
        [SerializeField, Header("RTS Move Player")] private List<UF_Key> moveRTSActions = new List<UF_Key>();
        [SerializeField, Header("RTS Move Player Value")] private bool moveRTSActionValue = false;

        private bool GetMoveRTSValue => moveRTSActionValue = GetListKeyDownInputValue(moveRTSActions);

        #endregion

        #endregion
    
    
        #region unity methods
        private void Update()
        {
            // Axis
            OnMouseAxis?.Invoke(new Vector2(MouseXValue, MouseYValue));
            OnMoveFPS?.Invoke(new Vector2(MoveHorizontalValue, MoveVerticalValue));
        
            // Input
            OnMoveRTS?.Invoke(GetMoveRTSValue, MousePosition);
        }

        private bool GetListKeyDownInputValue(List<UF_Key> _actions)
        {
            for (int i = 0; i < _actions.Count; i++)
            {
                if (UnityEngine.Input.GetKeyDown(_actions[i].Key))
                    return true;
            }
            return false;
        }
    
    
        private bool GetListKeyInputValue(List<UF_Key> _actions)
        {
            for (int i = 0; i < _actions.Count; i++)
            {
                if (UnityEngine.Input.GetKey(_actions[i].Key))
                    return true;
            }
            return false;
        }
        #endregion

    }
}