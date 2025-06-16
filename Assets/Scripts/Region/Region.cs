using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


namespace mattordev.regions
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class Region : MonoBehaviour
    {
        public string regionName;
        public BoxCollider regionCollider; // region collider.

        public AudioClip regionTransitionSFX; // sound effect to play when entering this region.

        public enum weatherType
        {
            Clear,
            Sunny,
            Raining,
            Thunderstorm,
            Foggy
        }

        public weatherType currentWeatherType;

        // Start is called before the first frame update
        void Start()
        {
            currentWeatherType = weatherType.Clear; // set default weather.

            if (regionCollider == null)
                regionCollider = GetComponent<BoxCollider>();
        }
    }
}

