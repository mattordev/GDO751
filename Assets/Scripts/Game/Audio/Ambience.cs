using System.Collections;
using System.Collections.Generic;
using AstralCandle.Game;
using AstralCandle.Utilities;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle{
    public class Ambience : AudioSystem, ISingleton<Ambience>{
        [SerializeField] float transitionTime = 1;
        [SerializeField] MinMax<float> trackPlaybacktimes = new(60, 240);
        [Header("Defaults")]
        [SerializeField] AudioClip defaultAmbience;
        [SerializeField] AudioClip[] defaultTracks;

        int srcIndex;
        float fadeTimer; // The time till we cross fade to another source (Additive ontop of clip's length)
        AudioClip[] tracks; // Set of tracks that can be played
        AudioSource track; // Current track being played
        AudioSource toDestroy; // Track to be destroyed
        

        protected override void Init(){
            if(!(this as ISingleton<Ambience>).CreateSingleton(this)){
                Destroy(gameObject);
                return;
            }
            
            // Load default ambience here
            Load(defaultAmbience, defaultTracks);
        }

        protected override void Process(){
            if(Time.time >= fadeTimer){ Load(sources[srcIndex].clip, tracks); } // Cross fade to another source before it finishes - Stops the clipping at the end

            // Manage Track playback -- Call upon cross fade to play track at x time (From research, we'll have to use actual audiosource instead of playoneshot)- Perhaps instance new audiosource objects
        }


        // --- FUNCTIONS
        public void Load(AudioClip clip, params AudioClip[] tracks){
            int prvIndx = srcIndex;
            this.tracks = tracks;
            if(sources[srcIndex].clip != clip){ 
                StopCoroutine(PlayTrack()); 
                if(track){ Fade(track, transitionTime, false, () => { Destroy(track.gameObject); return true; }); }
                StartCoroutine(PlayTrack());
            }
            

            UFunc.ModulusCounter(ref srcIndex, sources.Length);

            sources[srcIndex].clip = clip;
            sources[srcIndex].Play();

            fadeTimer = Time.time + clip.length - transitionTime;
            CrossFade(sources[prvIndx], sources[srcIndex], transitionTime);            
        }
    
    
        IEnumerator PlayTrack(){
            float delay = (track?.clip?.length ?? 0) + Random.Range(trackPlaybacktimes.min, trackPlaybacktimes.max);
            yield return new WaitForSeconds(delay);

            AudioSource newSrc = new GameObject("Track").AddComponent<AudioSource>();
            newSrc.clip = tracks[Random.Range(0, tracks.Length)];
            newSrc.volume = 0;
            newSrc.transform.parent = transform;
            newSrc.Play();

            toDestroy = track;
            CrossFade(track, newSrc, transitionTime, () => { Destroy(toDestroy?.gameObject); return true; });
            track = newSrc;
            StartCoroutine(PlayTrack());
        }
    }
}