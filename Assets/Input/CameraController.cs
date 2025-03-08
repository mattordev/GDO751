using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AstralCandle.Utilities;
using System.Linq;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Input{
    public abstract class CameraController : MonoBehaviour, ISingleton<CameraController>{
        [SerializeField] float positionSmoothing = .1f;
        [SerializeField] float rotationSmoothing = .1f;
        [SerializeField] float zoomSmoothing = .1f;

        Camera _cam;
        protected Camera Camera => _cam ??= Camera.main;

        #region CONTROL
        /// <summary>
        /// Last known key to be added to the stack
        /// </summary>
        Transform lastKey = null; 

        readonly Dictionary<Transform, (Mode, Vector3, Vector3, float?, float?, float?)> subscribers = new(); // Acts as a stack where last in is the active user

        Vector3 trgtPos, curPos, posVel, smoothOfst, ofstVel;
        Quaternion trgtRot, curRot, rotVel;
        Vector3? ofst;
        float curZoom = 100, zoomSmooth, zoomVel;

        /// <summary>
        /// Adds a camera subscriber to the stack
        /// </summary>
        /// <param name="mode">The mode we want the camera to operate as</param>
        /// <param name="target">The object we want to focus on</param>
        /// <param name="worldOffset">The main offset from the target. (IF MODE IS 'SPOTLIGHT' THEN THIS BECOMES THE ACTUAL POSITION)</param>
        /// <param name="localOffset">For when the camera is looking in the right direction but for example needs to be higher</param>
        /// <param name="zoom">How zoomed in to the target are we?</param>
        /// <param name="rotationSmoothing">How fast do we react to rotation</param>
        public void Take(Mode mode, Transform target, Vector3 worldOffset, Vector3 localOffset, float? zoomMin = null, float? zoomMax = null, float? rotationSmoothing = null){
            if(subscribers.ContainsKey(target)){
                subscribers[target] = (mode, worldOffset, localOffset, zoomMin, zoomMax, rotationSmoothing);
                return;
            }
            subscribers.Add(target, (mode, worldOffset, localOffset, zoomMin, zoomMax, rotationSmoothing));
            lastKey = target;
        }
        /// <summary>
        /// Attempts to release the target from the stack
        /// </summary>
        /// <param name="target">The target we want to no longer be tracked by the camera</param>
        public void Release(Transform target){
            if(subscribers.ContainsKey(target)){ 
                subscribers.Remove(target); 
                // Gets the last index in the stack
                if(target == lastKey){ lastKey = subscribers.Keys.ToArray()[^1]; }
            }
        }

        void ProcessSubscribers(){
            if(!lastKey){ return; }
            subscribers.TryGetValue(lastKey, out (Mode, Vector3, Vector3, float?, float?, float?) v);
            if(lastKey != null){ 
                switch(v.Item1){
                    case Mode.Stationary:
                        (trgtPos, trgtRot, ofst) = Stationary(lastKey, v.Item2, v.Item3);
                        break;
                    case Mode.Pivot:
                        (trgtPos, trgtRot, ofst) = Pivot(lastKey, v.Item2);
                        break;
                    case Mode.Spotlight:
                        (trgtPos, trgtRot, ofst) = Spotlight(lastKey, v.Item2);
                        break;
                }
            }

            

            transform.rotation = curRot;
            transform.position = curPos + transform.TransformDirection(smoothOfst);
            Camera.fieldOfView = zoomSmooth;
        }

        /// <summary>
        /// The camera stays still, doesn't move at all once at position
        /// </summary>
        /// <param name="target">The object we want to focus on</param>
        /// <param name="worldOffset">The main offset from the target
        /// <param name="localOffset">For when the camera is looking in the right direction but for example needs to be higher</param>
        /// <returns>Position, Rotation and Offset (If any)</returns>
        protected abstract (Vector3 position, Quaternion rotation, Vector3? offset) Stationary(Transform target, Vector3 worldOffset, Vector3 localOffset);
        /// <summary>
        /// Allows for player input to rotate the camera around the target
        /// </summary>
        /// <param name="target">The object we want to focus on</param>
        /// <param name="worldOffset">The main offset from the target
        /// <returns>Position, Rotation and Offset (If any)</returns>
        protected abstract (Vector3 position, Quaternion rotation, Vector3? offset) Pivot(Transform target, Vector3 worldOffset);
        /// <summary>
        /// Like stationary, but keeps the target in the center of the screen
        /// </summary>
        /// <param name="target">The object we want to focus on</param>
        /// <param name="worldOffset">The real position to move the camera to
        /// <returns>Position, Rotation and Offset (If any)</returns>
        protected abstract (Vector3 position, Quaternion rotation, Vector3? offset) Spotlight(Transform target, Vector3 worldOffset);
        /// <summary>
        /// Using a min & max value, calculate where inbetween to have the camera zoomed in at. (Perhaps used InverseLerp and save the percentage)
        /// </summary>
        /// <param name="min">The minimum possible value</param>
        /// <param name="max">The maximum possible value</param>
        /// <returns>The new zoom amount</returns>
        protected abstract float Zoom(float min, float max);
        #endregion


        void Awake(){
            if(!(this as ISingleton<CameraController>).CreateSingleton(this)){
                Destroy(gameObject);
                return;
            }
            trgtPos = transform.position;
            curPos = trgtPos;
            trgtRot = transform.rotation;
            curRot = trgtRot;
        }
        void LateUpdate() => ProcessSubscribers();

        void FixedUpdate(){
            if(!lastKey){ return; }
            subscribers.TryGetValue(lastKey, out (Mode, Vector3, Vector3, float?, float?, float?) v);
            curRot = UFunc.SmoothDampRotation(curRot, trgtRot, ref rotVel, v.Item6 ?? rotationSmoothing, Mathf.Infinity, Time.fixedDeltaTime);
            curPos = Vector3.SmoothDamp(curPos, trgtPos, ref posVel, positionSmoothing, Mathf.Infinity, Time.fixedDeltaTime);

            smoothOfst = Vector3.SmoothDamp(smoothOfst, ofst ?? Vector3.zero, ref ofstVel, positionSmoothing, Mathf.Infinity, Time.fixedDeltaTime);

            (float? zMi, float? zMx) = lastKey && v.Item4 != null && v.Item5 != null? (subscribers[lastKey].Item4, subscribers[lastKey].Item5) : (curZoom, curZoom);
            zoomSmooth = Mathf.SmoothDamp(zoomSmooth, Zoom((float)zMi, (float)zMx), ref zoomVel, zoomSmoothing, Mathf.Infinity, Time.fixedDeltaTime);
        }


        /// <summary>
        /// Various different camera modes to invoke different behaviours
        /// </summary>
        public enum Mode{
            /// <summary>
            /// Fixed position
            /// </summary>
            Stationary,
            /// <summary>
            /// Follow & Allow pivot input
            /// </summary>
            Pivot,
            /// <summary>
            /// Like stationary, but keeps target in center of screen
            /// </summary>
            Spotlight
        }
    }
}