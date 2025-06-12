using AstralCore.SFX;
using AstralCore.Utils;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.UI{
    public class MainMenuMusic : MonoBehaviour{
        [SerializeField] AudioClip main, overlay;
        [SerializeField] MinMax<float> sfxInterval;

        void Start() => Audio.Instance.music.Play(new(main, overlay, sfxInterval));
    }
}