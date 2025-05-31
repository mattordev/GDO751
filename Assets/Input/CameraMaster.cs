using System.Collections;
using System.Collections.Generic;
using AstralCandle.Game;
using AstralCandle.Input;
using AstralCandle.Utilities;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Input{
    public sealed class CameraMaster : CameraController{
        [SerializeField] UserInput input;
        [SerializeField] MinMax<float> clampedAngles = new(-40, 85);
        Vector2 mouseVelocity = default;
        float yaw, pitch, zoomPercent = 0.5f;

        PlanetManager _p;
        PlanetManager PlanetManager => _p ??= ISingleton<PlanetManager>.Instance;

        void OnEnable(){
            input.OnLook += GetMouse;
            input.OnZoom += ZoomAmount;
        }
        void OnDisable(){
            input.OnLook -= GetMouse;
            input.OnZoom -= ZoomAmount;
        }

        void GetMouse(bool usingGamepad, Vector2 velocity) => mouseVelocity = velocity * Time.fixedDeltaTime;
        void ZoomAmount(float value) => zoomPercent = Mathf.Clamp01(zoomPercent - value * input.ZoomSensitivity * Time.fixedDeltaTime);

        protected override (Vector3 position, Quaternion rotation, Vector3? offset) Stationary(Transform target, Vector3 worldOffset, Vector3 localOffset){
            Vector3 offset = target.position + target.TransformDirection(worldOffset);
            Vector3 dir = (target.position - offset).normalized;
            return(
                offset + target.TransformDirection(localOffset),
                Quaternion.LookRotation(dir),
                null
            );
        }
        protected override (Vector3 position, Quaternion rotation, Vector3? offset) Pivot(Transform target, Vector3 worldOffset){
            // Sorting input orientation
            yaw += mouseVelocity.x * input.LookSensitivity.x;
            pitch -= (mouseVelocity.y * input.LookSensitivity.y) * (input.InvertY? -1: 1);
            pitch = Mathf.Clamp(pitch, clampedAngles.min, clampedAngles.max);

            // Translating local orientation to world orientation
            Quaternion rot = Quaternion.Euler(pitch, yaw, 0);
            if(PlanetManager){
                Transform planet = PlanetManager?.GetNearestPlanet(target);
                Vector3 relativeDirection = (target.position - planet.position).normalized;
                Quaternion planetAlignment = Quaternion.FromToRotation(Vector3.up, relativeDirection);
                rot *= planetAlignment;
            }
            // Quaternion localRot = Quaternion.Euler(pitch, yaw, 0);

            // Quaternion camRot = planetAlignment * localRot;
            return (
                target.position,
                rot,
                worldOffset
            );
        }
        protected override (Vector3 position, Quaternion rotation, Vector3? offset) Spotlight(Transform target, Vector3 worldOffset){
            Vector3 dir = (target.position - worldOffset).normalized;
            return (worldOffset, Quaternion.LookRotation(dir), null);
        }
        protected override float Zoom(float min, float max) => Mathf.Lerp(min, max, zoomPercent);
    }
}