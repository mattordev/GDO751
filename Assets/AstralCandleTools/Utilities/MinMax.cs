using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ©️2024-2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Utilities{    
    /// <summary>
    /// A simple class allowing for any value contain a Min & Max value
    /// </summary>
    [Serializable] public class MinMaxPlus<T> : MinMax<T> where T: IComparable<T>{
        /// <summary>
        /// The concurrent value
        /// </summary>
        [HideInInspector] public T current;

        public MinMaxPlus(T min, T max, T current = default) : base(min, max) => this.current = current;
    }
    /// <summary>
    /// A simple class allowing for any value contain a Min & Max value
    /// </summary>
    [Serializable] public class MinMax<T> where T: IComparable<T>{
        /// <summary>
        /// The minimum possible value
        /// </summary>
        [Tooltip("The minimum possible value")] public T min;
        /// <summary>
        /// The maximum possible value
        /// </summary>
        [Tooltip("The maximum possible value")] public T max;

        /// <summary>
        /// Constructor for MinMax
        /// </summary>
        /// <param name="min">The minimum possible value</param>
        /// <param name="max">The maximum possible value</param>
        public MinMax(T min, T max){
            this.min = min;
            this.max = max;
        }
    }    
}