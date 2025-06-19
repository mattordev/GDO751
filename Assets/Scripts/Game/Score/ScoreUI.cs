using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AstralCore.Utils;
using TMPro; // TextMesh Pro for UI text rendering.


namespace mattordev.game.score
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text scorePopUp; // Text component for displaying score.

        [SerializeField] private AnimEaser animEaser; // Animation easer for score pop-up animations.

        // Intance of ScoreUI for singleton pattern.
        public static ScoreUI Instance { get; private set; }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            if (Instance == null)
            {
                Instance = this; // Set the singleton instance.
                DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed on scene load.
            }
            else
            {
                Destroy(gameObject); // Destroy duplicate instances.
                return;
            }

            if (scorePopUp == null)
            {
                Debug.LogWarning("Score Pop Up Text is not assigned!"); // Log a warning if the text component is not assigned.
            }
        }

        /// <summary>
        /// Flashes the score UI with a pop-up effect.
        /// This method updates the score pop-up text and applies an animation effect to it using the AnimEaser.
        /// </summary>
        /// <param name="score"></param>
        /// The Score to flash & add to the total score.
        /// <param name="score"></param>
        public void FlashScoreUI(int score)
        {
            if (scorePopUp != null)
            {
                scorePopUp.text = $"+{score}!";
                animEaser.Play(); // Start the easing animation

                StartCoroutine(AnimateScorePopUp()); // Run the coroutine to animate scale over time
            }
            else
            {
                Debug.LogWarning("Score Pop Up Text is not assigned!");
            }
        }

        /// <summary>
        /// Controls the animation of the score pop-up.
        /// This coroutine scales the score pop-up text from zero to a larger size and then back to zero.
        /// It uses the AnimEaser to control the timing and easing of the animation.
        /// </summary>
        /// <returns> Nothing </returns>
        private IEnumerator AnimateScorePopUp()
        {
            float duration = animEaser.Duration;
            float timer = 0f;
            float value = 0f; // Initialize value for animation progress

            // Start hidden and scaled to zero
            scorePopUp.transform.localScale = Vector3.zero;

            while (timer < duration)
            {
                value = animEaser.Play(); // Update animation
                scorePopUp.transform.localScale = Vector3.LerpUnclamped(Vector3.zero, Vector3.one * 1.5f, value);


                timer += Time.deltaTime;
                yield return null;
            }

            // Optional: snap back to zero (or one) if easing ends mid-animation
            scorePopUp.transform.localScale = Vector3.zero;
            value = animEaser.Trim(0f); // Reset the animation to the start


            // Hide the text
            scorePopUp.text = "";
        }
    }
}