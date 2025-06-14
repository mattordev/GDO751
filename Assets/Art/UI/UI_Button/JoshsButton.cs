using System;
using System.Collections;
using System.Collections.Generic;
using AstralCore.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.UI
{
    [RequireComponent(typeof(Image))]
    public class JoshsButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler{
        [SerializeField] public TMP_Text label;
        [SerializeField] Image image;
        [SerializeField] Settings settings;
        [SerializeField] AnimEaser easing;
        [SerializeField, Tooltip("What happens when the user clicks on this button")] UnityEvent onClick;
        [SerializeField, Tooltip("What happens when the user hovers over this button")] UnityEvent onHoverEnter;
        [SerializeField, Tooltip("What happens when the user no longer hovers over this button")] UnityEvent onHoverExit;

        Image _img;
        Image Image => _img ??= GetComponent<Image>();


        Vector2? _defaultScale;
        Vector2 DefaultScale => _defaultScale ??= transform.localScale;
        Color? _defaultLabCol;
        Color DefaultLabCol => _defaultLabCol ??= (image?.color ?? label.color);

        Color? _defaultColour;
        Color DefaultColour => _defaultColour ??= Image.color;
        bool isHovering;


        void Animate()
        {
            easing.SetReverse(!isHovering);
            float value = easing.Play();
            Image.fillAmount = Mathf.LerpUnclamped(settings.minMaxSlider.min, settings.minMaxSlider.max, value);
            
            if (image)
            {
                image.color = Color.LerpUnclamped(DefaultLabCol, DefaultLabCol * settings.colourMultiplier.min, value);
            }
            else if (label)
            {
                label.color = Color.LerpUnclamped(DefaultLabCol, DefaultLabCol * settings.colourMultiplier.min, value);
            }
            

            float flash = Mathf.Sin(Time.time * settings.flashingFrequency) / settings.flashingAmplitude;
            flash = Methods.Remap(flash, -1, 1, 0, 1);
            Color flashColour = Color.LerpUnclamped(DefaultColour * settings.colourMultiplier.min, DefaultColour * settings.colourMultiplier.max, flash);
            Image.color = Color.LerpUnclamped(DefaultColour, flashColour, value);

            float x = Mathf.Lerp(settings.scaling.min, settings.scaling.max, flash);
            float y = Mathf.Lerp(settings.scaling.min, settings.scaling.max, 1 - flash);
            Image.transform.localScale = Vector2.Lerp(DefaultScale, Vector3.Scale(DefaultScale, new(x, y)), value);
        }

        public void OnPointerClick(PointerEventData eventData) => onClick?.Invoke();
        public void OnPointerEnter(PointerEventData eventData){
            isHovering = true;
            onHoverEnter.Invoke();
        }
        public void OnPointerExit(PointerEventData eventData){
            isHovering = false;
            onHoverExit?.Invoke();
        }


        void Update() => Animate();

        [Serializable]
        public class Settings
        {
            [Tooltip("Fill amount of the slider on the image")] public MinMax<float> minMaxSlider = new(0, 1);
            [Tooltip("Multiplied by the \"default\" colour")] public MinMax<float> scaling = new(.95f, 1.1f);
            [Tooltip("Multiplied by the \"default\" colour")] public MinMax<Color> colourMultiplier = new(Color.white, Color.white);
            [Tooltip("How fast does it flash?")] public float flashingFrequency = 3;
            [Tooltip("The range of the flash")] public float flashingAmplitude = 1;
        }
    }
}