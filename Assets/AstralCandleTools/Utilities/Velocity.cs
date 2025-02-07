using UnityEngine;

/// <summary>
/// ©️2024-2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Utilities{
    /// <summary>
    /// Allows us to track the positional velocity of this objects transform
    /// </summary>
    public class Velocity{
        Vector3 position;
        public Vector3 Value{ get; private set; }
        public Velocity(Vector3 position) => this.position = position;

        /// <summary>
        /// Calculates the velocity for parsed position 
        /// </summary>
        /// <param name="position">The concurrent position</param>
        /// <param name="localiseVelocity">If set, will return velocity data relative to the parsed transform</param>
        /// <returns>Velocity</returns>
        public Vector3 CalculateVelocity(Vector3 position, Transform localiseVelocity = null){
            // Velocity calculation
            Vector3 newVal = position - this.position; 
            if(localiseVelocity){ newVal = localiseVelocity.InverseTransformDirection(newVal); }
            Value = newVal / Time.fixedDeltaTime;
            this.position = position;

            return Value;
        }
    }
}