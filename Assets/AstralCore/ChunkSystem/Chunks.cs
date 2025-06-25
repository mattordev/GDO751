using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.ChunkSystem{
    public class Chunks {
        public readonly int CHUNK_SIZE;
        readonly Dictionary<Vector3Int, Dictionary<Type, IChunk>> chunks;
        Dictionary<Neighbour, Vector3Int> neighbours;
        public Chunks(int chunksize = 16) {
            CHUNK_SIZE = chunksize;
            chunks = new();

            // Allows for neighbour searching
            neighbours = new();
            foreach (Neighbour n in Enum.GetValues(typeof(Neighbour))){
                int index = (int)n;
                int axis = index % 3;
                int sign = (index / 3 == 0) ? 1 : -1;

                Vector3Int direction = axis switch{
                    0 => new(0, 0, sign),
                    1 => new(sign, 0, 0),
                    2 => new(0, sign, 0),
                    _ => Vector3Int.zero,
                } * CHUNK_SIZE;
                neighbours.Add(n, direction);
            }
        }

        /// <summary>
        /// Attempts to find the chunk, if not will create one
        /// </summary>
        /// <typeparam name="T">The chunk type we are searching for</typeparam>
        /// <param name="position">The position we are comparing</param>
        /// <param name="callback">Behaviour to happen once chunk is found</param>
        /// <returns>The chunk</returns>
        public T Get<T>(Vector3 position, Action<T> callback = null) where T : class, IChunk, new() {
            Vector3Int key = IChunk.GetChunk(position, CHUNK_SIZE);
            var type = typeof(T);
            if (!chunks.TryGetValue(key, out Dictionary<Type, IChunk> chunkTypes))
            { // Returns dictionary if exists, otherwise creates a new one
                chunkTypes = new();
                chunks[key] = chunkTypes;
            }

            T chunk;

            if (!chunkTypes.TryGetValue(type, out IChunk _chunk))
            { // Adds a new instance of chunk to dictionary if it doesnt exist
                chunk = new T();
                chunk._Initialise(key, CHUNK_SIZE);
                chunkTypes[type] = chunk;
            }
            else { chunk = _chunk as T; }

            callback?.Invoke(chunk);
            return chunk;
        }

        /// <summary>
        /// Attempts to find the chunk, if not will create one
        /// </summary>
        /// <typeparam name="T">The chunk type we are searching for</typeparam>
        /// <param name="position">The position we are comparing</param>
        /// <param name="neighbour">The direction we want to go</param>
        /// <param name="callback">Behaviour to happen once chunk is found</param>
        /// <returns>The chunk</returns>
        public T Get<T>(Vector3 position, Neighbour neighbour, Action<T> callback = null) where T : class, IChunk, new() => Get(position + neighbours[neighbour], callback);

        /// <summary>
        /// Attempts to find the chunk, if not will create one
        /// </summary>
        /// <typeparam name="T">The chunk type we are searching for</typeparam>
        /// <param name="chunk">The chunk we are comparing</param>
        /// <param name="neighbour">The direction we want to go</param>
        /// <param name="callback">Behaviour to happen once chunk is found</param>
        /// <returns>The chunk</returns>
        public T Get<T>(T chunk, Neighbour neighbour, Action<T> callback = null) where T : class, IChunk, new() => Get(chunk.Position + neighbours[neighbour], callback);

        /// <summary>
        /// Removes any chunks that have not been called for a while
        /// </summary>
        public void ExpireUnusedChunks(){
            foreach (var kv in chunks){
                List<Type> oldChunks = new();
                foreach (var chunkKV in kv.Value)
                {
                    if (!chunkKV.Value.IsActive) { oldChunks.Add(chunkKV.Key); }
                }
                for (int i = 0; i < oldChunks.Count; i++) { kv.Value.Remove(oldChunks[i]); }
            }
        }

        /// <summary>
        /// The neighbour we are wanting to check
        /// </summary>
        public enum Neighbour {
            FORWARDS,
            RIGHT,
            BACK,
            LEFT,
            UP,
            DOWN
        }
    }
}