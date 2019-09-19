using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                if (!_instance)
                {
                    _instance = FindObjectOfType<GameController>();
                }
                return _instance;

            }
        }
        #endregion
#pragma warning disable CS0649
        [SerializeField] PlanetFactory planetFactory;
        [SerializeField] Color playerColor;
        [SerializeField] int playerBaseShips = 50;
        [SerializeField] float playerShipsProductionRate = 2.5f;
        [SerializeField] Color neutralColor;
#pragma warning restore CS0649

        Camera cam;

        List<PlanetController> playersPlanets;
        List<PlanetController> choosenPlanets;
        private void Start()
        {
            cam = Camera.main;
            InputController.OnClick += InteractWithPlanet;
            GenerateGame();

        }

        private void InteractWithPlanet(PlanetController clicked)
        {
            if (playersPlanets.Contains(clicked))
            {
                Debug.Log("This is players planet");
                clicked.SetMarked(true);
                
            }
            foreach (var planet in playersPlanets.Where(other => other != clicked))
            {
                planet.SetMarked(false);
            }

            foreach (var planet in playersPlanets.Where(planet => planet.IsSelected))
            {
                planet.Attack(clicked);
            }
        }

#if DEBUG
        private void Update()
        {
            if (Input.GetKey(KeyCode.X))
            {
                planetFactory.TryCreateRandomPlanet(transform);
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                GenerateGame();
            }
        }
#endif

        private void GenerateMap()
        {
            planetFactory.Initialize(cam.ViewportToWorldPoint(cam.rect.min), cam.ViewportToWorldPoint(cam.rect.max));
            PlanetController planet = planetFactory.TryCreateRandomPlanet(transform);
            int i = 0;
            while (i < 10)
            {
                planet = planetFactory.TryCreateRandomPlanet(transform);
                i++;
            }
        }

        private void GenerateGame()
        {
            GenerateMap();
            PlanetController planet = planetFactory.GetRandomPlanet();
            PlanetSettings settings = planet.GetSettings();
            settings._Color = playerColor;
            settings.ShipsAmount = playerBaseShips;
            settings.ShipsProductionRate = playerShipsProductionRate;

            planet.Setup(settings);

            playersPlanets = new List<PlanetController>();
            playersPlanets.Add(planet);
            
            choosenPlanets = new List<PlanetController>();  

        }
    }
}
