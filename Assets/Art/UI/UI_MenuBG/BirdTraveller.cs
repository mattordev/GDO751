using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

/// <summary>
/// ©️YEARHERE Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.UIAI{
    public class BirdTraveller : TurtleTraveller{
        [SerializeField] float strideDistance;
        [SerializeField] Transform footprintStorage;
        [SerializeField] Feet[] feet;

        float timeTillStride;

        bool toggleFoot = false;

        protected override void Update() {
            base.Update();
            if (Time.time >= timeTillStride && feet[toggleFoot ? 1 : 0].Stride(Transform, canvas, footprintStorage, strideDistance)) {
                toggleFoot = !toggleFoot;

                timeTillStride = Time.time + 1;
            }
        }
        [Serializable]
        public class Feet{

            [SerializeField] Footprints _feet;

            RectTransform _t;
            public RectTransform T => _feet.transform as RectTransform;

            Vector2? _prvPos;
            public Vector2 PreviousPosition => _prvPos ??= T.position;

            public bool Stride(RectTransform Transform, RectTransform canvas, Transform footprintStorage, float strideDistance){
                if (Vector2.Distance(T.position, PreviousPosition) >= strideDistance){
                    _prvPos = Transform.position;
                    Instantiate(_feet, Transform.TransformPoint(T.anchoredPosition), T.rotation, footprintStorage).Activate();
                    return true;
                }
                return false;
            }
            
            
        }
    }
}