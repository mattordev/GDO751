using System;
using System.Drawing;
using UnityEngine;

/// <summary>
/// ©️2024-2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Utilities{
    public static class UFunc{
        /// <summary>
        /// Allows for a function to run if the random number is less than the parsed chance value
        /// </summary>
        /// <param name="chance">The likelyhood of being true</param>
        /// <param name="func">A function to be called if the roll was successful</param>
        /// <returns>True/False</returns>
        public static (bool, float) Roll(float chance, Func<bool> func = null){
            float rnd = UnityEngine.Random.Range(0, 1);
            bool success = rnd <= chance;
            
            if(success){ func?.Invoke(); }
            return (success, rnd);
        }

        /// <summary>
        /// Clamps an angle between 0, 360
        /// </summary>
        /// <param name="angle">Current angle</param>
        /// <param name="min">Min angle</param>
        /// <param name="max">Max angle</param>
        /// <returns>The clamped angle</returns>
        public static float ClampAngle(float angle, float min, float max){
            if (angle > 180f) { angle -= 360; }
            angle = Mathf.Clamp(angle, min, max);

            if (angle < 0f) { angle += 360; }
            return angle;
        }
    
        /// <summary>
        /// Useful for creating "Wrap-around" values between 0 and the maxValue. (By default will iterate upwards)
        /// </summary>
        /// <param name="value">The value we wish to modify</param>
        /// <param name="maxValue">The max value </param>
        /// <param name="doesReverse">Reverses the operation if true</param>
        public static void ModulusCounter(ref int value, int maxValue, bool doesReverse = false) => value = (value + (doesReverse? -1 + maxValue: +1)) % maxValue;
    
        /// <summary>
        /// Calculates the radian position offset to create a circle out of vectors
        /// </summary>
        /// <param name="index">The concurrent point out of them all we are calculating for</param>
        /// <param name="resolution">The number of points this circle has</param>
        /// <returns>Radian orientated vector2 offset for part of a circle. (CAUTION: This results in only an X,Y angle. If wanting X,Z for example. Ensure you convert it over!)</returns>
        public static Vector2 GetCircleDirection(int index, int resolution){
            float angle = (float)index * 2 * Mathf.PI / resolution;
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        public static Vector2[] GetCircleDirectionsAroundAxis(int resolution, Vector2 axis, float completeness){
            Vector2[] directions = new Vector2[resolution];
            float angleStep = completeness * 2 * Mathf.PI / resolution;
            float angleOffset = (resolution % 2 == 0)? angleStep / 2 : 0;
            float middleIndex = resolution / 2;
            for(int i = 0; i < resolution; i++){
                float angle = (i - middleIndex) * angleStep + angleOffset;
                Vector2 p = new(Mathf.Cos(angle), Mathf.Sin(angle));
                Quaternion rot = Quaternion.FromToRotation(Vector2.right, axis);
                directions[i] = rot * p;
            }
            return directions;
        }
        
        public static Quaternion SmoothDampRotation(Quaternion current, Quaternion target, ref Quaternion currentAngularVelocity, float smoothTime, float? speed = null, float? deltaTime = null){
            float s = speed ?? Mathf.Infinity;
            float delta = deltaTime ?? Time.deltaTime;

            if(Quaternion.Dot(current, target) < 0){ target = new(-target.x, -target.y, -target.z, -target.w); }
            static Vector4 Convert(Quaternion v) => new(v.x, v.y, v.z, v.w);
            Vector4 curVec = Convert(current);
            Vector4 trgtVec = Convert(target);
            Vector4 velocity = Convert(currentAngularVelocity);

            curVec.x = Mathf.SmoothDamp(curVec.x, trgtVec.x, ref velocity.x, smoothTime, s, delta);
            curVec.y = Mathf.SmoothDamp(curVec.y, trgtVec.y, ref velocity.y, smoothTime, s, delta);
            curVec.z = Mathf.SmoothDamp(curVec.z, trgtVec.z, ref velocity.z, smoothTime, s, delta);
            curVec.w = Mathf.SmoothDamp(curVec.w, trgtVec.w, ref velocity.w, smoothTime, s, delta);

            currentAngularVelocity = new(velocity.x, velocity.y, velocity.z, velocity.w);


            return new Quaternion(curVec.x, curVec.y, curVec.z, curVec.w).normalized;
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
    }
}