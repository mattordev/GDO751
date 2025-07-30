using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.Utils{
    /// <summary>
    /// Useful for spatial queries | Generates and contains directions
    /// </summary>
    public readonly struct Directions : IEnumerable<Vector3>
    {
        /// <summary>
        /// Generated directions
        /// </summary>
        readonly Vector3[] DIRECTIONS;
        public Directions(bool xEnabled, bool yEnabled, bool zEnabled, int resolution = 1)
        {
            static int GetToggledAxis(params bool[] axis) => axis.Count(v => v); // Counts all axis which is marked true
            static bool IsOrigin(Vector3 v, float epsilon = 1e-5f) => Mathf.Abs(v.x) < epsilon && Mathf.Abs(v.y) < epsilon && Mathf.Abs(v.z) < epsilon;

            int samplesPerAxis = 2 * resolution + 1;
            float stepSize = 2f / (samplesPerAxis - 1);

            DIRECTIONS = new Vector3[(int)Mathf.Pow(samplesPerAxis, GetToggledAxis(xEnabled, yEnabled, zEnabled)) - 1]; // Calculates number of directions to search

            int counter = 0;
            for (int xi = 0; xi < (xEnabled ? samplesPerAxis : 1); xi++)
            {
                float x = xEnabled ? -1f + xi * stepSize : 0f;
                for (int yi = 0; yi < (yEnabled ? samplesPerAxis : 1); yi++)
                {
                    float y = yEnabled ? -1f + yi * stepSize : 0f;

                    for (int zi = 0; zi < (zEnabled ? samplesPerAxis : 1); zi++)
                    {
                        float z = zEnabled ? -1f + zi * stepSize : 0f;
                        if (IsOrigin(new(x, y, z))) { continue; } // Origin
                        DIRECTIONS[counter++] = new Vector3(x, y, z).normalized;
                    }
                }
            }
        }

        public static implicit operator Vector3[](Directions d) => d.DIRECTIONS;


        /// <summary>
        /// Generates directions for the XY axis | Assumes resolution of 1
        /// </summary>
        public static Directions XY => new(true, true, false);
        /// <summary>
        /// Generates directions for the XYZ axis | Assumes resolution of 1
        /// </summary>
        public static Directions XYZ => new(true, true, true);
        /// <summary>
        /// Generates directions for the XZ axis | Assumes resolution of 1
        /// </summary>
        public static Directions XZ => new(true, false, true);

        public IEnumerator<Vector3> GetEnumerator() => ((IEnumerable<Vector3>)DIRECTIONS).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => DIRECTIONS.GetEnumerator();

        public override string ToString() => string.Join(",", DIRECTIONS);

        /// <summary>
        /// The number of generated directions
        /// </summary>
        public int Count => DIRECTIONS.Length;
    }        
}