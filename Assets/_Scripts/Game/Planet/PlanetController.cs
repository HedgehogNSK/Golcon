using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hedge.Tools;
using System;

namespace Golcon
{
    public class PlanetController : MonoBehaviour
    {      
#pragma warning disable CS0649
        [SerializeField] ParticleSystem glowEffect;
        [SerializeField] ShipController shipPrefab;
        [SerializeField] int shipsSendingPerFrame = 3;
        [SerializeField] int maxShipsSending = 50;
#pragma warning restore CS0649
        
        public int OwnerID { get; private set; }
        public event Action<PlanetController, int> OwnerChanged;

        TextMesh txtMesh;
        SpriteRenderer planetImg;
        CircleCollider2D planetCollider;
        private int shipsAmount;
        public int ShipsAmount { get { return shipsAmount; }
            protected set {               
                shipsAmount = value;
                txtMesh.text =((float)shipsAmount).ToShortNumber();
            } }
        public float ShipsProductionRate { get; protected set; }
       

        Coroutine production;
        float cachedColliderSize;
        private void Awake()
        {
            planetImg = GetComponentInChildren<SpriteRenderer>();
            planetCollider = GetComponentInChildren<CircleCollider2D>();
            cachedColliderSize = planetCollider.radius;
            txtMesh = GetComponentInChildren<TextMesh>();
            
        }
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
            planetCollider.radius = cachedColliderSize* settings.Scale;

           ShipsAmount = settings.ShipsAmount;
            ShipsProductionRate = settings.ShipsProductionRate;
            
            txtMesh.color = settings.TxtColor;
            OwnerID = settings.OwnerID;

        }
        public PlanetSettings GetSettings()
        {
            PlanetSettings settings = new PlanetSettingsBuilder().SetDiameter(planetImg.transform.localScale.x)
                .SetEscadrille(ShipsAmount)
                .SetPlanetColor(planetImg.color)
                .SetTxtColor(txtMesh.color)
                .SetShipsProductionRate(ShipsProductionRate)
                .SetOwner(OwnerID);
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
            //Debug.Log(other.gameObject.name);
            int totalDamage = ShipsAmount / 2;
            ShipsAmount -= totalDamage;

            StartCoroutine(SendShips2Target(totalDamage, other));
            
        }

        private IEnumerator SendShips2Target(int totalDamage, PlanetController target)
        {

#if DEBUG
            int j = 0;
#endif
            int damagePerShip = 1;
             ShipController ship;
            
            if(totalDamage >maxShipsSending)
            {
                damagePerShip = totalDamage / maxShipsSending;

                int undividableDmg = totalDamage % maxShipsSending;

                if (undividableDmg != 0)
                {
                    ship = CreateShip(target, undividableDmg);
                }
            }

          
            for (int i = 0; i < (totalDamage> maxShipsSending ? maxShipsSending : totalDamage); i++)
            {
                ship = CreateShip(target, damagePerShip);
#if DEBUG
                j++;
                ship.gameObject.name = "Ship " + j;
#endif
                if (i % shipsSendingPerFrame == 0)
                    yield return new WaitForFixedUpdate();

            }


        }

        private ShipController CreateShip(PlanetController target, int damage)
        {
            Vector3 targetPos;
            Vector3 instantiatePoint;

            targetPos = target.transform.position;
            instantiatePoint = planetCollider.ClosestPoint(targetPos);

            ShipController newShip;
            newShip = Instantiate(shipPrefab);
            newShip.transform.position = instantiatePoint;
            newShip.Init(OwnerID, target, damage);

            return newShip;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            
            ShipController ship = collision.gameObject.GetComponent<ShipController>();
            if (ship.Target != this) return;
            if(ship.OwnerID != OwnerID)
            {
                ShipsAmount-=ship.Damage;
                if (ShipsAmount<=0)
                {
                    OwnerID = ship.OwnerID;
                    ShipsAmount = Mathf.Abs(ShipsAmount);
                    OwnerChanged?.Invoke(this, OwnerID);
                }
            }
            else
            {
                ShipsAmount+= ship.Damage;
            }
        }


    }
}