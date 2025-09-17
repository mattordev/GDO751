using System;
using AstralCandle.Bird;
using AstralCore.Utils;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle{
    public class Legs : BirdAnimProfile<BirdMaster>{
        [SerializeField, Range(0.00001f, 1)] float lerpThreshold = .2f;
        [SerializeField] AnimEaser easing;
        [SerializeField] Vector2 relaxed, flying;
        [SerializeField] float legMoveFreq = 10, legMoveAmpltiude = 5;

        float legMovementTimer = 0;

        float curSpeed = 0;


        public override void Run(BirdMaster master, float delta){
            Vector3 offset = Vector2.zero;
            switch(master.isGrounded){
                case true:
                    break;
                case false:
                    legMovementTimer += delta * legMoveFreq;
                    
                    offset = Vector3.LerpUnclamped(relaxed, flying, Mathf.Clamp01(master.Motor.GetPercentSpeed() / lerpThreshold));
                    offset += new Vector3(Mathf.Cos(legMovementTimer/3) / legMoveAmpltiude, 0);
                    break;
            }

            
            
            foreach(Bone b in bones){ b.Set(transform, offset); }            
        }


        public override void Debug(){
            Gizmos.DrawWireSphere(transform.TransformPoint(relaxed), 0.25f);
            Gizmos.DrawWireSphere(transform.TransformPoint(flying), 0.25f);
        }
    }
}