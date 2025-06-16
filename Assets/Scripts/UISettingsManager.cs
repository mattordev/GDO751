using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AstralCore.QOL;
using AstralCore.Utils;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle{
    public class UISettingsManager : MonoBehaviour{
        [SerializeField] TMP_Text resolution, graphics, fpsText, masterText, sfxText, musicText;
        [SerializeField] Slider fps, master, sfx, music;
        [SerializeField] Toggle fullscreen, vsync;
        // ---
        AstralCore.QOL.Resolution Resolution => (AstralCore.QOL.Resolution)UserSettingsManager.settings["resolution"];
        AstralCore.QOL.Graphics Graphics => (AstralCore.QOL.Graphics)UserSettingsManager.settings["graphics"];
        AstralCore.QOL.VSync VSync => (AstralCore.QOL.VSync)UserSettingsManager.settings["vsync"];
        AstralCore.QOL.FPS FPS => (AstralCore.QOL.FPS)UserSettingsManager.settings["fps"];
        AstralCore.QOL.Sound Sound => (AstralCore.QOL.Sound)UserSettingsManager.settings["sound"];
        // ---
        int resIndx = 0, qualIndx = 0;

        // ---

        public void SetResolution() => SetResolution(null);
        void SetResolution(bool? dontIncrement = null) {
            (int width, int height) r = (Resolution.resolution.width, Resolution.resolution.height);

            if (dontIncrement == true) {
                resIndx = Resolution.GetIndex();
                resolution.text = $"{r.width}x{r.height}";
                return;
            }
            Methods.ModulusCounter(ref resIndx, Resolution.Resolutions.Length);
            (r.width, r.height) = Resolution.Resolutions[resIndx];
            (Resolution.resolution.width, Resolution.resolution.height) = r;
            resolution.text = $"{r.width}x{r.height}";
        }

        public void SetFullscreenUI(bool v) => SetFullscreen(v);
        void SetFullscreen(bool? v = null){
            if (v == null){
                fullscreen.SetIsOnWithoutNotify(Resolution.resolution.fullscreen);
                return;
            }
            Resolution.resolution.fullscreen = (bool)v;
        }

        public void SetVSyncUI(bool v) => SetVSync(v);
        void SetVSync(bool? v = null){
            if (v == null){
                vsync.SetIsOnWithoutNotify(VSync.enableVSync);
                return;
            }
            VSync.enableVSync = (bool)v;
        }

        public void SetGraphics() => SetGraphics(null);
        void SetGraphics(bool? dontIncrement = null){
            string g = Graphics.graphicsProfile;
            if (dontIncrement == true){
                qualIndx = Graphics.GetIndex();
                graphics.text = $"{g}";
                return;
            }
            Methods.ModulusCounter(ref qualIndx, Graphics.GraphicsProfile.Length);
            g = Graphics.GraphicsProfile[qualIndx];
            Graphics.graphicsProfile = g;
            graphics.text = $"{g}";
        }

        public void SetFPSUI(float v) => SetFPS(v);
        void SetFPS(float? v = null){
            fpsText.text = $"{v?.ToString() ?? FPS.FPSLimit.ToString()}";
            if (v == null){
                fps.maxValue = FPS.maxFPS;
                fps.SetValueWithoutNotify(FPS.FPSLimit);
                fps.minValue = FPS.MIN_FPS;
                return;
            }
            FPS.FPSLimit = (int)v;
        }


        public void SetMaster(float v) => SetSound(Sound.SType.Master, v);
        public void SetSFX(float v) => SetSound(Sound.SType.SFX, v);
        public void SetMusic(float v) => SetSound(Sound.SType.Music, v);

        void SetSound(Sound.SType sType, float? v = null){
            float _v = Mathf.Round((v ?? 0) * 100);
            float fV = _v / 100;
            Slider _s = null;
            TMP_Text _t = null;
            float baseV = 0;
            if (sType.Equals(Sound.SType.Master))
            {
                _s = master;
                _t = masterText;
                baseV = Sound.master;
                Sound.master = v != null ? fV : Sound.master;
            }
            else if (sType.Equals(Sound.SType.SFX))
            {
                _s = sfx;
                _t = sfxText;
                baseV = Sound.sound;
                Sound.sound = v != null ? fV : Sound.sound;
            }
            else if (sType.Equals(Sound.SType.Music))
            {
                _s = music;
                _t = musicText;
                baseV = Sound.music;
                Sound.music = v != null ? fV : Sound.music;
            }

            _t.text = $"{(v != null? _v.ToString("F0") : baseV * 100)}%";
            if (v == null){ _s?.SetValueWithoutNotify(baseV); }
        }
        // ---


        // ---
        public void Save() => UserSettingsManager.Save();
        public void Apply() => UserSettingsManager.Apply();
        void Start(){
            SetResolution(true);
            SetFullscreen();
            SetVSync();
            SetGraphics(true);
            SetFPS();
            SetSound(Sound.SType.Master);
            SetSound(Sound.SType.SFX);
            SetSound(Sound.SType.Music);
        }
    }
}