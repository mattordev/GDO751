using System;
using System.Collections.Generic;
using System.Text;
using AstralCore.SceneManagement;
using AstralCore.Utils;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.UI{
    public class Menu : MonoBehaviour {
        [SerializeField] UI[] ui;

        void Awake() => ShowUI();
        void Update(){
            foreach (UI u in ui){ u.Scale(); }
        }



        public void ShowUI(string category = "MainMenu"){
            foreach (UI u in ui){
                if (u.Category.Equals(category)){
                    u.Hide(false);
                    continue;
                }
                u.Hide(true);
            }
        }
       
        

        /// <summary>
        /// Forcefully loads scene without any transition
        /// </summary>
        /// <param name="sceneName">The scene we want to load</param>
        public static void ChangeScene(string sceneName = "MainMenu") => SceneManager.Instance.LoadScene(sceneName, "crossfade");

        /// <summary>
        /// Quits Application
        /// </summary>
        public static void Quit() => Application.Quit();


        [Serializable] public class UI {
            [SerializeField] string category;
            [SerializeField] AnimEaser easing;
            [SerializeField] RectTransform[] mainUI;

            Vector2[] _dScale;
            Vector2[] DefaultScale {
                get {
                    if (_dScale == null) {
                        _dScale = new Vector2[mainUI.Length];
                        for (int i = 0; i < _dScale.Length; i++) {
                            _dScale[i] = mainUI[i].localScale;
                        }
                    }
                    return _dScale;
                }
            }

            public void Hide(bool doHide) => easing.SetReverse(doHide);

            public void Scale(){
                float t = easing.Play();
                for (int i = 0; i < mainUI.Length; i++)
                {
                    mainUI[i].localScale = Vector2.LerpUnclamped(Vector2.zero, DefaultScale[i], t);
                }
            }

            public string Category => category;
        }
    }
}