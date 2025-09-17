using System;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.Utils{
    /// <summary>
    /// Allows for Min/Max definitions 
    /// </summary>
    [Serializable] public struct MinMax<T>{
        public T min, max;
        public MinMax(T min, T max){
            this.min = min;
            this.max = max;
        }
    }
}