using System;
using System.Collections;
using System.Collections.Generic;
using AstralCandle.Utils.Animation;
using AstralCandle.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.UI{
    [RequireComponent(typeof(Image))]
    public class JoshsButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler{
        [SerializeField] Settings settings;
        [SerializeField] AnimEaser easing;
        [SerializeField, Tooltip("What happens when the user clicks on this button")] UnityEvent onClick;

        Image _img;
        Image Image => _img ??= GetComponent<Image>();
        Color? _defaultColour;
        Color DefaultColour => _defaultColour ??= Image.color;
        bool isHovering;


        void Animate()
        {
            easing.SetReverse(!isHovering);
            float value = easing.Play();
            Image.fillAmount = Mathf.LerpUnclamped(settings.minMaxSlider.min, settings.minMaxSlider.max, value);

            float flash = Mathf.Sin(Time.time * settings.flashingFrequency) / settings.flashingAmplitude;
            flash = Maths<float>.Remap(flash, -1, 1, 0, 1);
            Color flashColour = Color.LerpUnclamped(DefaultColour * settings.colourMultiplier.min, DefaultColour * settings.colourMultiplier.max, flash);
            Image.color = Color.LerpUnclamped(DefaultColour, flashColour, value);
        }

        public void OnPointerClick(PointerEventData eventData) => onClick?.Invoke();
        public void OnPointerEnter(PointerEventData eventData) => isHovering = true;
        public void OnPointerExit(PointerEventData eventData) => isHovering = false;


        void Update() => Animate();

        [Serializable]
        public class Settings{
            [Tooltip("Fill amount of the slider on the image")] public Maths<float>.MinMax minMaxSlider = new(0, 1);
            [Tooltip("Multiplied by the \"default\" colour")] public Maths<Color>.MinMax colourMultiplier = new(Color.white, Color.white);
            [Tooltip("How fast does it flash?")] public float flashingFrequency = 3;
            [Tooltip("The range of the flash")] public float flashingAmplitude = 1;
        }
    }
}