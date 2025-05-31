using System.Collections;
using System.Collections.Generic;
using AstralCandle.Bird;
using AstralCandle.Utilities;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle{
    public class Legs : BirdAnimProfile<BirdMaster>{
        [SerializeField] float speedThreshold;
        [SerializeField] float speedTuneSmoothing = .1f;
        [SerializeField] AnimSys easing;
        [SerializeField] Vector2 relaxed, flying;
        [SerializeField] float legMoveFreq = 10, legMoveAmpltiude = 5;

        float legMovementTimer = 0;

        float speedTuneVel;
        float curSpeed = 0;


        public override void Run(BirdMaster master, float delta){
            curSpeed = Mathf.SmoothDamp(curSpeed, master.Velocity.Magnitude, ref speedTuneVel, speedTuneSmoothing, Mathf.Infinity, delta);

            Vector3 offset = Vector2.zero;
            switch(master.isGrounded){
                case true:
                    break;
                case false:
                    legMovementTimer += delta * legMoveFreq;
                    float t = easing.Trim(curSpeed / speedThreshold);
                    offset = Vector3.LerpUnclamped(relaxed, flying, t);
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