using System.Collections;
using System.Collections.Generic;
using AstralCandle.Input;
using AstralCandle.Utilities;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Bird{
    public class BirdMaster : MonoBehaviour{    
        [SerializeField] Vector3 offset;
        BirdAnimProfile<BirdMaster>[] profiles;

        public Transform planet;
        public bool isGrounded;

        Velocity _v;
        public Velocity Velocity => _v ??= new(transform.position);

        CameraController _c;
        CameraController Camera => _c ??= ISingleton<CameraController>.Instance;


        void Awake() => profiles = GetComponents<BirdAnimProfile<BirdMaster>>();

        void Start()
        {
            Camera.Take(CameraController.Mode.Pivot, transform, offset, Vector3.zero, 45, 85);
        }

        void LateUpdate(){
            for(int i = 0; i < profiles.Length; i++){
                profiles[i].Run(this, Time.deltaTime);
            }
        }

        void FixedUpdate() => Velocity.CalculateVelocity(transform.position);

        /// <summary>
        /// Calculates a "power" value based on the orientation to the target
        /// </summary>
        /// <param name="min">Clamped dot product minimum</param>
        /// <param name="max">Clamped dot product maximum</param>
        /// <returns>Value between 0 and 1 resembling the power</returns>
        public float DirectionToGround(float min = 0, float max = 1) => UFunc.Remap(Vector3.Dot(-transform.right, planet? (transform.position - planet.position).normalized : -Vector3.up), min, max, 0, 1);
    }
}