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
    public abstract class BirdMotor : Steering3D, ISpeed{
        [SerializeField, Tooltip("Between 0-Maxspeed, controls when we are prime at steering")] protected AnimationCurve turningSensitivity;
        [SerializeField, Tooltip("Speed of turning")] protected float baseTurningSpeed;
        [SerializeField, Tooltip("How strong is gravity against bird")] float gravityScaler = 12f;
        [SerializeField, Tooltip("Resistance against bird")] float drag = 1;
        [SerializeField, Tooltip("How much force to we apply to the bird when actively moving")] float thrustPower = 1;
        [SerializeField, Range(0, 1), Tooltip("% speed until we start facing upright")] float idleSpeed;
        [SerializeField] float minSpeed;
        [SerializeField] Color[] colourMultiplier;
        [SerializeField] AnimEaser easing;

        public float GravityAffector => Vector3.Dot(transform.forward, Vector3.down);
        protected float velocity;
        public float MoveVelocity => velocity;
        /// <summary>
        /// If true, then thrust is being applied
        /// </summary>
        public bool activeMotion = false;

        public bool DestroyBird{ get; set; }

        Rigidbody _rb;
        Rigidbody RB => _rb ??= GetComponent<Rigidbody>();

        
        protected override void ProcessFixedUpdate(){
            if (InputSO.Pause){
                RB.velocity = Vector3.zero;
                return;
            }
            // Motion calculation
            velocity += (gravityScaler * GravityAffector) * Time.fixedDeltaTime;
            velocity *= 1 - (drag * Time.fixedDeltaTime);
            Vector3 dir = transform.InverseTransformDirection(Velocity.normalized);
            if (dir.z > 0) { velocity += thrustPower * Time.fixedDeltaTime; }
            activeMotion = dir.z > 0;

            velocity = Mathf.Clamp(velocity, minSpeed, Profile.maxSpeed);

            // Rotation calculation
            Vector3 lookDir = new(transform.forward.x, 0, transform.forward.z);
            Quaternion desired = GetDesiredRotation(lookDir);
            Quaternion flatLook = Quaternion.LookRotation(lookDir);
            Quaternion final = Quaternion.Slerp(flatLook, desired, Mathf.Clamp01(GetPercentSpeed() / idleSpeed));

            // Application
            RB.velocity = 50 * velocity * transform.forward;
            RB.MoveRotation(final);
        }

        void LateUpdate(){
            easing.SetReverse(DestroyBird);
            transform.localScale = Vector3.one * easing.Play();
            if (DestroyBird && easing.Percent <= 0){
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Returns the rotation we want the bird facing
        /// </summary>
        protected abstract Quaternion GetDesiredRotation(Vector3 lookDir);

        public float GetPercentSpeed() => velocity / Profile.maxSpeed;

        protected virtual void Awake()
        {
            Renderer[] ren = GetComponentsInChildren<Renderer>();
            MaterialPropertyBlock blk = new();
            Color c = colourMultiplier[Random.Range(0, colourMultiplier.Length)];
            foreach (Renderer r in ren){
                r.GetPropertyBlock(blk);
                blk.SetColor("_Color", c);
                r.SetPropertyBlock(blk);
            }
        }
    }
}