﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Golcon
{
    [CreateAssetMenu(fileName = "Create Planet Factory", order = 10000)]
    public class PlanetFactory : ScriptableObject
    {

#pragma warning disable CS0649
        [SerializeField] PlanetController _planetPrefab;
        [SerializeField] Vector2 planetScaleRange;
        [SerializeField] Vector2 planeEscadrilleAmountRange;
#pragma warning restore CS0649

        List<PlanetController> createdPlanets = new List<PlanetController>();
        Vector3 minPoint;
        Vector3 maxPoint;

        public void Initialize(Vector3 minBorderPoint, Vector3 maxBorderPoint)
        {
            minPoint = minBorderPoint;
            maxPoint = maxBorderPoint;
            DeleteAllPlanets();


        }
        public PlanetController CreatePlanet()
        {
            PlanetController planet = Instantiate(_planetPrefab);
            createdPlanets.Add(planet);
            return planet;
        }

        private void DeletePlanet(PlanetController planet)
        {
            createdPlanets.Remove(planet);
            Destroy(planet.gameObject);
        }

        private void DeleteAllPlanets()
        {
            foreach(var planet in createdPlanets)
            {
               if(planet!=null) Destroy(planet.gameObject);
            }
            createdPlanets.Clear();
        }

        public PlanetController CreateRandomPlanet(Transform parent)
        {
            float scale = Random.Range(planetScaleRange.x, planetScaleRange.y);
            PlanetController planet = CreatePlanet();
            PlanetSettings settings = planet.GetSettings();

            settings.Scale = scale;
            settings.ShipsAmount = (int)Random.Range(planeEscadrilleAmountRange.x, planeEscadrilleAmountRange.y);
            planet.Setup(settings);

            Vector3 point;
            if (!TryGetSpawnPoint(planet, out point))
            {
                DeletePlanet(planet);
                return null;
            }

            planet.transform.position = point;
            planet.transform.parent = parent;




            return planet;
        }

        //Brute method of searching spawn points by rule: the new planet must be located on distance of 2 radius or more from another planet
        //not ideal but fast develop
        private bool TryGetSpawnPoint(PlanetController planet, out Vector3 point)
        {
            Bounds planetBounds = planet.GetBounds;
            Vector3 minSpawnPoint = minPoint + planetBounds.extents;
            Vector3 maxSpawnPoint = maxPoint - planetBounds.extents;
            
            point = new Vector3(Random.Range(minSpawnPoint.x, maxSpawnPoint.x), Random.Range(minSpawnPoint.y, maxSpawnPoint.y), planet.transform.position.z);
            int i = 0;//looping protection
            while (!SpawnPointBruteCheck(point, planet) && i < 1000)
            {
                point = new Vector3(Random.Range(minSpawnPoint.x, maxSpawnPoint.x), Random.Range(minSpawnPoint.y, maxSpawnPoint.y), planet.transform.position.z);
                i++;
            }
            if (i < 1000)
            {
                return true;
            }
            else
            {
                Debug.LogError("Couldn't find free place");
                return false;
            }

        }

        private bool SpawnPointBruteCheck(Vector3 spawnPoint, PlanetController newPlanet)
        {
            foreach (PlanetController planet in createdPlanets.Except(new PlanetController[] { newPlanet }))
            {
                float left = (spawnPoint.x - planet.transform.position.x) * (spawnPoint.x - planet.transform.position.x) +
                    (spawnPoint.y - planet.transform.position.y) * (spawnPoint.y - planet.transform.position.y);
                float right = 4 * (planet.GetBounds.extents.x * newPlanet.GetBounds.extents.x) * (planet.GetBounds.extents.y * newPlanet.GetBounds.extents.y);
                if (left <= right) return false;

            }
            return true;
        }

        public IEnumerable<PlanetController> GetCreatedPlanets()
        {
            return createdPlanets;
        }
        public PlanetController GetRandomPlanet() => createdPlanets[Random.Range(0, createdPlanets.Count)];


    }
}