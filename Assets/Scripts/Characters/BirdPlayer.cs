using System.Collections;
using System.Collections.Generic;
using AstralCandle.Input;
using AstralCore.AI.SteeringSystem;
using AstralCore.Utils;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Character{
    public class BirdPlayer : BirdMotor{
        [SerializeField] UserInput input;

        Quaternion curRot, angVel;

        Transform _cam;
        Transform Camera => _cam ??= UnityEngine.Camera.main.transform;

        protected override Quaternion GetDesiredRotation(){
            float turningSharpness = baseTurningSpeed * turningSensitivity.Evaluate(GetPercentSpeed());
            Quaternion desiredLook = Quaternion.Euler(Camera.eulerAngles.x, Camera.eulerAngles.y, 0);
            curRot.SmoothDamp(desiredLook, ref angVel, 1f / turningSharpness, Mathf.Infinity, Time.fixedDeltaTime);
            return curRot;
        }

        protected override Vector3[] GetForces(){
            Vector3 iv = input.InputVelocity;
            (iv.z, iv.y) = (iv.y, 0);
            iv.x = 0;

            return new Vector3[] {
                SteeringFuncs.Seek(transform.TransformPoint(iv), this)
            };
        }

        protected override void InitOnStart() => CameraController.Instance.Take(this);
    }
}