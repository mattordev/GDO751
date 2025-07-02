using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Character{
    public class BirdPlayer : BirdMotor{
        protected override Vector3[] GetForces() => new Vector3[] { };

        protected override void InitOnStart() { }
    }
}