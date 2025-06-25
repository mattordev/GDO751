using System.Collections;
using System.Collections.Generic;
using AstralCore.ChunkSystem;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.AI{
    public class TstAStar : MonoBehaviour
    {
        [SerializeField] ChunkManager chunkManager;
        ChunkPathfinder p;
        [SerializeField] Transform start, end;

        List<GraphNode> path;

        void Start(){
            p = new(chunkManager.Chunks, 1, Pathfinder.Axis.XYZ);
        }

        void Update()
        {
            path = p.Search(start.position, end.position);
        }


        void OnDrawGizmos()
        {
            for (int i = 0; i < path?.Count; i++){
                Vector3Int position = path[i];
                Gizmos.DrawWireCube(position, Vector3.one);
            }
        }
    }
}