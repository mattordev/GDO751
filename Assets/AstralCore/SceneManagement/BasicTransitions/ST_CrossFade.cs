using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.SceneManagement{
    [RequireComponent(typeof(Image))]
    public class ST_CrossFade : SceneTransition{
        Image _img;
        Image Image => _img ??= GetComponent<Image>();
        protected override void Update(){
            float value = easing.Play();
            Image.color = new(Image.color.r, Image.color.g, Image.color.b, value);
        }
    }
}