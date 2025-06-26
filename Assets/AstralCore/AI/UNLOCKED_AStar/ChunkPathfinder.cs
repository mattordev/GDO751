using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AstralCore.ChunkSystem;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.AI{
    public class ChunkPathfinder : Pathfinder{
        Chunks chunks;
        public ChunkPathfinder(Chunks chunks, int cellSize, Axis axis, int maxIterations = 1000, int? heapCapacity = null) : base(cellSize, axis, maxIterations, heapCapacity){
            this.chunks = chunks;
        }
        
        protected override int CalculateDistance(GraphNode from, GraphNode to){
            int x = Mathf.FloorToInt(Mathf.Abs(from.position.x - to.position.x));
            int y = Mathf.FloorToInt(Mathf.Abs(from.position.y - to.position.y));
            int z = Mathf.FloorToInt(Mathf.Abs(from.position.z - to.position.z));

            int minXYZ = Mathf.Min(x, Mathf.Min(y, z));
            int midXYZ = Mathf.Max(Mathf.Min(x, y), Mathf.Min(Mathf.Max(x, y), z));

            return minXYZ + DIAGONAL_COST * (midXYZ - minXYZ) + STRAIGHT * (x + y + z - midXYZ);
        }

        protected override bool IsObstacle(Vector3Int position)
        {
            bool isObstacle = false;
            chunks.Get<ChunkContainer<PathfinderObstacle>>(position, c =>
            {
                c.Iterate(ob =>
                {
                    if (!isObstacle && ob.Contains(position)) { isObstacle = true; }
                });
            }
            );
            return isObstacle;
        }
    }
}