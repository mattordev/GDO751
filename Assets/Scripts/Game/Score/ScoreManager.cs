using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMesh Pro for UI text rendering.


namespace mattordev.game.score
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private int score = 0; // Current score.
        [SerializeField] private int highScore = 0; // High score. Loaded from PlayerPrefs.
        [SerializeField] private string highScoreKey = "HighScore"; // Key for saving high score in PlayerPrefs.
        public TMP_Text scoreText; // Text component for displaying score.
        public TMP_Text highScoreText; // Text component for displaying high score.
        public static ScoreManager Instance { get; private set; } // Singleton instance of ScoreManager.

        // Update is called once per frame
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

            LoadHighScore(); // Load the high score from PlayerPrefs.
            UpdateHighScoreText(); // Update the high score display.
            UpdateScoreText(); // Initialize the score display.

            // Need to make this event dependent on whats happening in the game.
            StartCoroutine(AddScoreOnInterval(1f, 1)); // Start adding score every second with an amount of 10.
        }

        IEnumerator AddScoreOnInterval(float interval, int amount)
        {
            while (true) // Infinite loop to keep adding score at specified intervals.
            {
                yield return new WaitForSeconds(interval); // Wait for the specified interval.
                AddScore(amount); // Add the specified amount to the score.
                UpdateHighScore(); // Check and update high score if necessary.
            }
        }

        public void AddScore(int amount)
        {
            score += amount; // Increase score by the specified amount.
            UpdateScoreText(); // Update the UI text to reflect the new score.
        }

        void ResetScore()
        {
            score = 0; // Reset the score to zero.
            UpdateScoreText(); // Update the UI text to reflect the reset score.
        }

        void UpdateScoreText()
        {
            if (scoreText != null)
            {
                scoreText.text = score.ToString(); // Update the score text.
            }
            else
            {
                Debug.LogWarning("Score Text component is not assigned!"); // Log a warning if the score text is not set.
            }
        }

        void UpdateHighScore()
        {
            SaveHighScore(); // Save the high score if the current score is greater.
            LoadHighScore(); // Load the high score to ensure it is up-to-date.
        }

        void SaveHighScore()
        {
            if (score > highScore) // Check if the current score is greater than the high score.
            {
                highScore = score; // Update the high score.
                PlayerPrefs.SetInt(highScoreKey, highScore); // Save the high score to PlayerPrefs.
                PlayerPrefs.Save(); // Ensure changes are saved.
            }
        }

        void LoadHighScore()
        {
            highScore = PlayerPrefs.GetInt(highScoreKey, 0); // Load the high score from PlayerPrefs, defaulting to 0 if not found.
            UpdateHighScoreText(); // Update the high score display.
        }

        void UpdateHighScoreText()
        {
            if (highScoreText != null)
            {
                highScoreText.text = highScore.ToString(); // Update the high score text.
            }
            else
            {
                Debug.LogWarning("High Score Text component is not assigned!"); // Log a warning if the high score text is not set.
            }
        }

    }
}

