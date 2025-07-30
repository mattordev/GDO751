using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AstralCore.Utils;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.SceneManagement{
    public sealed class SceneManager : Singleton<SceneManager> {
        /// <summary>
        /// Triggered when changing scenes
        /// </summary>
        public static event Action OnChangingScenes;
        [SerializeField] Transition[] transitions;

        public static bool LoadingScene { get; private set; } = false;

        Dictionary<string, SceneTransition> _transitions;
        Dictionary<string, SceneTransition> Transitions{
            get{
                if (_transitions == null){
                    _transitions = new();
                    foreach (Transition t in transitions){ _transitions.Add(t.name, t.transition); }
                }
                return _transitions;
            }
        }

        /// <summary>
        /// Loads the scene with a transition
        /// </summary>
        /// <param name="sceneName">The scene we want to load</param>
        /// <param name="transitionName">The transition we want to use</param>
        public void LoadScene(string sceneName, string transitionName = null) {
            if(LoadingScene){ return; }

            if (!Instance.Transitions.TryGetValue(transitionName, out SceneTransition s)){
                Debug.LogWarning($"WARNING: Unable to transition to \"{sceneName}\". Transition: \"{transitionName}\" not found. Check names! (Case sensitive)");
            }
            OnChangingScenes?.Invoke();
            StartCoroutine(LoadSceneAsync(sceneName, s));
        }

        /// <summary>
        /// Reloads current scene
        /// </summary>
        public void ReloadScene() => LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, "crossfade");


        IEnumerator LoadSceneAsync(string sceneName, SceneTransition transition) {
            LoadingScene = true;
            AsyncOperation scene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            scene.allowSceneActivation = false;
            yield return transition?.In();
            do { transition?.WhileLoading(scene.progress); } // Callback
            while (scene.progress < .9f);
            scene.allowSceneActivation = true;
            LoadingScene = false;
            yield return transition?.Out();
        }

        [Serializable] public struct Transition {
            [Tooltip("The name of this transition")] public string name;
            [Tooltip("The transition script to use")] public SceneTransition transition;
        }
    }
}