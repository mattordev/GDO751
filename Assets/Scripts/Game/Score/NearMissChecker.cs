using UnityEngine;

/// <author>
/// ©️2025 Designed and Programmed by Matthew Roberts. All rights reserved.
/// </author>

namespace mattordev.game.score
{
    public class NearMissChecker : MonoBehaviour{
        [SerializeField] float scoreMultiplier = 1;
        [SerializeField] private float nearMissDistance = 1.0f; // Distance threshold for a near miss.
        [SerializeField] private LayerMask targetLayer; // Layer mask to specify which objects are targets for near misses.
        int touchedColliders; // Used to calculate how many points to give the player once they are no longer risk of hitting it
        float timeNearColliders; // Timer to count how long we have been at risk at hitting something

        // Update is called once per frame
        void Update()
        {
            CheckForNearMisses();

        }

        /// <summary>
        /// Checks for near misses by detecting colliders within a specified distance.
        /// This method uses Physics.OverlapSphere to find colliders within the near miss distance.
        /// </summary>
        void CheckForNearMisses()
        {
            // Get all colliders within the near miss distance.
            int hitColliders = Physics.OverlapSphere(transform.position, nearMissDistance, targetLayer).Length;
            int dif = Mathf.Max(touchedColliders - hitColliders, 0); // If num of hit colliders is less than touched. Then we have successfully evaded thus reward!


            // Raise & Reset timer based on detected collisions
            timeNearColliders = (touchedColliders > 0) ? timeNearColliders + Time.deltaTime : 0;

            if (dif > 0 && timeNearColliders > 0)
            {
                int score = dif * Mathf.CeilToInt(timeNearColliders * scoreMultiplier);
                ScoreManager.Instance.AddScore(score); // Add score for the near miss.
                ScoreUI.Instance.FlashScoreUI(score, "Near Miss"); // Flash the score UI with the added score.
            }
            touchedColliders = hitColliders; // Update wih new colliders
        }

        void OnDrawGizmos()
        {
            // Draw a sphere in the editor to visualize the near miss distance.
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, nearMissDistance);
        }
    }
}

