using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.Utils{
    public static class QuaternionExtension{
        public static void SmoothDamp(ref this Quaternion current, Quaternion target, ref Quaternion currentAngularVelocity, float smoothTime, float maxSpeed = float.PositiveInfinity, float? deltaTime = null){
            float dt = deltaTime ?? Time.deltaTime;
            float dot = Quaternion.Dot(current, target);
            float invert = dot > 0 ? 1 : -1;
            target.x *= invert;
            target.y *= invert;
            target.z *= invert;
            target.w *= invert;

            Vector4 rslt = new Vector4(
                Mathf.SmoothDamp(current.x, target.x, ref currentAngularVelocity.x, smoothTime, maxSpeed, dt),
                Mathf.SmoothDamp(current.y, target.y, ref currentAngularVelocity.y, smoothTime, maxSpeed, dt),
                Mathf.SmoothDamp(current.z, target.z, ref currentAngularVelocity.z, smoothTime, maxSpeed, dt),
                Mathf.SmoothDamp(current.w, target.w, ref currentAngularVelocity.w, smoothTime, maxSpeed, dt)
            ).normalized;

            Vector4 err = Vector4.Project(new(currentAngularVelocity.x, currentAngularVelocity.y, currentAngularVelocity.z, currentAngularVelocity.w), rslt);
            currentAngularVelocity.x -= err.x;
            currentAngularVelocity.y -= err.y;
            currentAngularVelocity.z -= err.z;
            currentAngularVelocity.w -= err.w;
            
            current = new(rslt.x, rslt.y, rslt.z, rslt.w);
        }
    }
}