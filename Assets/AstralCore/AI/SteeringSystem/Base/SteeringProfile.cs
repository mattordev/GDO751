using System;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.AI.SteeringSystem{
    [Serializable] public struct SteeringProfile{
        #region VARIABLES
        /// <summary>
        /// The influenced object
        /// </summary>
        [SerializeField, Tooltip("The influenced object")] Transform transform;
        /// <summary>
        /// Maximum possible speed
        /// </summary>
        [SerializeField, Tooltip("Maximum possible speed")] public float maxSpeed;
        /// <summary>
        /// Maximum possible force that can be applied at a time
        /// </summary>
        [SerializeField, Tooltip("Maximum possible force that can be applied at a time")] public float maxForce;
        /// <summary>
        /// Dictates how quickly the agent can turn
        /// </summary>
        [SerializeField, Tooltip("Dictates how quickly the agent can turn")] public float maxTurnAngle;
        /// <summary>
        /// The concurrent position of the influenced object
        /// </summary>
        public readonly Vector3 Position => transform.position;
        #endregion
        public SteeringProfile(Transform transform, float maxSpeed, float maxForce, float maxTurnAngle){
            this.transform = transform;
            this.maxSpeed = maxSpeed;
            this.maxForce = maxForce;
            this.maxTurnAngle = maxTurnAngle;
        }

        public static implicit operator Transform(SteeringProfile profile) => profile.transform;
    }
}