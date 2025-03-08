using AstralCandle.Utilities;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Game{
    public class Planet : MonoBehaviour{    
        [SerializeField] bool showDebug = false;
        [SerializeField, Tooltip("The attraction force of this planet")] float gravity = -9.81f;
        [SerializeField, Tooltip("The attraction force of this planet")] float attractionRadius = 500;
        public float Gravity{ get => -gravity; set => gravity = value; }

        public Vector3 Attract(Transform obj){
            Vector3 dir = transform.position - obj.position;
            float modifier = 1 - Mathf.Clamp01(dir.magnitude / attractionRadius);
            return Gravity * modifier * dir.normalized;
        }
        public float DistanceToCore(Transform obj) => Vector3.Distance(transform.position, obj.position);


        void Start() => ISingleton<PlanetManager>.Instance?.AddPlanet(this);

        void OnDrawGizmos(){
            if(!showDebug){ return; }
            Gizmos.DrawWireSphere(transform.position, attractionRadius);            
        }
    }
}