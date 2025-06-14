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
    public class Singleton<T>: MonoBehaviour where T: MonoBehaviour{
        static T _instance;
        /// <summary>
        /// Provides access to variables and methods found in this class
        /// </summary>
        public static T Instance{
            get{
                if (_instance == null){
                    _instance = FindObjectOfType<T>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }

        /// <summary>
        /// Attempts to create a singleton for this class
        /// </summary>
        /// <param name="destroyOnLoad">Stops the object being destoryed when loading a new scene</param>
        /// <param name="force">If a new instance is desired then when true will force the new instance to be created</param>
        /// <param name="callback">If successfully made into a singleton. What should happen afterwards?</param>
        /// <returns>Was successful?</returns>
        public bool CreateSingleton(bool destroyOnLoad = true, bool force = false, Action callback = null) {
            if (_instance == null || force) {
                Destroy(_instance?.gameObject); // Remove old
                _instance = this as T;
                callback?.Invoke();
                if (destroyOnLoad) { return true; }
                DontDestroyOnLoad(gameObject);
                return true;
            }
            return false;
        }
    }
}