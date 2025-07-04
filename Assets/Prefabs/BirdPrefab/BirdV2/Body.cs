using AstralCandle.Bird;
using AstralCore.Utils;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle{
    public class Body: BirdAnimProfile<BirdMaster>{
        [SerializeField] float speedThreshold;
        [SerializeField] float speedTuneSmoothing = .1f;
        [SerializeField] AnimEaser easing;
        [SerializeField] MinMax<float> orientationThresholds = new(.5f, .9f);
        [SerializeField, Range(0,1)] float stopFlappingThreshold = 0.35f;
        [SerializeField] float relaxedRotation, flyingRotation;

        [Header("Bounce")]
        [SerializeField] float bounceFreq = 1f;
        [SerializeField] float bounceAmp = .1f;

        float speedTuneVel;
        float curSpeed = 0;
        float bounceTimer = 0;

        public override void Run(BirdMaster master, float delta){
            curSpeed = Mathf.SmoothDamp(curSpeed, master.Motor.MoveVelocity, ref speedTuneVel, speedTuneSmoothing, Mathf.Infinity, delta);
            float speedPercent = easing.Trim(curSpeed / speedThreshold);

            float orientation = master.DirectionToGround(orientationThresholds.min, orientationThresholds.max);
            float angle = 0;

            Vector3 position = default;
            switch(master.isGrounded){
                case true:
                    break;
                case false:
                    angle = Mathf.LerpUnclamped(relaxedRotation, flyingRotation, speedPercent);

                    bounceTimer += delta * bounceFreq;
                    Vector3 bobbing = new Vector2(0, Mathf.Sin(bounceTimer) / bounceAmp);
                    bobbing = Vector3.Lerp(Vector3.zero, bobbing, 1 - speedPercent);
                    if(orientation >= stopFlappingThreshold){ orientation = 1; }

                    position = Vector3.Lerp(bobbing, Vector3.zero, orientation);
                    break;
            }


            
            foreach(Bone b in bones){ 
                b.SetRotation(Quaternion.Euler(0, angle, 0)); 

                b.Set(transform, position);
            }            
        }


        public override void Debug(){}
    }
}