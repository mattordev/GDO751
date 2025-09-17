using System;
using System.Collections.Generic;
using AstralCore.Utils;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.AI{
    /// <summary>
    /// The heart of the A* pathfinding algorithm
    /// </summary>
    /// <typeparam name="T">must inherit from GraphNode</typeparam>
    public abstract class Pathfinder_OOP<T> where T : GraphNode{
        int maxIterations;
        BinaryMinHeap<T> openlist;
        HashSet<T> closedlist;

        /// <summary>
        /// Constructor for pathfinder
        /// </summary>
        /// <param name="cellSize">The cellsize to query the environment</param>
        /// <param name="maxIterations">The max iterations our solution will use until failure to find path</param>
        /// <param name="heapCapacity">The max nodes that can exist in our min heap</param>
        public Pathfinder_OOP(int maxIterations = 1000, int? heapCapacity = null){
            this.maxIterations = maxIterations;
            openlist = new(heapCapacity ?? maxIterations);
            closedlist = new(heapCapacity ?? maxIterations);
        }

        /// <summary>
        /// Queries the world to find a path between the start and end position
        /// </summary>
        /// <param name="start">Our start position</param>
        /// <param name="end">The target position</param>
        /// <returns>List of nodes</returns>
        public virtual List<T> Search(T start, T end){
            // Clear incase of failure
            openlist.Clear();
            closedlist.Clear();

            openlist.Enqueue(start);


            for (int i = 0; i < maxIterations && openlist.Count > 0; i++){
                T current = openlist.Dequeue();
                if (current.Equals(end)) { return TracePath(current); }
                closedlist.Add(current);

                SearchNeighbours(current, end, ref openlist, ref closedlist);
            }
            return null;
        }

        /// <summary>
        /// Traces nodes back to create a path
        /// </summary>
        /// <param name="node">The currently selected node</param>
        /// <returns>A queue of nodes forming a path</returns>
        List<T> TracePath(T node){
            List<T> path = new();
            while (node.parent != null){
                path.Add(node);
                node = (T)node.parent;
            }
            // Clear once complete
            openlist.Clear();
            closedlist.Clear();
            path.Reverse();
            return path;
        }

        /// <summary>
        /// Dictates how we search for neighbours
        /// </summary>
        /// <param name="current">The current node</param>
        /// <param name="current">The target node</param>
        /// <param name="openlist">The nodes we are yet to search</param>
        /// <param name="closedlist">The nodes we have already visited</param>
        protected abstract void SearchNeighbours(T current, T end, ref BinaryMinHeap<T> openlist, ref HashSet<T> closedlist);
        /// <summary>
        /// Calculate the distance between 2 nodes
        /// </summary>
        /// <param name="from">Node A</param>
        /// <param name="to">Node B</param>
        /// <returns></returns>
        protected abstract int CalculateDistance(T from, T to);
    }

    /// <summary>
    /// Base node for basic A* traversal
    /// </summary>
    public class GraphNode : IComparable<GraphNode>, IEquatable<GraphNode>{
        /// <summary>
        /// The position of this node
        /// </summary>
        public Vector3Int position;
        /// <summary>
        /// This node's parent
        /// </summary>
        public readonly GraphNode parent;
        /// <summary>
        /// costs of this node
        /// </summary>
        public readonly int gCost, hCost;
        /// <summary>
        /// Total cost of this node
        /// </summary>
        public int FCost => gCost + hCost;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position">The position of this node</param>
        /// <param name="gCost">Cost from start</param>
        /// <param name="hCost">Cost from end</param>
        /// <param name="parent">This node's parent</param>
        public GraphNode(Vector3Int position, int gCost = 0, int hCost = 0, GraphNode parent = null){
            this.position = position;
            this.parent = parent;
            this.gCost = gCost;
            this.hCost = hCost;
        }
        public static implicit operator GraphNode(Vector3 pos) => new(Vector3Int.FloorToInt(pos));
        public static implicit operator GraphNode(Vector3Int pos) => new(pos);
        public static implicit operator Vector3Int(GraphNode node) => node.position;
        public static implicit operator Vector2Int(GraphNode node) => (Vector2Int)node.position;


        #region INTERFACES
        public int CompareTo(GraphNode other){
            int compare = FCost.CompareTo(other.FCost);
            return compare != 0 ? compare : hCost.CompareTo(other.hCost);
        }
        public bool Equals(GraphNode other) => position.Equals(other.position);
        public override int GetHashCode() => position.GetHashCode();
        #endregion
    }
}