using System;
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
            InputController.OnSelectArea += SelectPlanets;
            GenerateGame();

        }

        private void SelectPlanets(Rect selectArea)
        {
            Debug.Log(selectArea);
            foreach(var planet in playersPlanets)
            {
              if(selectArea.Contains(planet.transform.position,true))
                {
                    planet.SetMarked(true);
                }
                else
                {
                    planet.SetMarked(false);
                }
            }
        }

        private void InteractWithPlanet(PlanetController clicked)
        {
            if (clicked != null)
            {
                if (playersPlanets.Contains(clicked))
                {
                    clicked.SetMarked(true);
                }
                else
                {
                    foreach (var planet in playersPlanets.Where(planet => planet.IsSelected))
                    {
                        planet.Attack(clicked);
                    }
                }
            }
            foreach (var planet in playersPlanets.Where(other => other != clicked))
            {
                planet.SetMarked(false);
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
            PlanetController planet;
            
            int i = 0;
            while (i < 10)
            {
                planet = planetFactory.TryCreateRandomPlanet(transform);
                if (planet != null)
                    planet.OwnerChanged += OwnerOfPlanetChanged;
                i++;
            }
        }

        private void OwnerOfPlanetChanged(PlanetController planet, int owner_id)
        {
            if(playersPlanets[0].OwnerID == owner_id)
            {
                PlayerOwnedPlanet(planet);
            }
            else
            {
                if(playersPlanets.Contains(planet))
                {
                    playersPlanets.Remove(planet);
                }
            }

            
        }

        private void GenerateGame()
        {
            GenerateMap();
            PlanetController rndPlanet = planetFactory.GetRandomPlanet();
            playersPlanets = new List<PlanetController>();
            PlayerOwnedPlanet(rndPlanet);

            PlanetSettings settings = rndPlanet.GetSettings();
            settings.ShipsAmount = playerBaseShips;
            rndPlanet.Setup(settings);

        }

        private void PlayerOwnedPlanet(PlanetController planet)
        {
            PlanetSettings settings = planet.GetSettings();
            settings._Color = playerColor;
            settings.ShipsProductionRate = playerShipsProductionRate;
            settings.OwnerID = 1;
            planet.Setup(settings);
            
            playersPlanets.Add(planet);
        }
    }
}
