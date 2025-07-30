using System.Collections;
using System.Collections.Generic;
using AstralCandle.Character;
using AstralCandle.Input;
using AstralCore.Utils;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Bird{
    public class BirdMaster : MonoBehaviour
    {
        [SerializeField] BirdMotor motor;
        BirdAnimProfile<BirdMaster>[] profiles;
        public bool isGrounded;

        public BirdMotor Motor => motor;

        private void Awake() {
            profiles = GetComponents<BirdAnimProfile<BirdMaster>>();
        }

        void LateUpdate()
        {
            if(InputSO.Pause){ return; }
            for (int i = 0; i < profiles.Length; i++)
            {
                profiles[i].Run(this, Time.deltaTime);
            }
        }

        public float DirectionToGround(float min, float max) => Methods.Remap(Vector3.Dot(-transform.right, Vector3.down), min, max, 0, 1);
    }
}