using System;
using System.Collections;
using System.Collections.Generic;
using AstralCandle.Utils;
using AstralCandle.Utils.Animation;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ©️YEARHERE Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.UI{
    public class TextureSlider : MonoBehaviour{
        [SerializeField] Slider[] sliders;

        /* NOTE TO SELF:
        * ADD OFFSET FOR EACH BG ELEMENT
        * ADD TIMER TO CHANGE SIZE OF EACH ELEMENT
        * ADD A TIMER TO CHANGE THE DIRECTION
        */


        void Update(){
            foreach (Slider img in sliders){ img.Slide(); }
        }

        [Serializable]
        public class Slider{
            [SerializeField] RawImage image;
            [SerializeField] float scrollSpeed = 1;
            float? _defaultScale;
            float DefaultScale => _defaultScale ??= image.uvRect.width;

            public void Slide(){
                Rect r = image.uvRect;
                Vector2 offset = new Vector2(r.x, r.y) + (scrollSpeed/100 * Time.deltaTime * Vector2.one);

                image.uvRect = new(offset, DefaultScale * Vector2.one);
            }

        }
    }
}