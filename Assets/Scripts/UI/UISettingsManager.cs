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
        [SerializeField] TMP_Text resolution, graphics, fpsText;
        [SerializeField] Slider fps;
        [SerializeField] Toggle fullscreen, vsync;
        // ---
        AstralCore.QOL.Resolution Resolution => (AstralCore.QOL.Resolution)UserSettingsManager.settings["resolution"];
        AstralCore.QOL.Graphics Graphics => (AstralCore.QOL.Graphics)UserSettingsManager.settings["graphics"];
        AstralCore.QOL.VSync VSync => (AstralCore.QOL.VSync)UserSettingsManager.settings["vsync"];
        AstralCore.QOL.FPS FPS => (AstralCore.QOL.FPS)UserSettingsManager.settings["fps"];
        // ---
        int resIndx = 0, qualIndx = 0;

        // ---

        public void SetResolution() => SetResolution(null);
        public void SetResolution(bool? dontIncrement = null) {
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
        public void SetFullscreen(bool? v = null){
            if (v == null){
                fullscreen.SetIsOnWithoutNotify(Resolution.resolution.fullscreen);
                return;
            }
            Resolution.resolution.fullscreen = (bool)v;
        }

        public void SetVSyncUI(bool v) => SetVSync(v);
        public void SetVSync(bool? v = null){
            if (v == null){
                vsync.SetIsOnWithoutNotify(VSync.enableVSync);
                return;
            }
            VSync.enableVSync = (bool)v;
        }

        public void SetGraphics() => SetGraphics(null);
        public void SetGraphics(bool? dontIncrement = null){
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
        public void SetFPS(float? v = null){
            fpsText.text = $"{v?.ToString() ?? FPS.FPSLimit.ToString()}";
            if (v == null){
                fps.maxValue = FPS.maxFPS;
                fps.SetValueWithoutNotify(FPS.FPSLimit);
                fps.minValue = FPS.MIN_FPS;
                return;
            }
            FPS.FPSLimit = (int)v;
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
            
            
        }
    }
}