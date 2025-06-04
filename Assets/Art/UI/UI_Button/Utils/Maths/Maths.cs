using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ©️YEARHERE Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Utils{
    public class Maths<T> : MonoBehaviour{
        [Serializable] public struct MinMax{
            /// <summary>
            /// Minimum value
            /// </summary>
            [Tooltip("Minimum value")] public T min;
            /// <summary>
            /// Maximum value
            /// </summary>
            [Tooltip("Maximum value")] public T max;
            /// <summary>
            /// Constructor for a minmax class
            /// </summary>
            /// <param name="_min">Minimum value</param>
            /// <param name="_max">Maximum value</param>
            public MinMax(T _min, T _max){
                min = _min;
                max = _max;
            }
        }
        
        /// <summary>
        /// Remaps an input value from the fromMin to fromMax range and converts it and maps it to a new set of values between toMin and toMax
        /// </summary>
        /// <param name="inputValue">The value to be modified</param>
        /// <param name="fromMin">The minimum possible raw value</param>
        /// <param name="fromMax">The maximum possible raw value</param>
        /// <param name="toMin">The minimum new value</param>
        /// <param name="toMax">The maximum new value</param>
        /// <param name="doClamp">Clamps the result between toMin and toMax</param>
        /// <returns>A remapped value using toMin & toMax as a referencing point</returns>
        public static float Remap(float inputValue, float fromMin, float fromMax, float toMin, float toMax, bool doClamp = true){
            fromMin = Mathf.Min(fromMin, fromMax);
            fromMax = Mathf.Max(fromMin, fromMax);

            float i = (((inputValue - fromMin) / (fromMax - fromMin)) * (toMax - toMin) + toMin);
            if(doClamp){ i = Mathf.Clamp(i, toMin, toMax); }
            if(toMax < toMin){ i = toMin + toMax - i; } // Inverts it to ensure toMax is always greater
            return i;
        } 

        public void Clamp(T a, T b)
        {

        }
    }
}