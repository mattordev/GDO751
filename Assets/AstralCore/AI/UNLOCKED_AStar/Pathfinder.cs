using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AstralCore.AI;
using AstralCore.Utils;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.AI{
    public abstract class Pathfinder : Pathfinder_OOP<GraphNode> {
        protected const int VERTICAL_COST = 17, DIAGONAL_COST = 14, STRAIGHT = 10;
        Axis axis;
        Gridify gridify;

        public Pathfinder(int cellSize, Axis axis, int maxIterations = 1000, int? heapCapacity = null) : base(maxIterations, heapCapacity){
            gridify = new(cellSize);
            this.axis = axis;
        }


        public override List<GraphNode> Search(GraphNode start, GraphNode end){
            start.position = gridify.Calc(start.position);
            end.position = gridify.Calc(end.position);
            return base.Search(start, end);
        }

        protected override int CalculateDistance(GraphNode from, GraphNode to){
            int x = Mathf.FloorToInt(Mathf.Abs(from.position.x - to.position.x));
            int y = Mathf.FloorToInt(Mathf.Abs(from.position.y - to.position.y));
            int z = Mathf.FloorToInt(Mathf.Abs(from.position.z - to.position.z));

            int minXYZ = Mathf.Min(x, Mathf.Min(y, z));
            int midXYZ = Mathf.Max(Mathf.Min(x, y), Mathf.Min(Mathf.Max(x, y), z));
            // This cost structure (Using vertical) makes the path more stair-like. Removing this makes a* more direct
            return VERTICAL_COST * minXYZ + DIAGONAL_COST * (midXYZ - minXYZ) + STRAIGHT * (x + y + z - midXYZ);
        }

        protected override void SearchNeighbours(GraphNode current, GraphNode end, ref BinaryMinHeap<GraphNode> openlist, ref HashSet<GraphNode> closedlist) {
            for (int i = 0; i < axis.DIRECTIONS.Length; i++){
                Vector3Int direction = axis.DIRECTIONS[i];
                Vector3Int neighbourPosition = current.position + direction * gridify.CELL_SIZE;
                if (closedlist.Contains(neighbourPosition) || IsObstacle(neighbourPosition)){ continue; }

                GraphNode neighbourNode = new(neighbourPosition, current.gCost + CalculateDistance(current.position, neighbourPosition), CalculateDistance(neighbourPosition, end), current);
                openlist.Enqueue(neighbourNode);
            }
        }

        protected abstract bool IsObstacle(Vector3Int position);

        /// <summary>
        /// Used to dictate how neighbours are checked
        /// </summary>
        public readonly struct Axis
        {
            public readonly Vector3Int[] DIRECTIONS;
            public Axis(bool _x, bool _y, bool _z)
            {
                static int GetToggledAxis(params bool[] axis) => axis.Count(v => v); // Counts all axis which is marked true
                static bool IsZero(params int[] axis) => axis.All(v => v == 0); // Checks all parsed values if they are equal to zero (thus origin in this context)

                DIRECTIONS = new Vector3Int[(int)Mathf.Pow(3, GetToggledAxis(_x, _y, _z)) - 1]; // Calculates number of directions to search

                int counter = 0;
                int XI = _x ? 1 : 0;
                int YI = _y ? 1 : 0;
                int ZI = _z ? 1 : 0;

                for (int x = -XI; x <= XI; x++)
                    for (int y = -YI; y <= YI; y++)
                        for (int z = -ZI; z <= ZI; z++)
                        {
                            if (IsZero(x, y, z)) { continue; } // Origin
                            DIRECTIONS[counter] = new(x, y, z);
                            counter++;
                        }
            }

            /// <summary>
            /// Creates an axis for only the X/Y axis
            /// </summary>
            public static Axis XY => new(true, true, false);
            /// <summary>
            /// Creates an axis for the X/Y/Z axis
            /// </summary>
            public static Axis XYZ => new(true, true, true);
            /// <summary>
            /// Creates an axis for only the X/Z axis
            /// </summary>
            public static Axis XZ => new(true, true, true);
        }
        
    }
}