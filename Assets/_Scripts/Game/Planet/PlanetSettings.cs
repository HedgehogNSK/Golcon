using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Golcon
{
    public class PlanetSettings 
    {
       
        public Color _Color { get; set; }
        public Color TxtColor { get; set; }
        public float Scale { get; set; }
        public int ShipsAmount { get; set;}
        public float ShipsProductionRate { get; set; }
        public int OwnerID { get; set; }

    }
}