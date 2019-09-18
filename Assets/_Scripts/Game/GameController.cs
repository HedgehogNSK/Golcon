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
#pragma warning restore CS0649

        Camera cam;
        Vector3 maxPoint;
            Vector3 minPoint;
        private void Start()
        {
            cam = Camera.main;
            maxPoint = cam.ViewportToWorldPoint(cam.rect.max);
            minPoint = cam.ViewportToWorldPoint(cam.rect.min);
            Debug.Log(minPoint + " " + maxPoint);
            planetFactory.Initialize();
            planetFactory.CreateRandomPlanet(transform, minPoint,maxPoint);
        }

#if DEBUG
        private void Update()
        {
         if(Input.GetKeyDown(KeyCode.X))
            {
                planetFactory.CreateRandomPlanet(transform, minPoint, maxPoint);
            }
        }
#endif
    }
}
