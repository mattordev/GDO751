using TMPro;
using UnityEngine;
using AstralCore.SFX;
using AstralCore.Utils;
using mattordev.game.score;
using System.Collections.Generic;
using System.Collections;

/// <author>
/// ©️2025 Designed and Programmed by Matthew Roberts. All rights reserved.
/// </author>

namespace mattordev.regions
{
    public class RegionTracker : MonoBehaviour
    {
        private string currentRegionName;
        public TMP_Text regionUI;
        [SerializeField] private AnimEaser animEaser; // Animation easer for score pop-up animations.

        // list of all visited regions
        private List<Region> visitedRegions = new List<Region>();

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            // Load visited regions from PlayerPrefs
            LoadVisitedRegions();

            // Initialize the UI text
            regionUI.text = currentRegionName;
        }

        void LoadVisitedRegions()
        {
            string visitedRegionNames = PlayerPrefs.GetString("VisitedRegions", "");
            if (!string.IsNullOrEmpty(visitedRegionNames))
            {
                string[] regionNames = visitedRegionNames.Split(',');
                foreach (string regionName in regionNames)
                {
                    Region region = FindRegionByName(regionName);
                    if (region != null)
                    {
                        visitedRegions.Add(region);
                    }
                    else
                    {
                        Debug.LogWarning($"Region '{regionName}' not found in the scene. It may have been removed or renamed.");
                    }
                }
            }
            else
            {
                Debug.Log("No visited regions found in PlayerPrefs.");
            }
        }

        /// <summary>
        /// Finds a region by its name in the scene.
        /// </summary>
        /// <param name="regionName">The name of the region to find.</param>
        /// <returns>The Region object if found, otherwise null.</returns>
        /// <remarks>
        /// This method searches through all Region objects in the scene to find one with the specified name.
        /// </remarks>

        Region FindRegionByName(string regionName)
        {
            // Find the region by name in the scene
            Region[] regions = FindObjectsOfType<Region>();
            foreach (Region region in regions)
            {
                if (region.regionName == regionName)
                {
                    return region; // Return the found region
                }
            }
            return null; // Return null if no region found
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Region")
            {

                // Get the region component
                Region region = other.GetComponent<Region>();

                // Get the region name
                currentRegionName = region.regionName;
                // display it
                regionUI.text = currentRegionName;

                StartCoroutine(FadeRegionUI());

                // Play audio transition sound
                // Audio.Instance.music.Play(new(region.regionTransitionSFX));

                // Check if the region has already been visited
                if (visitedRegions.Contains(region))
                {
                    return; // If already visited, do not process further.
                }
                // Add the new region to the visited list
                visitedRegions.Add(region);

                ScoreManager.Instance.AddScore(region.scoreBoostWhenEntering); // Add score boost when entering the region.
                ScoreUI.Instance.FlashScoreUI(region.scoreBoostWhenEntering, "New Region"); // Flash the score UI with the score boost.
                Debug.Log($"Entered region: {currentRegionName}. Score boosted by {region.scoreBoostWhenEntering}.");
            }
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.tag == "Region")
            {
                currentRegionName = "";
                // display it
                regionUI.text = currentRegionName;

                // reset the text alpha to default whilst maintaning the current color
                regionUI.color = new Color(regionUI.color.r, regionUI.color.g, regionUI.color.b, 1f);


                // save region list to PlayerPrefs
                string visitedRegionNames = string.Join(",", visitedRegions.ConvertAll(r => r.regionName));
                PlayerPrefs.SetString("VisitedRegions", visitedRegionNames);
                PlayerPrefs.Save(); // Save the PlayerPrefs to persist the data.
            }
        }

        /// <summary>
        /// Animates the score UI with a fade-out effect.
        /// This coroutine fades out the score pop-up text over a specified duration using the AnimEaser.
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeRegionUI()
        {
            float duration = animEaser.Duration;
            float timer = 0f;

            animEaser.Play();

            Color startTextColor = regionUI.color;
            Color fadedLabelColor = new Color(startTextColor.r, startTextColor.g, startTextColor.b, 0f);

            while (timer < duration)
            {
                float value = animEaser.TrimInSeconds(timer);
                regionUI.color = Color.Lerp(startTextColor, fadedLabelColor, value);

                timer += Time.deltaTime;
                yield return null;
            }

            // Clean up
            regionUI.text = "";
            regionUI.color = startTextColor;
            animEaser.Trim(0f); // Reset easer
        }

    }
}

