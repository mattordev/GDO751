using System;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Utils.Animation{
    [Serializable] public class AnimEaser : AnimEaserBase{
        /// <summary>
        /// Constructor for an animation easer
        /// </summary>
        /// <param name="curve">The animation curve we want to follow</param>
        /// <param name="duration">The total duration of this animation</param>
        /// <param name="time">The type of time this system will use to update</param>
        public AnimEaser(AnimationCurve curve, float duration, UpdateTime time) : base(curve, duration, time) { }

        /// <summary>
        /// Sets the animation to move forwards/backwards
        /// </summary>
        /// <param name="value">True/False</param>
        public void SetReverse(bool value) => reversed = value;

        /// <summary>
        /// Finds the position in the curve given the parsed value
        /// </summary>
        /// <param name="value">A percentage value between 0-1</param>
        /// <returns>The position in the curve</returns>
        public float Trim(float value) => Value = Curve.Evaluate(value);
        
        /// <summary>
        /// Finds the position in the curve given the parsed value
        /// </summary>
        /// <param name="value">A value (In seconds)</param>
        /// <returns>The position in the curve</returns>
        public float TrimInSeconds(float value) => Value = Curve.Evaluate(Mathf.Clamp01(value / Duration));

        /// <summary>
        /// Runs the animation
        /// </summary>
        /// <returns>The position in the curve</returns>
        public float Play(){
            Elapsed += Time;
            Value = Curve.Evaluate(Percent);
            return Value;
        }
    }
}