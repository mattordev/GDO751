using AstralCore.Utils;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.SFX{
    public sealed class Audio : Singleton<Audio>{
        

        public Sound sound;
        public Music music;


        void Update() => music.Process();



        public enum SoundType
        {
            SOUND,
            MUSIC
        }
    }
}