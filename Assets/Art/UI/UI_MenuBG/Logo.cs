using System.Collections;
using System.Collections.Generic;
using AstralCore.Utils;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.UI{
    public class Logo : MonoBehaviour{
        [SerializeField] AnimEaser easer;
        [SerializeField] float bounceHeight = 10;
        [SerializeField] MinMax<Vector2> scales;

        RectTransform _t;
        RectTransform Rect => _t ??= transform.GetChild(0) as RectTransform;
        float? yOffset;
        float YOffset => yOffset ??= Rect.anchoredPosition.y;
        

        void Update(){
            float timeValue = Mathf.Sin(Time.time / (easer.Duration/2));
            timeValue = Mathf.InverseLerp(-1, 1, timeValue); // Maps it between 0, 1
            float value = easer.Trim(timeValue);

            Rect.localScale = Vector2.LerpUnclamped(scales.min, scales.max, value);
            Rect.anchoredPosition = new(0, Mathf.LerpUnclamped(YOffset, YOffset + bounceHeight, value));
        }
    }
}