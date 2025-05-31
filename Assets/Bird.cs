using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AstralCandle.Utilities;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Tst{
    public class Bird : MonoBehaviour{    
        [SerializeField] bool showDebug = false;
        [SerializeField] Transform COM;
        [SerializeField] Wing[] wings = new Wing[2];
        [SerializeField] Offset[] wingsOffset = new Offset[2];
        [SerializeField] float flapFrq = 1, flapAmp = 1, flapRange = 2;
        [SerializeField] MinMax<float> directionDeltas = new(0.5f, 0.9f);

        float flapTimer = 0;

        private void OnDrawGizmos() {
            if(!showDebug){ return; }
            
            foreach(Wing w in wings){
                foreach(Offset o in wingsOffset){
                    if(!o.showDebug){ continue; }
                    Gizmos.DrawSphere(w.CalcOffset(COM, o.offset), 0.1f);
                }
            }
        }

        protected virtual void Update(){
            flapTimer += Time.deltaTime * flapFrq;

            float flap = Mathf.Lerp(Mathf.Sin(flapTimer) / flapAmp * flapRange, 0, DirectionToGround(-1, .85f));

            Vector3 newOfst = Vector3.Lerp(wingsOffset[1].offset + new Vector3(0, flap), wingsOffset[0].offset, DirectionToGround());

            foreach(Wing w in wings){ w.SetPosition(COM, newOfst); }
        }

        float DirectionToGround(float min = 0, float max = 1) => UFunc.Remap(Vector3.Dot(-COM.right, -Vector3.up), min, max, 0, 1);

        [Serializable] public class Wing{
            [SerializeField] Transform wingBone;
            [SerializeField] bool inverseOffset = false;

            public Vector3 CalcOffset(Transform COM, Vector3 offset){
                offset.z = inverseOffset? -offset.z: offset.z;
                return COM.TransformPoint(offset);
            }

            public void SetPosition(Transform COM, Vector3 newOfst){
                wingBone.position = CalcOffset(COM, newOfst);
            }
        }
        [Serializable] public class Offset{
            public string name;
            public bool showDebug = false;
            public Vector3 offset;
        }
    }
}