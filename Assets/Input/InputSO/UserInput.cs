using UnityEngine;
using UnityEngine.InputSystem;

namespace AstralCandle.Input{

    /// <summary>
    /// ©️2024 Designed and Programmed by Joshua Thompson. All rights reserved
    /// </summary>

    [CreateAssetMenu(fileName = "User Input Manager", menuName = "ScriptableObjects/New User Input Manager")]
    public class UserInput : InputSO{
        [Header("Pivot Sensitivity")]
        [SerializeField] Vector2 mouseSensitivity = Vector2.one * 100;
        [SerializeField] Vector2 controllerSensitivity = Vector2.one * 125;
        [SerializeField] bool invertY = false;
        [Header("Zoom Sensitivity")]
        [SerializeField] float mouseZoomSensitivity = .05f;
        [SerializeField] float controllerZoomSensitivity = .05f;

        /// <summary>
        /// How sensitive is the look direction? 
        /// </summary>
        public Vector2 LookSensitivity => UsingGamepad? controllerSensitivity: mouseSensitivity;
        public float ZoomSensitivity => UsingGamepad? controllerZoomSensitivity: mouseZoomSensitivity;
        public bool InvertY => invertY;

        #region VARIABLES
        public delegate void Vector2Event(bool usingGamepad, Vector2 value);
        public event Vector2Event OnLook;
        public delegate void FloatEvent(float value);
        public event FloatEvent OnZoom;
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
                case "Zoom":
                    OnZoom?.Invoke(ctx.ReadValue<float>());
                    break;       
            }
        }
    }
}