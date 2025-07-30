using System.Collections;
using System.Collections.Generic;
using AstralCandle.Character;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle{
    [RequireComponent(typeof(SphereCollider))]
    public class DeathDetector : MonoBehaviour{
        [SerializeField] bool showDebug;
        [SerializeField] float detectionDistance;
        [SerializeField] float hitDelta = 0.5f;
        [SerializeField] LayerMask obstacles;
        [SerializeField] bool reloadOnDestroy;
        SphereCollider _col;
        SphereCollider Collider => _col ??= GetComponent<SphereCollider>();

        BirdMotor _m;
        BirdMotor motor => _m ??= GetComponent<BirdMotor>();


        void FixedUpdate(){
            Vector3 direction = transform.forward * detectionDistance;
            Ray ray = new(Collider.ClosestPoint(direction), direction.normalized);
            if (Physics.SphereCast(ray, Collider.radius, out RaycastHit hit, direction.magnitude, obstacles) && hit.distance <= hitDelta){
                motor.DestroyBird = true;
                if (reloadOnDestroy){ AstralCore.SceneManagement.SceneManager.Instance.ReloadScene(); }
            }
        }

        void OnDrawGizmos()
        {
            if (!showDebug) { return; }
            Gizmos.DrawRay(transform.position, transform.forward * detectionDistance);
            Gizmos.DrawWireSphere(transform.position + (transform.forward * (detectionDistance - Collider.radius)), Collider.radius);
        }
    }
}