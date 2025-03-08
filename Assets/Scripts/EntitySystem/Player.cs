using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AstralCandle.Input;
using AstralCandle.Utilities;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.EntitySystem{
    public sealed class Player : Bird, ISingleton<Player>{    
        [SerializeField] UserInput input;
        [SerializeField] float angularSmoothing = 0.1f;

        CameraController _c;
        CameraController Camera => _c ??= ISingleton<CameraController>.Instance;
        // Camera controller - Gimbal locked based on the position in relation to the planet instead of the world position
        // Move towards cameras direction when 'W' is pressed

        protected override void Awake(){
            // Create singleton
            if(!(this as ISingleton<Player>).CreateSingleton(this)){
                Destroy(gameObject);
                return;
            }
            base.Awake();
        }

        protected override void Start(){
            base.Start();
            Camera.Take(CameraController.Mode.Pivot, transform, new Vector3(0, 0, -5), Vector3.zero, 45, 85);
        }


        protected override void FixedUpdate(){
            base.FixedUpdate();
            Move(new Vector3(input.InputVelocity.x, 0, input.InputVelocity.y) * MaxSpeed);
        }
    }
}