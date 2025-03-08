using UnityEngine;

/// <summary>
/// ©️2024 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.EntitySystem{
    /// <summary>
    /// Allows us to control where an entity is looking
    /// </summary>
    public interface ILook{
        /// <summary>
        /// The direction this entity is facing
        /// </summary>
        Vector3 LookDirection{ get; }
    }
}