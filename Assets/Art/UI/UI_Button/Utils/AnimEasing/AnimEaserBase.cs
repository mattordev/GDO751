using System;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Utils.Animation{
    [Serializable] public abstract class AnimEaserBase{
        [SerializeField, Tooltip("This will control the value through time")] AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField, Tooltip("The duration of this animation")] float _duration;
        [SerializeField, Tooltip("Will this be fps or physics orientated?")] UpdateTime _time;
        float _elapsed;

        /// <summary>
        /// If true, makes time return a negative value
        /// </summary>
        protected bool reversed = false;



        // QUICK GETTERS

        /// <summary>
        /// The elapsed duration of this animation (Time between 0-duration)
        /// </summary>
        public float Elapsed { get => _elapsed; protected set => _elapsed = Mathf.Clamp(value, 0, _duration); }
        /// <summary>
        /// Max duration (In seconds) of this animation
        /// </summary>
        public float Duration => _duration;
        /// <summary>
        /// Value between 0-1 resembling how complete the animation is
        /// </summary>
        public float Percent => Elapsed / Duration;
        /// <summary>
        /// The animation curve to follow
        /// </summary>
        protected AnimationCurve Curve => _curve;
        /// <summary>
        /// Based on constructor will return either time.deltaTime or time.fixedDeltaTime
        /// </summary>
        protected float Time => (_time.Equals(UpdateTime.Update)? UnityEngine.Time.deltaTime: UnityEngine.Time.fixedDeltaTime) * (reversed? -1: 1);
        /// <summary>
        /// The resulting value
        /// </summary>
        public float Value { get; protected set; }


        // CONSTRUCTORS

        /// <summary>
        /// Constructor for our animation system
        /// </summary>
        /// <param name="curve">This will control the value through time</param>
        /// <param name="duration">The duration of this animation</param>
        public AnimEaserBase(AnimationCurve curve, float duration, UpdateTime time){
            _curve = curve;
            _duration = duration;
            _time = time;
        }

        /// <summary>
        /// Constructor for our animation system
        /// </summary>
        /// <param name="v">Parsed animation system</param>
        public AnimEaserBase(AnimEaserBase v){
            _curve = v._curve;
            _duration = v._duration;
            _time = v._time;
        }

        /// <summary>
        /// Dictates what update time we will use for animation easing
        /// </summary>
        [Serializable] public enum UpdateTime{
            /// <summary>
            /// FPS orientated | Time.deltaTime
            /// </summary>
            [Tooltip("FPS orientated | Time.deltaTime")] Update,
            /// <summary>
            /// Physics orientated | Time.fixedDeltaTime
            /// </summary>
            [Tooltip("Physics orientated | Time.fixedDeltaTime")] FixedUpdate
        }
    }
}