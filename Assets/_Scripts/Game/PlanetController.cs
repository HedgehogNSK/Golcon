using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hedge.Tools;

namespace Golcon
{
    public class PlanetController : MonoBehaviour
    {      
#pragma warning disable CS0649
        [SerializeField] TextMesh txtMesh;
        [SerializeField] SpriteRenderer planetImg;
        [SerializeField] Collider2D planetCollider;
        [SerializeField] ParticleSystem glowEffect;
        [SerializeField] GameObject shipPrefab;
#pragma warning restore CS0649

        private int shipsAmount;
        public int ShipsAmount { get { return shipsAmount; }
            protected set {
                if(value<0)
                {
                    Debug.LogError("Amount of ships can't lesser than 0");
                    return;
                }
                shipsAmount = value;
                txtMesh.text =((float)ShipsAmount).ToShortNumber();
            } }
        public float ShipsProductionRate { get; protected set; }

        Coroutine production;
        public void Start()
        {
            SetMarked(false);
            production = StartCoroutine(EachSecondShipsProduction());
        }
        public void Setup(PlanetSettings settings)
        {
            planetImg.color = settings._Color;

            planetImg.transform.localScale = 
                glowEffect.transform.localScale = Vector3.one * settings.Scale;
             

            ShipsAmount = settings.ShipsAmount;
            ShipsProductionRate = settings.ShipsProductionRate;
            
            txtMesh.color = settings.TxtColor;

        }
        public PlanetSettings GetSettings()
        {
            PlanetSettings settings = new PlanetSettingsBuilder().SetDiameter(planetImg.transform.localScale.x)
                .SetEscadrille(ShipsAmount)
                .SetPlanetColor(planetImg.color)
                .SetTxtColor(txtMesh.color)
                .SetShipsProductionRate(ShipsProductionRate);
            return settings;
        }

        public Bounds GetBounds=> planetCollider.bounds;

        public bool IsSelected { get; private set; }
        public void SetMarked(bool on)
        {
            IsSelected = on;
            if (on)
            {
                glowEffect.Play();              
            }
            else
            {
                glowEffect.Stop();
            }
        }
               
        private IEnumerator EachSecondShipsProduction()
        {
            float currentProgress = 0;

            while (true)
            {
                yield return new WaitForSeconds(1);

                currentProgress += ShipsProductionRate;
                ShipsAmount += (int)currentProgress;
                currentProgress -= (int)currentProgress;
            }
        }
        
        public void Attack(PlanetController other)
        {
            int battleShips = ShipsAmount / 2;
            ShipsAmount -= battleShips;
            
        }
    }
}