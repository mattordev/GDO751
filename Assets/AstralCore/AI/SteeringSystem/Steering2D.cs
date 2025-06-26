using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.AI.SteeringSystem{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Steering2D : SteeringMono{
        Collider2D _col;
        /// <summary>
        /// The collider attached to this object
        /// </summary>
        protected Collider2D Collider => _col ??= GetComponent<Collider2D>();
        public override Vector3 GetClosestTo(Vector3 position) => Collider.ClosestPoint(position);
    }
}