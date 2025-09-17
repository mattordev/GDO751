using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.ChunkSystem{
    /// <summary>
    /// Container for what this chunk can hold -- Provides a cheap way to find entities within the chunk
    /// </summary>
    /// <typeparam name="T">The type this chunk contains</typeparam>
    public class ChunkContainer<T> : Chunk{
        readonly HashSet<T> data = new();
        public int Count => data.Count;

        /// <summary>
        /// Iterate through each element contained in this chunk and perform an action
        /// </summary>
        /// <param name="callback">The function we wish to perform on each element</param>
        public void Iterate(Action<T> callback){
            foreach (T entity in data) { callback?.Invoke(entity); }
        }

        /// <summary>
        /// Does this chunk contain this entity?
        /// </summary>
        /// <param name="entity">The entity we are comparing</param>
        /// <returns></returns>
        public bool Contains(T entity) => data.Contains(entity);
        /// <summary>
        /// Registers the parsed entity to this chunk
        /// </summary>
        /// <param name="entity">The entity we wish to register</param>
        /// <returns>True if successful</returns>
        public bool Register(T entity){
            IsActive = true;
            return data.Add(entity);
        }
        /// <summary>
        /// Unregisters the parsed entity from this chunk
        /// </summary>
        /// <param name="entity">The entity we wish to unregister</param>
        /// <returns>True if successful</returns>
        public bool Unregister(T entity){
            bool success = data.Remove(entity);
            IsActive = Count > 0;
            return success;
        }
        /// <summary>
        /// Changes registration from this chunk to another
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public ChunkContainer<T> SwitchTo(T entity, ChunkContainer<T> from){
            from?.Unregister(entity);
            Register(entity);
            return this;
        }

        public override string ToString() => $"Chunk ({typeof(T)}): {Position}";

        public static ChunkContainer<T> SwitchTo(T entity, ChunkContainer<T> from, ChunkContainer<T> to) => to.SwitchTo(entity, from);
    }

    /// <summary>
    /// Allows us to track this object's position relative to the chunk its within
    /// </summary>
    /// <typeparam name="T">Class inheriting MonoBehaviour</typeparam>
    public interface IChunkTrackable<T> where T : class {
        T TrackableComponent { get; }
        ChunkContainer<T> Chunk { get; set; }
        void UpdateChunk(Chunks chunks, Vector3 position) {
            ChunkContainer<T> newChunk = chunks.Get<ChunkContainer<T>>(position);
            Chunk = newChunk?.SwitchTo(TrackableComponent, Chunk);
        }
        
        /// <summary>
        /// Called upon deletion, should unregister from chunk!
        /// </summary>
        void OnDestroy();
    }
}