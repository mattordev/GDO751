using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AstralCandle.Input{

    /// <summary>
    /// ©️2024 Designed and Programmed by Joshua Thompson. All rights reserved
    /// </summary>

    public abstract class InputSO : ScriptableObject{
        [SerializeField] bool forceHideCursor = false;
        [SerializeField] InputActionAsset asset;
        [SerializeField] protected Action[] _actions;

        /// <summary>
        /// Are we using a gamepad in the last frame? 
        /// </summary>
        public bool UsingGamepad{ get; private set; } = false;

        public virtual void OnEnable() {
            if(!asset){ return; }
            
            for(int i = 0; i < _actions.Length; i++){
                Action a = _actions[i];
                InputAction action = asset.FindAction(a.Name);    
                if(action == null){ continue; }
                
                action.started += Run;
                if(a.RunOnPerform){ action.performed += Run; }
                action.canceled += Run;

                action.Enable();
                _actions[i].action = action;
            }
        }
        public virtual void OnDisable() {
            foreach(var a in _actions){
                a.action.Disable();
                a.action.started -= Run;
                if(a.RunOnPerform){ a.action.performed -= Run; }
                a.action.canceled -= Run;
            }
        }

        /// <summary>
        /// Clones this object
        /// </summary>
        public void Clone() => Instantiate(this);

        void Run(InputAction.CallbackContext ctx){
            UsingGamepad = Gamepad.current?.wasUpdatedThisFrame == true;        
            Cursor.visible = !(UsingGamepad || forceHideCursor);
            Cursor.lockState = Cursor.visible? CursorLockMode.None : CursorLockMode.Locked;
            ActionManager(ctx);
        }
        
        /// <summary>
        /// Should dictate what methods are called
        /// </summary>
        protected abstract void ActionManager(InputAction.CallbackContext ctx);

        [Serializable] public class Action{
            [SerializeField] string name;
            [SerializeField] bool runOnPerform = false;

            public string Name => name;
            public bool RunOnPerform => runOnPerform;
            [HideInInspector] public InputAction action;
        }
    }
}