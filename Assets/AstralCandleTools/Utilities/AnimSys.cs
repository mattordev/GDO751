using UnityEngine;

/// <summary>
/// ©️2024-2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Utilities{
    /// <summary>
    /// Allows us to create various powerful animations for a variety of purposes
    /// </summary>
    [System.Serializable] public class AnimSys{
        [SerializeField, Tooltip("This will control the value through time")] AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField, Tooltip("The duration of this animation")] float duration;

        float _elapsed;
        public float Elapsed{ get => _elapsed; private set => _elapsed = Mathf.Clamp(value, 0, duration); }
        
        /// <summary>
        /// Value between 0-1 resembling how complete we are in our animation
        /// </summary>
        public float Percent{ get; private set; }
        
        /// <summary>
        /// The resulting value
        /// </summary>
        public float Value{ get; private set; }

        /// <summary>
        /// Constructor for our animation system
        /// </summary>
        /// <param name="curve">This will control the value through time</param>
        /// <param name="duration">The duration of this animation</param>
        public AnimSys(AnimationCurve curve, float duration){
            this.curve = curve;
            this.duration = duration;
        }

        /// <summary>
        /// Constructor for our animation system
        /// </summary>
        /// <param name="system">Parsed animation system</param>
        public AnimSys(AnimSys system){
            this.curve = system.curve;
            this.duration = system.duration;
        }
        

        /// <summary>
        /// Resets the time back to 0
        /// </summary>
        /// <param name="reverse">If true, sets time to 1</param>
        public void Reset(bool reverse = false) => Elapsed = (!reverse)? 0 : 1;

        /// <summary>
        /// Plays the animation
        /// </summary>
        /// <param name="reverse">If true, sets time to 1</param>
        /// <returns>Value between 0-1 resembling how complete we are in our animation</returns>
        public float Play(float delta, bool reverse = false){
            Elapsed += (!reverse)? delta : -delta;
            Percent = Mathf.Clamp01(Elapsed / duration);
            Value = curve.Evaluate(Percent);
            return Value;
        }
    }
}