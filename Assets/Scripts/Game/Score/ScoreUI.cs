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

        /// <summary>
        /// Flashes the score UI with a pop-up effect.
        /// This method updates the score pop-up text and applies an animation effect to it using the AnimEaser.
        /// </summary>
        /// <param name="score"></param> <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        public void FlashScoreUI(int score)
        {
            if (scorePopUp != null)
            {
                scorePopUp.text = $"+{score}!"; // Update the score pop-up text.
                float flashUI = animEaser.Play(); // Set the animation value from the easer.
                // scorePopUp.transform.localScale = new Vector3(flashUI, flashUI, 1f); // Scale the text based on the animation value.
                scorePopUp.transform.localScale = Vector3.LerpUnclamped(Vector3.one, Vector3.one * 1.5f, flashUI); // Apply a scaling effect to the text.
            }
            else
            {
                Debug.LogWarning("Score Pop Up Text is not assigned!"); // Log a warning if the text component is not assigned.
            }
        }
    }
}