using System;
using System.Collections;
using System.Collections.Generic;
using AstralCandle.Character;
using AstralCore.Utils;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Input{
    public class CameraController : Singleton<CameraController>{
        [SerializeField] UserInput input;
        [SerializeField] float positionSmoothing, rotationSmoothing, zoomSmoothing;
        [SerializeField] MinMax<float> clampedAngles = new(-40,85);
        [SerializeField] MinMax<float> zoomAmount = new(30,90);
        [SerializeField] float additiveFOVZoom = 30;
        [SerializeField] Vector3 offset;
        [SerializeField] MinMax<float> bobFreq, bobAmp;
        [SerializeField] float bobLength = 1;

        readonly Data data = new();
        Camera _cam;
        protected Camera Camera => _cam ??= Camera.main;

        TargetProfile target;
        Vector2 mouseVelocity;

        public void Take(ISpeed target) => this.target = new TargetProfile(target);

        (Vector3, Quaternion) Pivot(){
            data.yaw += mouseVelocity.x * input.LookSensitivity.x;
            data.pitch -= mouseVelocity.y * input.LookSensitivity.y * (input.InvertY? -1: 1);
            data.pitch = Mathf.Clamp(data.pitch, clampedAngles.min, clampedAngles.max);
            Quaternion rot = Quaternion.Euler(data.pitch, data.yaw, 0);
            return (target.obj.position, rot);
        }

        #region EVENT SUBSCRIBERS
        void OnEnable(){
            input.OnLook += GetMouse;
            input.OnZoom += ZoomAmount;
        }

        void OnDisable(){
            input.OnLook -= GetMouse;
            input.OnZoom -= ZoomAmount;
        }
        #endregion

        private void ZoomAmount(float value) => data.zoomPercent = Mathf.Clamp01(data.zoomPercent - value * input.ZoomSensitivity * Time.deltaTime);
        private void GetMouse(bool usingGamepad, Vector2 value) => mouseVelocity = value * Time.deltaTime;


        protected override void Awake(){
            CreateSingleton(true);
            data.Init(transform, zoomAmount);
        }

        void LateUpdate() {
            if(InputSO.Pause){ return; }
            data.bobTimer += Time.deltaTime;

            data.UpdateRotation(rotationSmoothing, Time.deltaTime);
            (data.trgtPos, data.trgtRot) = Pivot();

            Vector3 bob = Vector3.LerpUnclamped(
                new(Mathf.Cos(data.bobTimer * bobFreq.min / 2) / bobAmp.min, Mathf.Sin(data.bobTimer * bobFreq.min) / bobAmp.min),
                new(Mathf.Cos(data.bobTimer * bobFreq.max / 2) / bobAmp.max, Mathf.Sin(data.bobTimer * bobFreq.max) / bobAmp.max),
                target.speed.GetPercentSpeed()
            ) * bobLength;
            Vector3 newPos = data.curPos + transform.TransformDirection(offset + bob);
            transform.SetPositionAndRotation(newPos, data.curRot);
            
            data.UpdateZoom(Mathf.Lerp(zoomAmount.min, zoomAmount.max, data.zoomPercent), zoomSmoothing, Time.deltaTime);
            Camera.fieldOfView = data.curZoom + Mathf.Lerp(0, additiveFOVZoom, target.speed.GetPercentSpeed());
        }

        void FixedUpdate(){
            if(InputSO.Pause){ return; }
            if (target.obj == null) { return; }
            data.UpdatePosition(positionSmoothing, Time.fixedDeltaTime);
        }

        readonly struct TargetProfile
        {
            public readonly Transform obj;
            public readonly ISpeed speed;
            public TargetProfile(ISpeed speed){
                this.speed = speed;
                this.obj = (speed as MonoBehaviour).transform;
            }
        }

        class Data{
            public Vector3 curPos, trgtPos, posVel;//, angVel;
            public Quaternion trgtRot, curRot, angVel;
            public float curZoom, zoomVel;
            public float yaw, pitch, zoomPercent = 0.5f;
            public float bobTimer;
            public void Init(Transform t, MinMax<float> zoomAmount)
            {
                curPos = t.position;
                curRot = t.rotation;
                curZoom = Mathf.Lerp(zoomAmount.min, zoomAmount.max, zoomPercent);
            }

            public void UpdatePosition(float smoothing, float delta) => curPos = Vector3.SmoothDamp(curPos, trgtPos, ref posVel, smoothing, Mathf.Infinity, delta);
            public void UpdateRotation(float smoothing, float delta) => curRot.SmoothDamp(trgtRot, ref angVel, smoothing, Mathf.Infinity, delta);
            public void UpdateZoom(float target, float smoothing, float delta) => curZoom = Mathf.SmoothDamp(curZoom, target, ref zoomVel, smoothing, Mathf.Infinity, delta);
        }
    }
}