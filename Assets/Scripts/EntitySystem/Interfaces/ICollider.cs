using UnityEngine;

/// <summary>
/// ©️2024 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.EntitySystem{
    /// <summary>
    /// Allows us to control where an entity is looking
    /// </summary>
    public interface ICollider{
        /// <summary>
        /// The main collider which interacts with the environment
        /// </summary>
        Collider Collider{ get; }
    }
}