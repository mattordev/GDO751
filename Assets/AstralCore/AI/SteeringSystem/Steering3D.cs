using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.AI.SteeringSystem{
    [RequireComponent(typeof(Collider))]
    public abstract class Steering3D : SteeringMono{
        Collider _col;
        /// <summary>
        /// The collider attached to this object
        /// </summary>
        protected Collider Collider => _col ??= GetComponent<Collider>();
        public override Vector3 GetClosestTo(Vector3 position) => Collider.ClosestPoint(position);
    }
}