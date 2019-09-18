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
#pragma warning restore CS0649

        public int ShipsAmount { get; protected set; }
        public int ShipsProductionRate { get; protected set; }

        public void Setup(PlanetSettings settings)
        {
            planetImg.color = settings._Color;

            planetImg.transform.localScale = Vector3.one * settings.Scale;

            ShipsAmount = settings.ShipsAmount;
            
            txtMesh.text = ShipsAmount > 0 ? ShipsAmount.ToString() : "";
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
        
    }
}