using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.ChunkSystem{
    public class ChunkManager : MonoBehaviour{
        [SerializeField] bool showDebug;
        [SerializeField] int chunksize = 16;
        Chunks _chunks;
        public Chunks Chunks => _chunks;
        void Awake(){
            _chunks = new Chunks(chunksize);
            InvokeRepeating(nameof(ExpireChunks), 1,1);
        }

        void ExpireChunks() => _chunks.ExpireUnusedChunks();

        void OnDrawGizmosSelected(){
            if(!showDebug || _chunks == null){ return; }
            Gizmos.DrawWireCube(IChunk.GetChunk(transform.position, chunksize) + Vector3.one * chunksize/2, Vector3.one * chunksize);
        }
    }
}