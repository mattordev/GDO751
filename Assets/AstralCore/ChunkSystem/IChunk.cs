using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.ChunkSystem{
    public interface IChunk : IEquatable<IChunk>
    {
        /// <summary>
        /// The name of this chunk
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// The position this chunk is found at
        /// </summary>
        public Vector3Int Position { get; }
        /// <summary>
        /// The total size of this chunk
        /// </summary>
        public int Scale { get; }

        /// <summary>
        /// Is this chunk currently being used?
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Extra bits of data associated to this chunk
        /// </summary>
        public HashSet<string> MetaData { get; }

        /// <summary>
        /// Checks to see if position is within the chunk
        /// </summary>
        /// <param name="position">The position we are comparing</param>
        /// <returns>True if in chunk</returns>
        public bool InChunk(Vector3 position) => GetChunk(position, Scale).Equals(Position);

        /// <summary>
        /// Initialises class with information required - Dont use!
        /// </summary>
        /// <param name="position">The position this chunk is found at</param>
        /// <param name="scale">The total size of this chunk</param>
        /// <param name="name">The name of this chunk</param>
        /// <param name="metaData">Extra bits of data associated to this chunk</param>
        public void _Initialise(Vector3Int position, int scale, string name = null, params string[] metaData);

        /// <summary>
        /// Calculates the position of a chunk given the parsed position and scale
        /// </summary>
        /// <param name="position">Position we are comparing</param>
        /// <param name="chunkSize">The size of the chunk</param>
        /// <returns>The position this chunk is found at</returns>
        public static Vector3Int GetChunk(Vector3 position, int chunkSize) => Vector3Int.FloorToInt(position / chunkSize) * chunkSize;

        // -- META DATA

        /// <summary>
        /// Queries chunk metadata to find if the parsed data is found inside
        /// </summary>
        /// <param name="callback">What to do if a single string is found</param>
        /// <param name="data">The data we are comparing</param>
        /// <returns>True, if all parsed strings are found</returns>
        public bool HasMetadata(Action<string> callback, params string[] data)
        {
            bool found = true;
            foreach (string d in data)
            {
                if (!MetaData.TryGetValue(d, out _))
                {
                    found = false;
                    continue;
                }
                callback?.Invoke(d);
            }
            return found;
        }

        /// <summary>
        /// Queries chunk metadata to find if any of the parsed data is found inside
        /// </summary>
        /// <param name="found">Outputs the found string</param>
        /// <param name="data">The data we are comparing</param>
        /// <returns>True, on the first string that matches if any</returns>
        public bool HasAnyMetadata(out string found, params string[] data)
        {
            foreach (string d in data)
            {
                if (MetaData.TryGetValue(d, out _))
                {
                    found = d;
                    return true;
                }
            }
            found = null;
            return false;
        }

        /// <summary>
        /// Queries chunk metadata to find if any of the parsed data is found inside
        /// </summary>
        /// <param name="callback">What to do if a string is found</param>
        /// <param name="data">The data we are comparing</param>
        /// <returns>True, on the first string that matches if any</returns>
        public bool HasAnyMetadata(Action<string> callback, params string[] data)
        {
            if (HasAnyMetadata(out string found, data))
            {
                callback?.Invoke(found);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to add all parsed data to this chunk
        /// </summary>
        /// <param name="data">The data we wish to associate with this chunk</param>
        public void AddMetadata(params string[] data){
            foreach (string d in data) { MetaData.Add(d); }
        }
        
        /// <summary>
        /// Attempts to remove all parsed data from the chunk
        /// </summary>
        /// <param name="data">The data we wish to remove from the chunk</param>
        public void RemoveMetadata(params string[] data){
            foreach (string d in data) { MetaData.Remove(d); }
        }
    }
}