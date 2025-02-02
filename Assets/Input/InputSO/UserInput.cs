using UnityEngine;
using UnityEngine.InputSystem;

namespace AstralCandle.Input{

    /// <summary>
    /// ©️2024 Designed and Programmed by Joshua Thompson. All rights reserved
    /// </summary>

    [CreateAssetMenu(fileName = "User Input Manager", menuName = "ScriptableObjects/New User Input Manager")]
    public class UserInput : InputSO{
        #region VARIABLES
        public delegate void Vector2Event(bool usingGamepad, Vector2 value);
        public event Vector2Event OnLook;
        public Vector2 InputVelocity{ get; private set; }
        #endregion

        protected override void ActionManager(InputAction.CallbackContext ctx){
            string _name = ctx.action.name;
            switch(_name){
                case "Move":
                    InputVelocity = ctx.ReadValue<Vector2>();
                    break;
                case "Look":
                    OnLook?.Invoke(UsingGamepad, ctx.ReadValue<Vector2>());
                    break;                
            }
        }
    }
}