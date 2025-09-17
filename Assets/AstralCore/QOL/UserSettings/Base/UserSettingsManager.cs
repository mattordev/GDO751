using System;
using System.Collections.Generic;
using System.Linq;
using AstralCore.Utils;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.QOL{
    public abstract class UserSettingsManager : Singleton<UserSettingsManager>{
        [SerializeField] bool showDebug = false;
        /// <summary>
        /// The datastore name
        /// </summary>
        protected const string PREFS_NAME = "user-settings";
        public static readonly Dictionary<string, UserSetting> settings = new();

        /// <summary>
        /// Return all modules to be added into the manager
        /// </summary>
        protected abstract UserSetting[] CreateModules();




        /// <summary>
        /// Gets stored data 
        /// </summary>
        void GetData(out string data) => data = PlayerPrefs.GetString(PREFS_NAME);
        /// <summary>
        /// Get keys from settings
        /// </summary>
        IEnumerable<string> settingsKeys => settings.Keys;


        /// <summary>
        /// Adds the settings module to the settings dictionary
        /// </summary>
        /// <param name="settings">The settings module(s) to add</param>
        void Add(params UserSetting[] _settings){
            foreach (UserSetting s in _settings) { settings[s.Name] = s; }
        }

        /// <summary>
        /// Initiates all modules
        /// </summary>
        void Init(){
            Add(CreateModules()); // Add all modules to dictionary

            GetData(out string data);
            if (showDebug){ Debug.Log($"User Settings: {data}"); }
            // If no save data...
            if (string.IsNullOrEmpty(data)){
                if (showDebug) { Debug.Log("No UserSettings... Initialising!"); }
                foreach (string k in settingsKeys){
                    settings[k].Init();
                    settings[k].Apply();
                }
                return; 
            }
            

            // If save data
            foreach (string segment in data.Split('|')){
                string[] parts = segment.Split('=');
                if (parts.Length != 2) { continue; } // Something wrong with this setting... - If less or greater args

                // Set and Apply
                if (settings.TryGetValue(parts[0], out UserSetting setting)){
                    setting.Decode(parts[1]); // Cache setting
                    setting.Apply();
                }
            }
        }

        /// <summary>
        /// Saves all the settings
        /// </summary>
        public static void Save(){
            string serialised = string.Join('|', settings.Select(m => $"{m.Key}={m.Value.Encode()}")); // Gathers all the settings into one
            PlayerPrefs.SetString(PREFS_NAME, serialised);
            PlayerPrefs.Save();
        }
        /// <summary>
        /// Applies the settings
        /// </summary>
        public static void Apply(){
            foreach (KeyValuePair<string, UserSetting> kv in settings){
                kv.Value.Apply();
            }
        }

        [ContextMenu("Reset User Settings")]
        public void Reset(){
            PlayerPrefs.DeleteKey(PREFS_NAME);
            Init();
            Debug.Log("Successfully reset user settings!");
        }



        protected override void Awake(){
            base.Awake();
            Init();
        }
    }
}