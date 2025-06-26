using System.Collections;
using System.Collections.Generic;
using AstralCore.AI;
using AstralCore.AI.SteeringSystem;
using AstralCore.ChunkSystem;
using UnityEngine;
using System.Linq;
using UnityEditor.Rendering;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle{
    public class TstSteer : Steering3D{
        [SerializeField] bool showDebug;
        [SerializeField] int cellSize = 1;
        [SerializeField] ChunkManager chunkManager;
        ChunkPathfinder p;
        [SerializeField] Transform start, end;
        [SerializeField] float delta;

        bool flip = false;
        (Transform a, Transform b) Points => !flip ? (start, end) : (end, start);
        Queue<Vector3Int> path = new();
        List<GraphNode> pathList;

        Vector3? targetPosition = null;
        protected override Vector3[] GetForces()
        {
            Vector3 seek = SteeringFuncs.Seek(targetPosition ?? transform.position, this);
            return new Vector3[] { seek };
        }

        protected override void InitOnStart()
        {
            p = new(chunkManager.Chunks, cellSize, Pathfinder.Axis.XYZ);
        }

        protected override void ProcessFixedUpdate()
        {
            if ((path == null || path.Count == 0) && targetPosition == null)
            {
                pathList = p.Search(Vector3Int.FloorToInt(Points.a.position), Vector3Int.FloorToInt(Points.b.position));
                foreach (Vector3Int v in pathList) { path.Enqueue(v); }
                flip = !flip;
            }
            if (path?.Count > 0 && targetPosition == null) { targetPosition = path.Dequeue(); }
            else if (targetPosition != null && Vector3.Distance((Vector3)targetPosition, transform.position) <= delta)
            {
                targetPosition = null;
            }


            transform.position += Velocity * Time.fixedDeltaTime;
        }

        void OnDrawGizmos(){
            if (!showDebug || pathList == null) { return; }
            for (int i = 0; i < pathList.Count; i++){
                Vector3Int position = pathList[i];
                Gizmos.DrawWireCube(position, Vector3.one * cellSize);
            }
        }
    }
}