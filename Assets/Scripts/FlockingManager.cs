using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AstralCandle.Character;
using AstralCore.AI;
using AstralCore.ChunkSystem;
using AstralCore.Utils;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle{
    public class FlockingManager : Singleton<FlockingManager>{
        [SerializeField] BirdAI[] birdPrefabs;
        [SerializeField] int cellSize;

        [SerializeField] bool showDebug;
        [SerializeField] MinMax<int> qtyOfFlocks;
        [SerializeField] MinMax<int> flockSize;
        [SerializeField] Vector3Int flockBoundaries;
        [SerializeField] float distanceDelta = 1;
        [SerializeField] MinMax<float> birdReplenTime;

        Flock[] flocks;
        [SerializeField] ChunkManager chunkManager;

        Dictionary<string,(int, float)> flockReplen = new(); // Replenishes a flock as they die

        void OnDrawGizmos()
        {
            if (!showDebug) { return; }
            Gizmos.DrawWireCube(transform.position, flockBoundaries);
        }


        protected override void Awake()
        {
            base.Awake();
            Vector3 bounds = flockBoundaries / 2;

            int numFlocks = UnityEngine.Random.Range(qtyOfFlocks.min, qtyOfFlocks.max);
            flocks = new Flock[numFlocks];
            for (int i = 0; i < numFlocks; i++)
            {
                Vector3 flockPos = new(
                    UnityEngine.Random.Range(transform.position.x - bounds.x, transform.position.x + bounds.x),
                    UnityEngine.Random.Range(transform.position.y - bounds.y, transform.position.y + bounds.y),
                    UnityEngine.Random.Range(transform.position.z - bounds.z, transform.position.z + bounds.z)
                );

                int size = UnityEngine.Random.Range(flockSize.min, flockSize.max);
                BirdAI[] motors = new BirdAI[size];
                for (int x = 0; x < size; x++)
                {
                    Vector3 lclOfst = new(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));
                    motors[x] = Instantiate(birdPrefabs[UnityEngine.Random.Range(0, birdPrefabs.Length)], flockPos + lclOfst, Quaternion.identity, transform);
                }
                ChunkPathfinder pathfinder = new(chunkManager.Chunks, cellSize, Pathfinder.Axis.XYZ);
                flocks[i] = new(motors, pathfinder, transform.position, bounds, distanceDelta, i, Replen);
            }
        }

        void Replen(int flockId)
        {
            if (!flockReplen.TryAdd(flockId.ToString(), (1, Time.time + UnityEngine.Random.Range(birdReplenTime.min, birdReplenTime.max)))){
                (int, float) v = flockReplen[flockId.ToString()];
                v.Item1 += 1;
                flockReplen[flockId.ToString()] = v;
            }
        }

        void Update(){
            for (int i = 0; i < flocks.Length; i++)
            {
                flocks[i].Run();
                if (flockReplen.ContainsKey(i.ToString()) && Time.time >= flockReplen[i.ToString()].Item2){
                    Debug.Log($"Replenishing {flockReplen[i.ToString()].Item1} bird(s)");
                    for (int x = 0; x < flockReplen[i.ToString()].Item1; x++){
                        BirdAI m = Instantiate(birdPrefabs[UnityEngine.Random.Range(0, birdPrefabs.Length)], flocks[i].AveragePosition, Quaternion.identity, transform);
                        flocks[i].motors.Add(m);
                        m.agents = flocks[i].motors.ToArray();
                        m.replen = Replen;
                        m.flockId = i;
                    }
                    flockReplen.Remove(i.ToString());
                }
                flocks[i].flockDead = false;
            }        
        }

        public class Flock
        {
            public readonly List<BirdAI> motors;
            Vector3 _destination;
            public Vector3 Destination{ // Applies new position on all agents
                get => _destination;
                private set{
                    _destination = new(
                        Mathf.Clamp(value.x, boundsOrigin.x - bounds.x, boundsOrigin.x + bounds.x),
                        Mathf.Clamp(value.y, boundsOrigin.y - bounds.y, boundsOrigin.y + bounds.y),
                        Mathf.Clamp(value.z, boundsOrigin.z - bounds.z, boundsOrigin.z + bounds.z)
                    );
                    for (int i = motors.Count - 1; i >= 0; i--){
                        if(motors[i].DestroyBird == true){ motors.RemoveAt(i); }
                        motors[i].targetPosition = _destination;
                    }
                }
            }
            public Vector3 AveragePosition
            {
                get
                {
                    Vector3 avg = default;
                    for (int i = motors.Count - 1; i >= 0; i--){
                        if (motors[i].DestroyBird == true)
                        {
                            motors.RemoveAt(i);
                            continue;
                        }
                        avg += motors[i].transform.position;
                        motors[i].agents = motors.ToArray();
                    }
                    return avg /= motors.Count;
                }
            }

            Vector3 NewPos(Vector3 origin)
            {
                return new(
                    UnityEngine.Random.Range(origin.x - bounds.x, origin.x + bounds.x),
                    UnityEngine.Random.Range(origin.y - bounds.y, origin.y + bounds.y),
                    UnityEngine.Random.Range(origin.z - bounds.z, origin.z + bounds.z)
                );
            }

            readonly float distanceDelta; // How close the average position has to be until we get a new position
            Vector3 boundsOrigin, bounds;
            readonly ChunkPathfinder pathfinder;

            Queue<GraphNode> path;

            public bool flockDead = false;

            public Flock(BirdAI[] motors, ChunkPathfinder pathfinder, Vector3 boundsOrigin, Vector3 bounds, float distanceDelta, int flockId, Action<int> replen)
            {
                this.motors = motors.ToList();
                this.pathfinder = pathfinder;
                this.boundsOrigin = boundsOrigin;
                this.bounds = bounds;
                this.distanceDelta = distanceDelta;
                
                Destination = NewPos(boundsOrigin);
                for (int i = 0; i < motors.Length; i++)
                {
                    motors[i].agents = motors.ToArray();
                    motors[i].replen = replen;
                    motors[i].flockId = flockId;
                }
                path = new();
            }
            

            public void Run()
            {
                if (flockDead) { return; }
                else if (!flockDead && motors.Count <= 0){
                    flockDead = true;
                    path.Clear();
                    return;
                }

                if (path.Count <= 0)
                {
                    List<GraphNode> nodes = pathfinder.Search(Destination, NewPos(boundsOrigin));
                    if (nodes == default) { return; }
                    for (int i = 0; i < nodes.Count; i++) { path.Enqueue(nodes[i]); }
                    if (path.Count <= 0) { return; }
                    Destination = path.Dequeue().position;
                }
                else if (Vector3.Distance(Destination, AveragePosition) <= distanceDelta){
                    Destination = path.Dequeue().position;
                }
            }
        }
    }
}