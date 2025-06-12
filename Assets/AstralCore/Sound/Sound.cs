using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.SFX{
    /// <summary>
    /// Allows for sound to be played in a consistant/easily accessible location
    /// </summary>
    [Serializable] public class Sound{
        [SerializeField, Tooltip("The source used to play global sfx. E.g. UI")] AudioSource sfxSource;
        [SerializeField, Tooltip("List of all sounds")] SoundOBJ[] _library;

        Dictionary<string, SoundOBJ> _l;
        Dictionary<string, SoundOBJ> SoundLibrary{
            get{
                if (_l == null){
                    _l = new();
                    foreach (SoundOBJ o in _library){
                        _l.Add(o.name, o);
                    }
                }
                return _l;
            }
        }
        /// ---


        /// <summary>
        /// Pulls a random clip from a category
        /// </summary>
        /// <param name="category">The sound category we want to retrieve a sound from</param>
        /// <returns>A random clip from the category</returns>
        public AudioClip GetClip(string category) => GetCategoryObject(category)?.RandomClip;
        /// <summary>
        /// Pulls the category object
        /// </summary>
        /// <param name="category">The sound category we want to retrieve</param>
        /// <returns>The category object</returns>
        public SoundOBJ GetCategoryObject(string category){
            SoundLibrary.TryGetValue(category, out SoundOBJ v);
            return v;
        }

        /// ---

        /// <summary>
        /// Plays audio from the parsed source
        /// </summary>
        /// <param name="src">The audio source we want to play from</param>
        /// <param name="category">The category of sound we want to pull</param>
        /// <param name="volume">The volume of the audio</param>
        public void Play(ref AudioSource src, string category, float volume = 1) => Play(ref src, GetClip(category), volume);
        /// <summary>
        /// Plays the audio in 2D space (Global - Useful for UI)
        /// </summary>
        /// <param name="category">The category of sound we want to pull</param>
        /// <param name="volume">The volume of the audio</param>
        public void Play(string category, float volume = 1) => Play(GetClip(category), volume);
        /// <summary>
        /// Plays the audio in 3D space
        /// </summary>
        /// <param name="category">The category of sound we want to pull</param>
        /// <param name="position">The position in 3D space we want to play the audio</param>
        /// <param name="volume">The volume of the audio</param>
        public void Play(string category, Vector3 position, float volume = 1) => Play(GetClip(category), position, volume);

        /// <summary>
        /// Plays audio from the parsed source
        /// </summary>
        /// <param name="src">The audio source we want to play from</param>
        /// <param name="clip">The audio clip we want to play</param>
        /// <param name="volume">The volume of the audio</param>
        public void Play(ref AudioSource src, AudioClip clip, float volume = 1) => src.PlayOneShot(clip, volume);
        /// <summary>
        /// Plays the audio in 2D space (Global - Useful for UI)
        /// </summary>
        /// <param name="clip">The audio clip we want to play</param>
        /// <param name="volume">The volume of the audio</param>
        public void Play(AudioClip clip, float volume = 1) => sfxSource.PlayOneShot(clip, volume);
        /// <summary>
        /// Plays the audio in 3D space
        /// </summary>
        /// <param name="clip">The audio clip we want to play</param>
        /// <param name="position">The position in 3D space we want to play the audio</param>
        /// <param name="volume">The volume of the audio</param>
        public void Play(AudioClip clip, Vector3 position, float volume = 1) => AudioSource.PlayClipAtPoint(clip, position, volume);
    }
}