using System;
using AstralCore.Utils;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.QOL{
    public class Sound : UserSetting
    {
        public Sound(string name) : base(name) { }
        public float master, sound, music;

        public override void Decode(string data)
        {
            Decode(data, out string[] split);
            try
            {
                float.TryParse(split[0], out master);
                float.TryParse(split[1], out sound);
                float.TryParse(split[2], out music);
            }
            catch (Exception e) { Debug.LogWarning($"UserSettings: Issue with retrieving {Name} data. {e}"); }
        }

        public override string Encode() => $"{master},{sound},{music}";
        public override void Apply()
        {
            UserSettingsManager.Mixer.SetFloat("master", GetValue(master));
            UserSettingsManager.Mixer.SetFloat("sfx", GetValue(sound));
            UserSettingsManager.Mixer.SetFloat("music", GetValue(music));
            UserSettingsManager.Mixer.GetFloat("master", out float v);
        }
        public override void Init(){ 
            UserSettingsManager.Mixer.SetFloat("master", GetValue(1));
            UserSettingsManager.Mixer.SetFloat("sfx", GetValue(1));
            UserSettingsManager.Mixer.SetFloat("music", GetValue(1));
        }

        float GetValue(float v) => Mathf.Log10(Mathf.Max(Mathf.Epsilon, v)) * 20;


        public enum SType
        {
            Master,
            SFX,
            Music
        }
    }
}