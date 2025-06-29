using System.Collections;
using System.Collections.Generic;
using AstralCore.AI.SteeringSystem;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Character{
    public class CharacterBase : Steering3D{
        [SerializeField] float gravityScaler = 12f;
        [SerializeField] float drag = 1;
        [SerializeField] float thrustPower = 1;
        float GravityAffector => Vector3.Dot(transform.forward, Vector3.down);
        float velocity;
        protected override Vector3[] GetForces() => new Vector3[] { };

        protected override void InitOnStart() { }

        protected override void ProcessFixedUpdate(){
            Vector3 dir = Velocity.normalized;
            dir.z = 1;

            velocity += (gravityScaler * GravityAffector) * Time.fixedDeltaTime;
            velocity *= 1 - (drag * Time.fixedDeltaTime);            
            if (dir.z > 0) { velocity += thrustPower * Time.fixedDeltaTime; }

            velocity = Mathf.Clamp(velocity, 0, Profile.maxSpeed);
            transform.position += transform.forward * velocity;
        }
    }
}