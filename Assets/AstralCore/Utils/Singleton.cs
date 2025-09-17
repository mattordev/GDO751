using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.Utils{
    /// <summary>
    /// Extends the monobehaviour with functionality allowing this class to be referenced
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : Component{
        [Header("Singleton Setting")]
        [SerializeField, Tooltip("If true, this object wont destroy when changing scenes thus stays accessible")] bool persistanceAcrossScenes = false;
        static T _instance;
        /// <summary>
        /// Provides access to variables and methods found in this class
        /// </summary>
        public static T Instance{
            get{
                if (_instance == null){
                    GameObject instancedObj = new();
                    Debug.LogWarning($"{typeof(T).Name} doesn't exist... Creating instance!");
                    instancedObj.name = $"{typeof(T).Name}";
                    _instance = instancedObj.AddComponent<T>();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Attempts to create a singleton for this class
        /// </summary>
        /// <param name="force">If a new instance is desired then when true will force the new instance to be created</param>
        /// <param name="callback">If successfully made into a singleton. What should happen afterwards?</param>
        /// <returns>Was successful?</returns>
        public bool CreateSingleton(bool force = false, Action callback = null){
            if (_instance == null || force){
                if (_instance){ Destroy(_instance.gameObject); } // Remove old

                _instance = this as T;
                callback?.Invoke();
                if (!persistanceAcrossScenes) { return true; }
                DontDestroyOnLoad(gameObject);
                return true;
            }
            return false;
        }



        // Attempt to create a singleton, If one already exists then this will be ignored
        protected virtual void Awake() => CreateSingleton();
    }
}