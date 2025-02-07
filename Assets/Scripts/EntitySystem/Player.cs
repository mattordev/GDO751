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
        // Camera controller - Gimbal locked based on the position in relation to the planet instead of the world position
        // Move towards cameras direction when 'W' is pressed

        protected override void Start(){
            // Create singleton
            if(!(this as ISingleton<Player>).CreateSingleton(this)){
                Destroy(gameObject);
                return;
            }
            base.Start();
        }
    }
}