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
    public class BirdPlayer : BirdMotor
    {
        public static BirdPlayer Instance;
        [SerializeField] UserInput input;
        [SerializeField, Range(0, 360)] float maxBankAngle = 180f;
        [SerializeField] LayerMask obstacles;
        [SerializeField] float obstacleRadius;
        [SerializeField] float dangerWeight;

        Quaternion curRot, angVel;

        Transform _cam;
        Transform Camera => _cam ??= UnityEngine.Camera.main.transform;

        protected override Quaternion GetDesiredRotation(Vector3 lookDir){
            Vector3 camForward = Camera.forward.normalized;
            // Debug.Log($"{camForward} // {lookDir}");
            float pitch = -Mathf.Asin(camForward.y) * Mathf.Rad2Deg;
            float yaw = Mathf.Atan2(camForward.x, camForward.z) * Mathf.Rad2Deg;

            Vector3 flatCamForward = new Vector3(camForward.x, 0, camForward.z).normalized;
            Vector3 flatLook = new Vector3(lookDir.x, 0, lookDir.z).normalized;
            float directionCompare = Vector3.Cross(flatCamForward, flatLook).y;

            float trgtRoll = Mathf.Lerp(0, maxBankAngle, GetPercentSpeed()) * directionCompare;

            Quaternion desiredLook = Quaternion.Euler(pitch, yaw, trgtRoll);

            float turningSharpness = baseTurningSpeed * turningSensitivity.Evaluate(GetPercentSpeed());
            curRot.SmoothDamp(desiredLook, ref angVel, 1f / turningSharpness, Mathf.Infinity, Time.fixedDeltaTime);
            return curRot;
        }


        protected override Vector3[] GetForces()
        {
            Vector3 iv = input.InputVelocity;
            (iv.z, iv.y) = (iv.y, 0);
            iv.x = 0;

            return new Vector3[] {
                SteeringFuncs.Seek(transform.TransformPoint(iv), this)
            };
        }

        protected override void InitOnStart() => CameraController.Instance.Take(this);

        protected override void Awake(){
            angVel = Quaternion.identity;
            base.Awake();
            Instance = this;
        }
    }
}