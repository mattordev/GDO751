using System.Collections;
using System.Collections.Generic;
using AstralCore.AI.SteeringSystem;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Character{
    public abstract class BirdMotor : Steering3D, ISpeed{
        [SerializeField, Tooltip("Between 0-Maxspeed, controls when we are prime at steering")] protected AnimationCurve turningSensitivity;
        [SerializeField, Tooltip("Speed of turning")] protected float baseTurningSpeed;
        [SerializeField, Tooltip("How strong is gravity against bird")] float gravityScaler = 12f;
        [SerializeField, Tooltip("Resistance against bird")] float drag = 1;
        [SerializeField, Tooltip("How much force to we apply to the bird when actively moving")] float thrustPower = 1;
        [SerializeField, Range(0, 1), Tooltip("% speed until we start facing upright")] float idleSpeed;

        public float GravityAffector => Vector3.Dot(transform.forward, Vector3.down);
        protected float velocity;
        public float MoveVelocity => velocity;
        
        protected override void ProcessFixedUpdate(){
            // Motion calculation
            velocity += (gravityScaler * GravityAffector) * Time.fixedDeltaTime;
            velocity *= 1 - (drag * Time.fixedDeltaTime);            
            Vector3 dir = transform.InverseTransformDirection(Velocity.normalized);
            if (dir.z > 0) { velocity += thrustPower * Time.fixedDeltaTime; }

            velocity = Mathf.Clamp(velocity, 0, Profile.maxSpeed);

            // Rotation calculation
            Quaternion desired = GetDesiredRotation();
            Quaternion flat = Quaternion.LookRotation(new(transform.forward.x, 0, transform.forward.z));
            Quaternion final = Quaternion.Slerp(flat, desired, Mathf.Clamp01(GetPercentSpeed() / idleSpeed));


            // Application
            transform.rotation = final;
            transform.position += transform.forward * velocity;
        }

        /// <summary>
        /// Returns the rotation we want the bird facing
        /// </summary>
        protected abstract Quaternion GetDesiredRotation();

        public float GetPercentSpeed() => velocity / Profile.maxSpeed;
    }
}