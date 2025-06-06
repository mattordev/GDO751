using System.Collections;
using System.Collections.Generic;
using AstralCandle.Utils.Animation;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ©️YEARHERE Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle{
    [RequireComponent(typeof(Image))]
    public class Footprints : MonoBehaviour
    {
        Image _img;
        Image Image => _img ??= GetComponent<Image>();
        [SerializeField] AnimEaser easing;
        bool activate = false;
        void Update()
        {
            if (!activate) { return; }

            float value = easing.Play();
            Image.color = new(Image.color.r, Image.color.g, Image.color.b, Image.color.a * (1 - value));

            if (easing.Percent >= 1) { Destroy(gameObject); }
        }

        public void Activate()
        {
            activate = true;
            Image.enabled = true;
        }
    }
}