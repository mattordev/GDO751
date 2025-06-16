using System;
using System.Collections;
using System.Collections.Generic;
using AstralCandle;
using AstralCore.Utils;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.SFX{
    /// <summary>
    /// Allows for music to be played persistantly across scenes
    /// </summary>
    [Serializable] public class Music{
        [Header("Main Music Settings")]
        [SerializeField, Tooltip("The object housing the sources (Should only be 2)")] Transform sourceOBJ;
        [SerializeField, Tooltip("The min/max volume for music")] MinMax<float> volume;
        [SerializeField, Tooltip("Fade between songs")] AnimEaser crossfade;
        [SerializeField, Tooltip("Fades all music")] AnimEaser fade;

        [Header("Overlay Settings")]
        [SerializeField, Tooltip("The source used to play special sounds")] AudioSource overlaySource;
        [SerializeField, Tooltip("The min/max volume for special sounds")] MinMax<float> overlayVolume;
        [SerializeField, Tooltip("Fades in/out special sounds")] AnimEaser overlayFade;
        /// ---

        AudioSource[] _srcs = null;
        AudioSource[] Sources => _srcs ??= sourceOBJ.GetComponents<AudioSource>();

        int srcIndx = 0;
        AudioSource CurrentSource => Sources[srcIndx];

        // Overlay
        MusicObj currentlyPlaying;
        float overlayCooldown, overlayFadeTime;
        
        /// <summary>
        /// Processes this class frame by frame
        /// </summary>
        public void Process(){
            Fade();
            if (!CurrentSource.clip) { return; } // If nothing is playing
            else if (CurrentSource.time >= CurrentSource.clip.length - crossfade.Duration) { Play(currentlyPlaying, true); }

            // Overlay
            if (!currentlyPlaying.overlay) { return; } // If no overlay
            else if (Time.time >= overlayCooldown) {
                overlayCooldown = currentlyPlaying.NextOverlay;
                overlayFadeTime = currentlyPlaying.CalculateFadeTime(overlayFade.Duration);
                overlayFade.SetReverse(false);
                overlayFade.Trim(0);
                overlaySource.Play();
            }
            if (Time.time >= overlayFadeTime) { overlayFade.SetReverse(true); } // Fade the overlay out
        }


        /// <summary>
        /// Returns a volume between the min/max values based on 't' input
        /// </summary>
        /// <param name="t">The time value between 0-1</param>
        /// <returns>Volume value to be plugged into sources</returns>
        float CalculateVolume(float t) => Mathf.LerpUnclamped(volume.min, volume.max, t);

        /// <summary>
        /// Fades between sources
        /// </summary>
        void Fade(){
            float cf = crossfade.Play();
            float f = fade.Play();
            for (int i = 0; i < Sources.Length; i++){
                if (Sources[i] == CurrentSource){
                    Sources[i].volume = CalculateVolume(cf * f);
                    continue;
                }
                Sources[i].volume = CalculateVolume((1 - cf) * f);
            }

            if (!currentlyPlaying.overlay) { return; } // If no overlay
            float of = overlayFade.Play();
            overlaySource.volume = Mathf.LerpUnclamped(overlayVolume.min, overlayVolume.max, of * f);
        }


        /// ---
        
        /// <summary>
        /// Crossfades music
        /// </summary>
        /// <param name="music">The music we want to play</param>
        /// <param name="force">Force override: Allowing for cross-fading to same audio</param>
        public void Play(MusicObj music, bool force = false){
            if (music.Equals(default(MusicObj))) { return; } // Dont play a null clip
            else if (!force && music.main.Equals(CurrentSource.clip)) { return; } // Unless force is true, dont override audio with same audio. (Stops spamming)

            Methods.ModulusCounter(ref srcIndx, Sources.Length); // Round-Robin counter

            CurrentSource.clip = music.main;
            CurrentSource.Play();

            crossfade.Trim(0);

            currentlyPlaying = music;
            if (!force){
                overlayFade.SetReverse(true);
                overlayCooldown = music.OverlayCooldown;
                overlayFadeTime = music.CalculateFadeTime(overlayFade.Duration);
                overlaySource.clip = music.overlay;
            }
        }

        /// <summary>
        /// Pauses the music by fading it out
        /// </summary>
        public void Pause() => fade.SetReverse(false);
        /// <summary>
        /// Plays the music by fading it in
        /// </summary>
        public void Resume() => fade.SetReverse(true);


        /// <summary>
        /// Allows us to play music with an extra feature
        /// </summary>
        public readonly struct MusicObj
        {
            /// <summary>
            /// The main music to be played
            /// </summary>
            public readonly AudioClip main;
            /// <summary>
            /// The music to be played ontop of the main music
            /// </summary>
            public readonly AudioClip overlay;
            /// <summary>
            /// The intervals to play the overlay
            /// </summary>
            readonly MinMax<float> overlayInterval;

            /// <summary>
            /// Constructor for music obj
            /// </summary>
            /// <param name="main">Main audio to play</param>
            /// <param name="overlay">Audio to overlay main as an extra</param>
            /// <param name="overlayInterval">The intervals between each time we play the overlay</param>
            public MusicObj(AudioClip main, AudioClip overlay = null, MinMax<float> overlayInterval = default)
            {
                this.main = main;
                this.overlay = overlay;
                this.overlayInterval = overlayInterval;
            }

            /// <summary>
            /// Generates a random cooldown on call
            /// </summary>
            public readonly float OverlayCooldown => Time.time + UnityEngine.Random.Range(overlayInterval.min, overlayInterval.max);
            /// <summary>
            /// Generates a random cooldown on call (Includes overlay duration)
            /// </summary>
            public readonly float NextOverlay => OverlayCooldown + overlay.length;
            /// <summary>
            /// Calculates the time we should fade the clip
            /// </summary>
            /// <param name="duration">The duration of the easing algorithm</param>
            /// <returns>The time we should initiate the fade</returns>
            public readonly float CalculateFadeTime(float duration) => Time.time + (overlay.length - duration);
        }
    }
}