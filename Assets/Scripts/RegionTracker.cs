using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using AstralCandle.Game;

public class RegionTracker : MonoBehaviour
{
    private string currentRegionName;
    public TMP_Text regionUI;


    // Start is called before the first frame update
    void Start()
    {

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
            // Play audio transition sound
            Audio.Instance.music.Play(region.regionTransitionSFX);
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
        }
    }
}
