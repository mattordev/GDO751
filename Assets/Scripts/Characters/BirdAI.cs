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
    public class BirdAI : BirdMotor{
        [SerializeField] bool showDebug = false;
        [SerializeField, Range(0, 360)] float maxBankAngle = 180f;
        [SerializeField] LayerMask obstacles;
        [SerializeField] float obstacleRadius;
        [SerializeField] float dangerWeight;
        [SerializeField] float playerEvadeDistance;
        [SerializeField] float flockSeparationDistance;
        [HideInInspector] public Vector3 targetPosition;
        Quaternion curRot, angVel;
        readonly Directions directions = Directions.XYZ;

        [HideInInspector] public BirdMotor[] agents;
        protected override Quaternion GetDesiredRotation(Vector3 lookDir)
        {
            Vector3 steeringDir = Velocity.normalized;
            float pitch = -Mathf.Asin(steeringDir.y) * Mathf.Rad2Deg;
            float yaw = Mathf.Atan2(steeringDir.x, steeringDir.z) * Mathf.Rad2Deg;

            Vector3 flatTarget = new Vector3(steeringDir.x, 0, steeringDir.z).normalized;
            Vector3 flatLook = new Vector3(lookDir.x, 0, lookDir.z).normalized;
            float directionCompare = Vector3.Cross(flatTarget, flatLook).y;

            float trgtRoll = Mathf.Lerp(0, maxBankAngle, GetPercentSpeed()) * directionCompare;

            Quaternion desiredLook = Quaternion.Euler(pitch, yaw, trgtRoll);

            float turningSharpness = baseTurningSpeed * turningSensitivity.Evaluate(GetPercentSpeed());
            curRot.SmoothDamp(desiredLook, ref angVel, 1f / turningSharpness, Mathf.Infinity, Time.fixedDeltaTime);

            return curRot;
        }

        

        protected override Vector3[] GetForces()
        {
            return new Vector3[] {
                SteeringFuncs.Avoid(this, SteeringFuncs.Seek(targetPosition, this).normalized, obstacleRadius, obstacles, directions, dangerWeight, showDebug) * .75f,
                SteeringFuncs.Cohesion(agents, this) * .05f,
                SteeringFuncs.Separation(agents, flockSeparationDistance, this) * .5f,
                SteeringFuncs.Wander(90, 3f, 4f, this) * .05f,
                SteeringFuncs.Evade(BirdPlayer.Instance, this, playerEvadeDistance) * .25f
            };
        }

        protected override void InitOnStart() { }
    }
}