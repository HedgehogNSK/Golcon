using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Golcon
{
    public class GameController : MonoBehaviour
    {
        #region INSTANCE
        static GameController _instance;
        static public GameController Instance
        {
            get
            {
                if(!_instance)
                {
                    _instance = FindObjectOfType<GameController>();
                }                
                return _instance;

            }
        }
        #endregion
#pragma warning disable CS0649
        [SerializeField]PlanetFactory planetFactory;
        [SerializeField] Color playerColor;
        [SerializeField] Color neutralColor;
#pragma warning restore CS0649

        Camera cam;

        List<PlanetController> playerPlanets;
        private void Start()
        {
            cam = Camera.main;
            GenerateGame();
        }

#if DEBUG
        private void Update()
        {
         if(Input.GetKey(KeyCode.X))
            {
                planetFactory.CreateRandomPlanet(transform);
            }

         if(Input.GetKeyDown(KeyCode.N))
            {
                GenerateGame();
            }
        }
#endif

        private void GenerateMap()
        {
            planetFactory.Initialize(cam.ViewportToWorldPoint(cam.rect.min), cam.ViewportToWorldPoint(cam.rect.max));
            PlanetController planet = planetFactory.CreateRandomPlanet(transform);
            int i = 0;
            while (i < 100)
            {
                planet = planetFactory.CreateRandomPlanet(transform);
                i++;
            }
        }

        private void GenerateGame()
        {
            GenerateMap();
            PlanetController planet = planetFactory.GetRandomPlanet();
            PlanetSettings settings = planet.GetSettings();
            settings._Color = playerColor;
            settings.ShipsAmount = 50;
            
            planet.Setup(settings);
            playerPlanets = new List<PlanetController>();
            playerPlanets.Add (planet);

        }
    }
}
