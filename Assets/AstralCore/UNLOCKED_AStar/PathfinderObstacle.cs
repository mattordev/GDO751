using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using AstralCore.ChunkSystem;



#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.AI{
    public class PathfinderObstacle : MonoBehaviour, IChunkTrackable<PathfinderObstacle>{
        [SerializeField] bool showDebug;
        [SerializeField] Vector3 positionOffset;
        [SerializeField] Vector3 scale = Vector3.one;
        [SerializeField] ChunkManager chunkManager;

        public PathfinderObstacle TrackableComponent => this;

        public ChunkContainer<PathfinderObstacle> Chunk { get; set; }

        Matrix4x4 Matrix {
            get{
                Vector3 c = transform.TransformPoint(positionOffset);
                Vector3 s = scale;
                Quaternion r = transform.rotation;
                return Matrix4x4.TRS(c,r,s);
            }
        }

        public bool Contains(Vector3 position){
            static bool LessThan(params float[] _p) => _p.All(v => Mathf.Abs(v) <= 0.5f);
            Vector3 local = Matrix.inverse.MultiplyPoint3x4(position);
            return LessThan(local.x, local.y, local.z);
        }

        public void OnDestroy() => Chunk?.Unregister(TrackableComponent);



        void Start()
        {
            (this as IChunkTrackable<PathfinderObstacle>).UpdateChunk(chunkManager.Chunks, Matrix.GetPosition());
            Debug.Log(Chunk);
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            if (!showDebug) { return; }
            Handles.matrix = Matrix;
            Handles.color = Color.red;
            Handles.DrawWireCube(Vector3.zero, Vector3.one);
            Handles.matrix = Matrix4x4.identity;
        }
        #endif
    }
}