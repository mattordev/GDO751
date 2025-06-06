using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.UI{
    public class Menu : MonoBehaviour{
        // UI swapping system
        // scene transitions
        // pause system
        // post processing blur when paused


        /// <summary>
        /// Forcefully loads scene without any transition
        /// </summary>
        /// <param name="sceneName">The scene we want to load</param>
        public static void ChangeScene(string sceneName = "MainMenu") => SceneManager.LoadScene(sceneName);

        /// <summary>
        /// Quits Application
        /// </summary>
        public static void Quit() => Application.Quit();
    }
}