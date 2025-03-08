using System.Collections;
using System.Collections.Generic;
using AstralCandle.Game;
using AstralCandle.Utilities;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.EntitySystem{
    public class Bird : Character{
        bool isGrounded = false;
        protected override void OnDeath(){}
        protected override void OnHeal(){}
        protected override void OnDamaged(){}
        protected override void OnHit(){}

        Vector3 previousPosition;

        protected override void Awake(){
            base.Awake();
            previousPosition = transform.position;
        }

        protected override void FixedUpdate(){
            base.FixedUpdate();
        }


        // protected override void Move(Vector3 localVelocity){
        //     Vector3 attraction = Manager.Attract(transform); // Force towards planet
        //     // move
        //     Vector3 newPosition = transform.TransformDirection(localVelocity.normalized) * localVelocity.magnitude;

        //     // rotate towards velocity
        //     Vector3 dir = (transform.position - previousPosition).normalized;
        //     Quaternion desiredRotation = Quaternion.FromToRotation(transform.up, (-attraction.normalized + dir.normalized).normalized) * transform.rotation;
        //     transform.rotation = desiredRotation;
        // }

        /// <summary>
        /// Calculates the orientation relative to the planet
        /// </summary>
        /// <returns>-1 - 1 relative to the orientation to the planet</returns>
        protected override float GetMovePower() => UFunc.Remap(Vector3.Dot(transform.forward, (Manager.GetNearestPlanet(transform).position - transform.position).normalized), -1, 1, 0, 1); // May need a more intuitive way for this
    }
}