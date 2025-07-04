using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle{
    [System.Serializable] public abstract class BirdAnimProfile<T> : MonoBehaviour{    
        [SerializeField] bool showDebug;
        [SerializeField] protected Bone[] bones;

        public abstract void Run(T master, float delta);
        public abstract void Debug();

        void OnDrawGizmos(){
            if(!showDebug){ return; }
            Debug();
        }
    }

    [System.Serializable] public class Bone{
        public Transform bone;
        public bool invertX, invertY, invertZ;

        public Vector3 Invert(Vector3 vector){
            vector.x = invertX? -vector.x : vector.x;
            vector.y = invertY? -vector.y : vector.y;
            vector.z = invertZ? -vector.z : vector.z;
            return vector;
        }


        /// <summary>
        /// Sets position of bone
        /// </summary>
        /// <param name="COM">Centre of mass to relate the offset off of</param>
        /// <param name="offset">The offset position from the COM</param>
        public void Set(Transform COM, Vector3 offset) => bone.position = COM.TransformPoint(Invert(offset));

        public void SetRotation(Quaternion rotation) => bone.localRotation = rotation; 
    }
    
}