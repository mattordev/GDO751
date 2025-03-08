using System.Collections;
using System.Collections.Generic;
using AstralCandle.Utilities;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Game{
    public class World : MonoBehaviour, ISingleton<World>{    
        [SerializeField] bool showGizmos;
        [SerializeField] Transform planet;
        public Transform Planet => planet;
        [SerializeField, Tooltip("How far out can characters go out from the planet?")] float maxAltitudeFromGround = 1;

        Collider _collider = null;
        Collider Collider => _collider ??= Planet?.GetComponent<Collider>();

        void Awake(){
            if(!(this as ISingleton<World>).CreateSingleton(this)){
                Destroy(gameObject);
                return;
            }
        }
        
        void Update(){
            
        }


        void OnDrawGizmosSelected(){
            if(!showGizmos || !Collider){ return; }
            float max = Mathf.Max(Collider.bounds.extents.x, Collider.bounds.extents.y, Collider.bounds.extents.z);
            Gizmos.DrawWireSphere(Collider.bounds.center, max + maxAltitudeFromGround);
        }
    }
}