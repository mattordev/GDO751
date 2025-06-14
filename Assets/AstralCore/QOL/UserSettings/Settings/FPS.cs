using System;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.QOL{
    public class FPS : UserSetting{
        public const int MIN_FPS = 24;
        public int maxFPS => Screen.currentResolution.refreshRate;

        public FPS(string name) : base(name) { }
        int _fps;
        public int FPSLimit{
            get => _fps;
            set => _fps = Mathf.Clamp(value, MIN_FPS, maxFPS);
        }

        

        public override void Decode(string data){
            Decode(data, out string[] split);
            try{
                int.TryParse(split[0], out _fps);
            }
            catch (Exception e) { Debug.LogWarning($"UserSettings: Issue with retrieving {Name} data. {e}"); }
        }

        public override string Encode() => $"{FPSLimit}";
        public override void Apply() => Application.targetFrameRate = FPSLimit;
        public override void Init()
        {
            FPSLimit = maxFPS;
        }
    }
}