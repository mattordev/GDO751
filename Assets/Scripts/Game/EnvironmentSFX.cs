using System.Collections;
using System.Collections.Generic;
using AstralCandle.Utilities;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Game{
    [RequireComponent(typeof(AudioSource))]
    public sealed class EnvironmentSFX : AudioManager, ISingleton<EnvironmentSFX>{
        [SerializeField] MinMax<float> randomPlayTime = new(60, 240);
        [SerializeField] AudioClip defaultAmbience;
        [SerializeField] AudioClip[] defaultTracks;
        [SerializeField] float defaultTransition = 1;
        #region PRIVATE VARS
        AudioSource[] _src; // Dont use
        AudioSource[] srcs => _src ??= GetComponents<AudioSource>(); // All the sources attached to this object (Should be 2)
        AudioClip[] tracks; // All the tracks we want to load as a potential to be played
        int srcIndex;
        float nextTrackToPlay = 0;
        float transitionTime;
        float fadeTimer;
        #endregion


        void Awake(){
            // Create singleton
            if(!(this as ISingleton<EnvironmentSFX>).CreateSingleton(this)){
                Destroy(gameObject);
                return;
            }
            GenerateTime(null);

            // Initiate all sources
            foreach(AudioSource s in srcs){
                s.volume = 0;
                s.loop = true;
                s.Play();
            }
            LoadAmbience(defaultAmbience, defaultTransition, defaultTracks);
        }
        
        protected override void Update(){
            base.Update();

            if(Time.time >= fadeTimer){ LoadAmbience(srcs[srcIndex].clip, transitionTime, tracks); }

            if(Time.time >= nextTrackToPlay){ PlayTrack(); }
        }
        /// <summary>
        /// Generates the next time to play a track
        /// </summary>
        /// <param name="clip">The clip we are playing</param>
        void GenerateTime(AudioClip clip) => nextTrackToPlay = Time.time + (clip?.length ?? 0) + UnityEngine.Random.Range(randomPlayTime.min, randomPlayTime.max);  

        /// <summary>
        /// Loads a random clip to play and sets a new timer
        /// </summary>
        void PlayTrack(){
            AudioClip clip = tracks[UnityEngine.Random.Range(0, tracks.Length)];
            GenerateTime(clip);
            srcs[srcIndex].PlayOneShot(clip);
        }

        /// <summary>
        /// Loads ambience music to be played with space for tracks to be overlayed ontop
        /// </summary>
        /// <param name="clip">The ambience music</param>
        /// <param name="timeTillFade">The time till we will fade in a copy of this track</param>
        /// <param name="tracks">The tracks that can be overlayed ontop of the ambience</param>
        public void LoadAmbience(AudioClip clip, float transitionTime = 1, params AudioClip[] tracks){
            this.tracks = tracks;
            
            if(clip != srcs[srcIndex].clip){ // If not the same ambience...
                GenerateTime(null); 
            } 

            int prvIndex = srcIndex;
            UFunc.ModulusCounter(ref srcIndex, srcs.Length);

            this.transitionTime = transitionTime; 
            srcs[srcIndex].clip = clip;
            srcs[srcIndex].Play();

            fadeTimer = Time.time + clip.length - transitionTime;

            CrossFade(srcs[srcIndex], srcs[prvIndex], transitionTime);
        }
    }
}