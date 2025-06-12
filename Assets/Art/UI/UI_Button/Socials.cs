using System.Collections;
using System.Collections.Generic;
using AstralCore.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.UI{
    public class Socials : MonoBehaviour, IPointerExitHandler{
        [SerializeField] AnimEaser easing;

        [SerializeField] RectTransform[] socialButtons;

        Vector3[] _defaultPos;
        Vector3[] DefaultPositions
        {
            get{
                if (_defaultPos == null){
                    _defaultPos = new Vector3[socialButtons.Length];
                    for (int i = 0; i < _defaultPos.Length; i++)
                    {
                        _defaultPos[i] = socialButtons[i].anchoredPosition;
                    }
                }
                return _defaultPos;
            }
        }

        void Awake(){
            for (int i = 0; i < DefaultPositions.Length; i++){
                socialButtons[i].anchoredPosition = Vector2.zero;
            }
            Hovering(false);
        }

        void Update(){
            float val = easing.Play();
            for (int i = 0; i < DefaultPositions.Length; i++)
            {
                socialButtons[i].anchoredPosition = Vector2.Lerp(Vector2.zero, DefaultPositions[i], val);
                socialButtons[i].gameObject.SetActive(val > 0);
            }
        }

        public void Hovering(bool isHovering) => easing.SetReverse(!isHovering);

        public void OpenURL(string url) => Application.OpenURL(url);

        public void OnPointerExit(PointerEventData eventData) => Hovering(false);
    }
}