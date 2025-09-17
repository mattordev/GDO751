using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ©️YEARHERE Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Character{
    public interface ISpeed{
        /// <summary>
        /// Dictates as a percentage to the assigned max speed how fast we are going
        /// </summary>
        /// <returns>A value between 0-1</returns>
        public float GetPercentSpeed();
    }
}