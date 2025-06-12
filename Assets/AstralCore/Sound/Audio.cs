using System;
using System.Collections;
using System.Collections.Generic;
using AstralCore.Utils;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.SFX{
    public sealed class Audio : Singleton<Audio>{
        

        public Sound sound;
        public Music music;


        void Awake() => CreateSingleton(false);
        void Update() => music.Process();



        public enum SoundType
        {
            SOUND,
            MUSIC
        }
    }
}