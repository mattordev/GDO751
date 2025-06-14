using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.QOL{
    public class Resolution : UserSetting{
        public Resolution(string name) : base(name) { }
        (int, int)[] _resolutions;
        /// <summary>
        /// All the available resolutions to pick from
        /// </summary>
        public (int Width, int Height)[] Resolutions => _resolutions ??= Screen.resolutions.Select(r => (r.width, r.height)).Distinct().ToArray();
        public (int width, int height, bool fullscreen) resolution;


        


        public override void Decode(string data){
            Decode(data, out string[] split);
            try{
                int.TryParse(split[0], out resolution.width);
                int.TryParse(split[1], out resolution.height);
                bool.TryParse(split[2], out resolution.fullscreen);
            }
            catch (Exception e) { Debug.LogWarning($"UserSettings: Issue with retrieving {Name} data. {e}"); }
        }

        public override string Encode() => $"{resolution.width},{resolution.height},{resolution.fullscreen}";
        public override void Apply() => Screen.SetResolution(resolution.width, resolution.height, resolution.fullscreen);
        public int GetIndex() => Array.FindIndex(Resolutions, r => r.Width == resolution.width && r.Height == resolution.height);
        
        public override void Init()
        {
            resolution.width = Screen.currentResolution.width;
            resolution.height = Screen.currentResolution.height;
            resolution.fullscreen = Screen.fullScreen;
        }
    }
}