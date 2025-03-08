using System.Collections;
using System.Collections.Generic;
using AstralCandle.EntitySystem;
using AstralCandle.Input;
using AstralCandle.Utilities;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle{
    public class DefaultController : MonoBehaviour, ISteering{    
        [SerializeField] UserInput input;
        [SerializeField] float maxSpeed, force;
        [SerializeField, Range(0, 1)] float slowMultiplier = 0.1f;
        
        [SerializeField] float angularSmoothing = 0.1f;
        Quaternion smRot, smRotVel;

        public Vector3 WorldPosition => Collider.bounds.center;
        public float MaxSpeed => GetSpeed();
        public float MaxForce => UFunc.Remap(Vector3.Dot(transform.forward, -Vector3.up), -1, 1, .001f, force);
        public float ReverseFactor => 1;

        public Vector3 Velocity { get; set; }
        public Vector3 LookDirection => transform.forward;

        Collider _col;
        public Collider Collider => _col ??= GetComponent<Collider>();
        CameraController _c;
        CameraController Camera => _c ??= ISingleton<CameraController>.Instance;
        ISteering _s;
        ISteering Steer => _s ??= this;


        float GetSpeed() => UFunc.Remap(Vector3.Dot(transform.forward, -Vector3.up), -1, 1, slowMultiplier, 1) * maxSpeed;

        void Start()
        {
            Camera.Take(CameraController.Mode.Pivot, transform, new Vector3(0, 0, -5), Vector3.zero, 45, 85);
            smRot = transform.rotation;
        }

        void FixedUpdate(){
            Vector3 inputDir = new(0, 0, Mathf.Clamp01(input.InputVelocity.y));
            Quaternion camXY = Quaternion.Euler(Camera.transform.eulerAngles.x, Camera.transform.eulerAngles.y, 0);
            Vector3 desiredMotion = camXY * inputDir;

            Vector3 dir = transform.TransformDirection(new(input.InputVelocity.x, 0, input.InputVelocity.y));
            transform.position += Steer.Result(Steer.Seek(transform.position + desiredMotion.normalized)) * Time.fixedDeltaTime;

            smRot = UFunc.SmoothDampRotation(smRot, Quaternion.LookRotation(Steer.Velocity.normalized), ref smRotVel, angularSmoothing, Mathf.Infinity, Time.fixedDeltaTime);
            transform.rotation = smRot;            
        }
    }
}