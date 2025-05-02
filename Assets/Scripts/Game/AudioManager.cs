using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Game{
    public abstract class AudioManager : MonoBehaviour{    
        AudioSource _new, _old;
        float duration, elapsed;

        List<(AudioSource, float, float)> fade = new();

        /// <summary>
        /// Raises volume of 1 source and lowers the other
        /// </summary>
        /// <param name="_new">The source we want to raise</param>
        /// <param name="_old">The source we want to lower</param>
        /// <param name="duration">The duration this transition will last</param>
        protected void CrossFade(AudioSource _new, AudioSource _old, float duration = 1){
            this._new = _new;
            this._old = _old;
            this.duration = duration;
            elapsed = Mathf.Lerp(0, duration, _new.volume);
        }

        protected void FadeOut(AudioSource src, float duration = 1) => fade.Add((src, duration, 0));
        float GetVolume(){
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            return Mathf.Lerp(0, 1, t);
        }

        protected virtual void Update(){
            float vol = GetVolume();

            _new.volume = vol;
            _old.volume = 1 - vol;

            // Fade out noise
            for(int i = fade.Count; i >0; i--){
                (AudioSource s, float d, float e) = fade[i];
                float elapsed = e += Time.deltaTime;

                float t = elapsed / d;
                s.volume = Mathf.Lerp(1, 0, t);
                if(t >= 1){ fade.RemoveAt(i); }
                else{ fade[i] = (s, d, elapsed); }
            }
        }
    }
}