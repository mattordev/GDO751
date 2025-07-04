using System.Collections;
using System.Collections.Generic;
using AstralCandle.Bird;
using AstralCore.Utils;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle{
    public class Wings: BirdAnimProfile<BirdMaster>{
        [SerializeField] float speedThreshold;
        [SerializeField] float speedTuneSmoothing = .1f;
        [SerializeField] AnimEaser easing;
        [SerializeField] MinMax<float> orientationThresholds = new(.5f, .9f);
        [SerializeField, Range(0,1)] float stopFlappingThreshold = 0.35f;
        [SerializeField] Vector3 relaxed, flapping;

        [Header("Flap")]
        [SerializeField] float flapFreq = 1f;
        [SerializeField] float flapAmp = .1f;
        [SerializeField] float flapRange;
        [SerializeField] float flapPull;

        float speedTuneVel;
        float curSpeed = 0;
        float flapTimer = 0;

        public override void Run(BirdMaster master, float delta){
            curSpeed = Mathf.SmoothDamp(curSpeed, master.Motor.MoveVelocity, ref speedTuneVel, speedTuneSmoothing, Mathf.Infinity, delta);
            float speedPercent = easing.Trim(curSpeed / speedThreshold);

            float orientation = master.DirectionToGround(orientationThresholds.min, orientationThresholds.max);

            Vector3 position = default;
            switch(master.isGrounded){
                case true:
                    break;
                case false:
                    flapTimer += delta * flapFreq;
                    Vector3 flap = flapping + new Vector3(-Mathf.Cos(flapTimer) / flapAmp * flapPull, Mathf.Sin(flapTimer) / flapAmp) * flapRange;
                    if(orientation >= stopFlappingThreshold){ flap = flapping; }

                    // relaxed if facing ground
                    position = Vector3.LerpUnclamped(flap, relaxed, orientation);
                    break;
            }


            foreach(Bone b in bones){ b.Set(transform, position); }            
        }


        public override void Debug(){

        }
    }
}