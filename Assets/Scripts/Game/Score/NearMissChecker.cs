using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author>
/// ©️2025 Designed and Programmed by Matthew Roberts. All rights reserved.
/// </author>

namespace mattordev.game.score
{
    public class NearMissChecker : MonoBehaviour
    {
        [SerializeField] private float nearMissDistance = 1.0f; // Distance threshold for a near miss.
        [SerializeField] private float nearMissCooldown = 5.0f; // Cooldown time before a target can be detected again.
        [SerializeField] private LayerMask targetLayer; // Layer mask to specify which objects are targets for near misses.
        private Collider[] hitColliders; // Array to store colliders that are within the near miss distance.

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
            hitColliders = Physics.OverlapSphere(transform.position, nearMissDistance, targetLayer);

            // Loop through each collider to check for near misses.
            foreach (var hitCollider in hitColliders)
            {
                // Check if the collider is a target object using layer comparison.
                // Assuming the target objects are tagged with "NearMissTarget".
                if (hitCollider.gameObject.layer == LayerMask.NameToLayer("NearMissTarget"))
                {
                    // Log the near miss and update the score.

                    Debug.Log("Near miss detected with: " + hitCollider.name);
                    ScoreManager.Instance.AddScore(10); // Add score for the near miss.
                    // need to flash the score addition ui on screen.
                }

                // Change layer to prevent further detection.
                hitCollider.gameObject.layer = LayerMask.NameToLayer("Default"); // Reset the layer to prevent further detection.
                //change it back after a short delay.
                StartCoroutine(ResetLayerAfterDelay(hitCollider.gameObject, 5f));
            }

            IEnumerator ResetLayerAfterDelay(GameObject target, float delay)
            {
                yield return new WaitForSeconds(delay);
                target.layer = LayerMask.NameToLayer("NearMissTarget"); // Reset the layer back to the target layer.
                Debug.Log("Near miss cooldown complete for: " + target.name);
            }

            void OnDrawGizmos()
            {
                // Draw a sphere in the editor to visualize the near miss distance.
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, nearMissDistance);
            }
        }
    }
}