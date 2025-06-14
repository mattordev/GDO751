using System;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.QOL{
    public class VSync : UserSetting{
        public VSync(string name) : base(name) { }
        public bool enableVSync;

        

        public override void Decode(string data){
            Decode(data, out string[] split);
            try{
                bool.TryParse(split[0], out enableVSync);
            }
            catch (Exception e) { Debug.LogWarning($"UserSettings: Issue with retrieving {Name} data. {e}"); }
        }

        public override string Encode() => $"{enableVSync}";
        public override void Apply() => QualitySettings.vSyncCount = enableVSync? 1 : 0;
        public override void Init() => enableVSync = QualitySettings.vSyncCount > 0;
    }
}