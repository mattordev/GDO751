using System;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.QOL{
    public class Graphics : UserSetting{
        public Graphics(string name) : base(name) { }
        string[] _graphics;
        /// <summary>
        /// All the available resolutions to pick from
        /// </summary>
        public string[] GraphicsProfile => _graphics ??= QualitySettings.names;
        public string graphicsProfile;
        

        public override void Decode(string data){
            Decode(data, out string[] split);
            try{
                graphicsProfile = split[0];
            }
            catch (Exception e) { Debug.LogWarning($"UserSettings: Issue with retrieving {Name} data. {e}"); }
        }

        public override string Encode() => $"{graphicsProfile}";
        public override void Apply() => QualitySettings.SetQualityLevel(GetIndex(), true);
        public int GetIndex() => Array.FindIndex(GraphicsProfile, g => g == graphicsProfile);
        public override void Init(){
            graphicsProfile = GraphicsProfile[QualitySettings.GetQualityLevel()];
        }
    }
}