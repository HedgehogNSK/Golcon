using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Golcon
{
    public class PlanetSettingsBuilder 
    {
        PlanetSettings settings;

        public PlanetSettingsBuilder()
        {
            settings = new PlanetSettings();
        }

        public PlanetSettingsBuilder SetTxtColor(Color color)
        {
            settings.TxtColor = color;
            return this;
        }

        public PlanetSettingsBuilder SetPlanetColor(Color color)
        {
            settings._Color = color;
            return this;

        }

        public PlanetSettingsBuilder SetDiameter(float diameter)
        {
            settings.Scale = diameter;
            return this;

        }

        public PlanetSettingsBuilder SetEscadrille(int shipsAmount)
        {
            settings.ShipsAmount = shipsAmount;
            return this;

        }

        public PlanetSettingsBuilder SetShipsProductionRate(float rate)
        {
            settings.ShipsProductionRate = rate;
            return this;
        }

        public PlanetSettingsBuilder SetOwner(int id)
        {
            settings.OwnerID = id;
            return this;
        }

        public static implicit operator PlanetSettings(PlanetSettingsBuilder builder)
        {
            return builder.settings;
        }
    }
}