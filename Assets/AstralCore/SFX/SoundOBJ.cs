using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.SFX{
    [CreateAssetMenu(fileName = "New Sound Category", menuName = "ScriptableObjects/AstralCore/SFX/New Sound Category")]
    public class SoundOBJ : ScriptableObject{
        [SerializeField, Tooltip("All the various clips associated to this category")] AudioClip[] _clips;

        /// <summary>
        /// All the various clips associated to this category
        /// </summary>
        public AudioClip[] Clips => Clips;

        /// <summary>
        /// Returns a random clip on call
        /// </summary>
        public AudioClip RandomClip => Clips[Random.Range(0, Clips.Length)];
    }
}