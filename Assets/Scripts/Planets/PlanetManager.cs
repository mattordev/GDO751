using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AstralCandle.Utilities;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Game{
    public sealed class PlanetManager : MonoBehaviour, ISingleton<PlanetManager>{
        [SerializeField] List<Planet> _planets = new();
        HashSet<Planet> planets = new();

        public void AddPlanet(Planet planet) => planets.Add(planet);
        public Vector3 Attract(Transform obj) => planets.Aggregate(Vector3.zero, (acc, planet) => acc + planet.Attract(obj));
        public Transform GetNearestPlanet(Transform obj) => planets.OrderBy(p => Vector3.Distance(obj.position, p.transform.position)).FirstOrDefault().transform;
        
        void Awake(){
            if(!(this as ISingleton<PlanetManager>).CreateSingleton(this)){
                Destroy(gameObject);
                return;
            }

            _planets.ForEach(planet => planets.Add(planet));
        }
    }
}