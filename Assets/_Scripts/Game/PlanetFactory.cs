using System.Collections;
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

        List<PlanetController> existingPlanets = new List<PlanetController>();
        public void Initialize()
        {
            existingPlanets.Clear();
        }
        public PlanetController CreatePlanet()
        {
            PlanetController planet = Instantiate(_planetPrefab);
            existingPlanets.Add(planet);
            return planet;
        }

        private bool DeletePlanet(PlanetController planet)
        {
            existingPlanets.Remove(planet);
            Destroy(planet.gameObject);
            return true;
        }
        
        public PlanetController CreateRandomPlanet(Transform parent, Vector3 minPoint, Vector3 maxPoint)
        {
            float scale = Random.Range(planetScaleRange.x, planetScaleRange.y);
            PlanetController planet  = CreatePlanet();
            planet.transform.localScale = Vector3.one * scale;

            Vector3 point;
            if (!TryGetSpawnPoint(minPoint, maxPoint, planet, out point))
            {
                DeletePlanet(planet);
                return null;
            }

            planet.transform.position = point;
            planet.transform.parent = parent;
            
            
            
            return planet;
        }

        private bool TryGetSpawnPoint(Vector3 minPoint, Vector3 maxPoint, PlanetController planet, out Vector3 point)
        {
            Bounds planetBounds = planet.GetBounds;
            minPoint += planetBounds.extents;
            maxPoint -= planetBounds.extents;            

            point = new Vector3(Random.Range(minPoint.x, maxPoint.x), Random.Range(minPoint.y, maxPoint.y), planet.transform.position.z);
            int i = 0;
            while (!SpawnPointBruteCheck(point, planet) && i<1000)
            {
                point = new Vector3(Random.Range(minPoint.x, maxPoint.x), Random.Range(minPoint.y, maxPoint.y), planet.transform.position.z);
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
            foreach (PlanetController planet in existingPlanets.Except(new PlanetController[] { newPlanet }))
            {
                float left = (spawnPoint.x - planet.GetBounds.center.x) * (spawnPoint.x - planet.GetBounds.center.x) +
                    (spawnPoint.y - planet.GetBounds.center.y) * (spawnPoint.y - planet.GetBounds.center.y);
                float right = 4 * (planet.GetBounds.extents.x * newPlanet.GetBounds.extents.x) * (planet.GetBounds.extents.y * newPlanet.GetBounds.extents.y);
                if (left <= right) return false;

            }
            return true;
        }

    }
}