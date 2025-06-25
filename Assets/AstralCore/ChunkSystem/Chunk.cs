using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.ChunkSystem{
    public class Chunk : IChunk{
        string _name;
        public string Name => _name;
        Vector3Int _position;
        public Vector3Int Position => _position;
        public Vector3Int Center => _position + Vector3Int.one * Scale/2;
        int _scale;
        public int Scale => _scale;
        HashSet<string> _metaData;
        public HashSet<string> MetaData => _metaData;
        public bool IsActive { get; set; }

        public Chunk(){}
        public Chunk(Vector3Int position, int scale, string name = null, params string[] metaData) => _Initialise(IChunk.GetChunk(position, scale), scale, name, metaData);

        public void _Initialise(Vector3Int position, int scale, string name = null, params string[] metaData){
            _position = position;
            _scale = scale;
            _name = name;
            IsActive = false;
            _metaData = metaData.ToHashSet();
            Initialise();
        }
        
        /// <summary>
        /// Initialises this collider so it is ready
        /// </summary>
        protected virtual void Initialise() {}


        public virtual bool Equals(IChunk other) => Position.Equals(other.Position);
        public override int GetHashCode() => Position.GetHashCode();
        public override string ToString() => $"{Name} ({typeof(Chunk)}): {Position}";

        public static implicit operator Chunk((Vector3 position, int scale) _) => new(Vector3Int.FloorToInt(_.position), _.scale);
        public static implicit operator (Vector3 position, int scale)(Chunk _) => (_.Position, _.Scale);
    }
}