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
        [SerializeField, Range(0, 360)] float maxBankAngle = 180f;

        Quaternion curRot, angVel;

        Transform _cam;
        Transform Camera => _cam ??= UnityEngine.Camera.main.transform;

        protected override Quaternion GetDesiredRotation(Vector3 lookDir){
            Vector3 currentCameraDirection = new(Camera.forward.x, 0, Camera.forward.z);
            float directionCompare = Vector3.Cross(currentCameraDirection, lookDir).y;

            float trgtRoll = Mathf.Lerp(0, maxBankAngle, GetPercentSpeed()) * directionCompare;
            
            
            Quaternion desiredLook = Quaternion.Euler(Camera.eulerAngles.x, Camera.eulerAngles.y, trgtRoll);
            
            float turningSharpness = baseTurningSpeed * turningSensitivity.Evaluate(GetPercentSpeed());
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