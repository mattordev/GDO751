using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.Utils{
    public static class Methods
    {
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
        public static float Remap(float inputValue, float fromMin, float fromMax, float toMin, float toMax, bool doClamp = true)
        {
            fromMin = Mathf.Min(fromMin, fromMax);
            fromMax = Mathf.Max(fromMin, fromMax);

            float i = (((inputValue - fromMin) / (fromMax - fromMin)) * (toMax - toMin) + toMin);
            if (doClamp) { i = Mathf.Clamp(i, toMin, toMax); }
            if (toMax < toMin) { i = toMin + toMax - i; } // Inverts it to ensure toMax is always greater
            return i;
        }

        /// <summary>
        /// Useful for creating "Wrap-around" values between 0 and the maxValue. (By default will iterate upwards)
        /// </summary>
        /// <param name="value">The value we wish to modify</param>
        /// <param name="maxValue">The max value </param>
        /// <param name="doesReverse">Reverses the operation if true</param>
        public static void ModulusCounter(ref int value, int maxValue, bool doesReverse = false) => value = (value + (doesReverse ? -1 + maxValue : +1)) % maxValue;
    }
}