using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Game{
    public abstract class AudioSystem : MonoBehaviour{
        protected const int AUDIO_SOURCES = 2;
        /// <summary>
        /// The sources we have created
        /// </summary>
        protected AudioSource[] sources;
        List<(FadeProfile, float, Func<bool>)> fades = new();
        void Awake(){
            // Generate sources
            sources = new AudioSource[AUDIO_SOURCES];
            for(int i = 0; i < AUDIO_SOURCES; i++){
                sources[i] = gameObject.AddComponent<AudioSource>();
                sources[i].volume = 0;
                sources[i].loop = true;

                sources[i].Play();
            }

            Init();
        }

        void Update(){
            for(int i = fades.Count-1; i > 0; i--){
                (FadeProfile profile, float elapsed, Func<bool> callback) = fades[i];
                elapsed += Time.deltaTime;

                float t = elapsed / profile.Duration;
                profile.Source.volume = Mathf.Lerp(profile.From, profile.Target, t);

                if(t >= 1){
                    fades.RemoveAt(i);
                    callback?.Invoke();
                    continue;
                }
                fades[i] = (profile, elapsed, callback);
            }

            Process();
        }


        // --- FUNCTIONS
        /// <summary>
        /// Calls on Awake
        /// </summary>
        protected abstract void Init();
        /// <summary>
        /// Calls on Update
        /// </summary>
        protected abstract void Process();

        /// <summary>
        /// Fades a source out whilst fading another in
        /// </summary>
        /// <param name="from">The source we are fading out</param>
        /// <param name="to">The source we are fading in</param>
        /// <param name="duration">The transitional duration</param>
        /// <param name="callback">What happens once fade is complete</param>
        protected void CrossFade(AudioSource from, AudioSource to, float duration = 1, Func<bool> callback = null){
            Fade(from, duration, false, callback);
            Fade(to, duration, true);
        }

        /// <summary>
        /// Fades a source in/out
        /// </summary>
        /// <param name="source">The source we are manipulating</param>
        /// <param name="duration">The transitional duration</param>
        /// <param name="fadeIn">Are we fading in or out?</param>
        /// <param name="callback">What happens once fade is complete</param>
        protected void Fade(AudioSource source, float duration = 1, bool fadeIn = false, Func<bool> callback = null){
            if(source == null){ return; }
            fades.Add((new FadeProfile(source, duration, fadeIn), 0, callback));
        }

        public readonly struct FadeProfile{
            public FadeProfile(AudioSource source, float duration = 1, bool fadeIn = false){
                Source = source;
                Duration = duration;
                Target = fadeIn? 1 : 0;
                From = fadeIn? 0 : 1;
            }
            public readonly AudioSource Source{ get; }
            public readonly float Duration{ get; }
            public readonly float From{ get; }
            public readonly float Target{ get; }
        }
    }
}