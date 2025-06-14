using System;
using System.Collections;
using System.Collections.Generic;
using AstralCandle.Game;
using AstralCandle.Utilities;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.EntitySystem{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public abstract class Character : Entity, ISteering{   
        #region PHYSICS
        /// <summary>
        /// Dont use!
        /// </summary>
        Rigidbody _rb; 
        /// <summary>
        /// The physics agent used to perform any physics interaction
        /// </summary>
        protected Rigidbody Agent => _rb ??= GetComponent<Rigidbody>();
        /// <summary>
        /// Dont use!
        /// </summary>
        Collider _col;
        /// <summary>
        /// Allows us to access the collider of this character
        /// </summary>
        public Collider Collider => _col ??= GetComponent<Collider>();
        public Vector3 LookDirection => transform.forward;
        public Vector3 WorldPosition => Collider.bounds.center;
        /// <summary>
        /// Dont use!
        /// </summary>
        World _world;
        /// <summary>
        /// Grabs the world info
        /// </summary>
        protected World World => _world ??= ISingleton<World>.Instance;
        #endregion
        #region HEALTH
        [SerializeField] float maxHealth = 100, health = 100;
        /// <summary>
        /// The maximum health this character can acquire
        /// </summary>
        protected float MaxHealth => maxHealth;
        /// <summary>
        /// Whats the concurrent health this character has?
        /// </summary>
        protected float Health{ get => health; set{
            value = Mathf.Clamp(value, 0, maxHealth);

            if(value > health){ OnHeal(); } 
            else if(value < health){ 
                OnDamaged(); 
                if(value == 0){ OnDeath(); }
            }
            else{ OnHit(); }

            health = value;
        }}
        
        

        /// <summary>
        /// What happens when this character dies?
        /// </summary>
        protected abstract void OnDeath();
        /// <summary>
        /// What happens when this character heals?
        /// </summary>
        protected abstract void OnHeal();
        /// <summary>
        /// What happens when this character is damaged?
        /// </summary>
        protected abstract void OnDamaged();
        /// <summary>
        /// What happens when this character is hit but not injured
        /// </summary>
        protected abstract void OnHit();
        #endregion
        #region SPEED
        [SerializeField] float speed = 10;
        [SerializeField] float force = 1;
        [SerializeField, Range(0, 1)] float reverseFactor = 0.5f;
        public float MaxSpeed => GetMovePower() * speed;
        public float MaxForce => force;
        public float ReverseFactor => reverseFactor;
        public Vector3 Velocity { get; set; }

        ISteering _s;
        ISteering Steer => _s ??= this;
        #endregion
        #region PLANET
        PlanetManager _manager;
        protected PlanetManager Manager => _manager ??= ISingleton<PlanetManager>.Instance;
        public bool UseGravity = true;
        #endregion
        
        

        // Orient the character in relation to the world
        protected virtual void Move(Vector3 localVelocity){
            Vector3 attraction = Manager.Attract(transform);
            Debug.Log(attraction);

            Quaternion desiredRotation = Quaternion.FromToRotation(transform.up, -attraction.normalized) * transform.rotation;
            Agent.MoveRotation(desiredRotation);

            Vector3 newPos = ((transform.TransformDirection(localVelocity.normalized).normalized * localVelocity.magnitude) + (attraction * (UseGravity? 1: 0))) * Time.fixedDeltaTime;
            Agent.MovePosition(transform.position + newPos);
        }

        /// <summary>
        /// Dictates the speed power of the character. 1 is full speed, -1 is negative full speed. 0 Is nothing
        /// </summary>
        /// <returns>A value between -1, 1</returns>
        protected virtual float GetMovePower() => 1;
    }
}