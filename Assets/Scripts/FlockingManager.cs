using System.Collections;
using System.Collections.Generic;
using AstralCandle.Character;
using AstralCore.AI;
using AstralCore.ChunkSystem;
using AstralCore.Utils;
using NUnit.Framework.Constraints;
using UnityEditor.Rendering;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle{
    public class FlockingManager : MonoBehaviour{
        [SerializeField] BirdAI[] birdPrefabs;
        [SerializeField] int cellSize;

        [SerializeField] bool showDebug;
        [SerializeField] MinMax<int> qtyOfFlocks;
        [SerializeField] MinMax<int> flockSize;
        [SerializeField] Vector3Int flockBoundaries;
        [SerializeField] float distanceDelta = 1;

        Flock[] flocks;
        [SerializeField] ChunkManager chunkManager;



        void OnDrawGizmos()
        {
            if (!showDebug) { return; }
            Gizmos.DrawWireCube(transform.position, flockBoundaries);
        }


        void Awake()
        {
            Vector3 bounds = flockBoundaries / 2;

            int numFlocks = Random.Range(qtyOfFlocks.min, qtyOfFlocks.max);
            flocks = new Flock[numFlocks];
            for (int i = 0; i < numFlocks; i++)
            {
                Vector3 flockPos = new(
                    Random.Range(transform.position.x - bounds.x, transform.position.x + bounds.x),
                    Random.Range(transform.position.y - bounds.y, transform.position.y + bounds.y),
                    Random.Range(transform.position.z - bounds.z, transform.position.z + bounds.z)
                );

                int size = Random.Range(flockSize.min, flockSize.max);
                BirdAI[] motors = new BirdAI[size];
                for (int x = 0; x < size; x++)
                {
                    Vector3 lclOfst = new(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
                    motors[x] = Instantiate(birdPrefabs[Random.Range(0, birdPrefabs.Length)], flockPos + lclOfst, Quaternion.identity, transform);
                }
                ChunkPathfinder pathfinder = new(chunkManager.Chunks, cellSize, Pathfinder.Axis.XYZ);
                flocks[i] = new(motors, pathfinder, transform.position, bounds, distanceDelta);
            }
        }

        void Update(){
            for (int i = 0; i < flocks.Length; i++){ flocks[i].Run(); }        
        }

        public class Flock
        {
            readonly BirdAI[] motors;
            Vector3 _destination;
            Vector3 Destination{ // Applies new position on all agents
                get => _destination;
                set{
                    _destination = new(
                        Mathf.Clamp(value.x, boundsOrigin.x - bounds.x, boundsOrigin.x + bounds.x),
                        Mathf.Clamp(value.y, boundsOrigin.y - bounds.y, boundsOrigin.y + bounds.y),
                        Mathf.Clamp(value.z, boundsOrigin.z - bounds.z, boundsOrigin.z + bounds.z)
                    );
                    foreach (BirdAI m in motors) { m.targetPosition = _destination; }
                }
            }
            Vector3 AveragePosition
            {
                get
                {
                    Vector3 avg = default;
                    for (int i = 0; i < motors.Length; i++)
                    {
                        avg += motors[i].transform.position;
                    }
                    return avg /= motors.Length;
                }
            }

            readonly float distanceDelta; // How close the average position has to be until we get a new position
            Vector3 boundsOrigin, bounds;
            readonly ChunkPathfinder pathfinder;

            Queue<GraphNode> path;

            public Flock(BirdAI[] motors, ChunkPathfinder pathfinder, Vector3 boundsOrigin, Vector3 bounds, float distanceDelta = 0.1f)
            {
                this.motors = motors;
                this.pathfinder = pathfinder;
                Destination = NewPos;
                this.boundsOrigin = boundsOrigin;
                this.bounds = bounds;
                this.distanceDelta = distanceDelta;

                for (int i = 0; i < motors.Length; i++){
                    motors[i].agents = motors;
                }
                path = new();
            }
            
            Vector3 NewPos => new(
                Random.Range(AveragePosition.x - bounds.x, AveragePosition.x + bounds.x),
                Random.Range(AveragePosition.y - bounds.y, AveragePosition.y + bounds.y),
                Random.Range(AveragePosition.z - bounds.z, AveragePosition.z + bounds.z)
            );

            public void Run()
            {
                if (path.Count <= 0){
                    List<GraphNode> nodes = pathfinder.Search(Destination, NewPos);
                    if(nodes == default){ return; }
                    for (int i = 0; i < nodes.Count; i++) { path.Enqueue(nodes[i]); }
                    if(path.Count <= 0){ return; }
                    Destination = path.Dequeue().position;
                }
                else if (Vector3.Distance(Destination, AveragePosition) <= distanceDelta){
                    Destination = path.Dequeue().position;
                }
            }
        }
    }
}