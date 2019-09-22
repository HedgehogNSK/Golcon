using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hedge.Tools;

namespace Golcon
{

    public class ShipController : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] float speed = 5;
        [Range(0.01f,1f)]
        [SerializeField] float speedDampingOnObstacle = 0.7f;
        [SerializeField] LayerMask planetLayer;
#pragma warning restore CS0649

        Collider2D shipCollider;
        SpriteRenderer sprite;
        Rigidbody2D rigid;
        public PlanetController Target { get; private set; }        
        public int OwnerID { get; private set; }      
        public int Damage { get; private set; }

        bool initialized = false;
        private void Awake()
        {
            shipCollider = GetComponent<Collider2D>();
            sprite = GetComponentInChildren<SpriteRenderer>();
            rigid = GetComponent<Rigidbody2D>();
        }
        // Start is called before the first frame update
        void Start()
        {

        }
        public void Init(int owner_id, PlanetController target, int damage)
        {
            OwnerID = owner_id;
            Target = target;
            Damage = damage;
            initialized = true;
        }

        private void FixedUpdate()
        {
            if (!initialized) return;

            Move();
            Rotate();
        }

        private void Rotate()
        {
            rigid.SetRotation(Quaternion.LookRotation(rigid.velocity, Vector3.back));
        }

        Vector3 cachedPosition = new Vector3(float.MaxValue, float.MaxValue, 0);
        private void Move()
        {
            if (Target == null)
            {
                Explode();
                return;
            }           
           
            Vector3 targetPosition = Target.transform.position;
            Vector2 newVel = (targetPosition - transform.position).normalized * speed;
                        
            RaycastHit2D[] raycastHits = new RaycastHit2D[1];
            float raycastDist = shipCollider.bounds.size.y * 5;
            shipCollider.Raycast(newVel, raycastHits, raycastDist, planetLayer);

            if (raycastHits[0].collider !=null)
            {
                PlanetController planet;
                planet = raycastHits[0].collider.GetComponent<PlanetController>();

                if (planet != Target)
                {
                    float deltaAngle = 5;
                    float angle = deltaAngle;
                    Vector2 tmpVel = newVel;

                    //loop turns raycasts around the ship to find free way to fly around unwanted planet
                    for (int i = 0; angle <= 90 && raycastHits[0].collider != null && planet != null && planet != Target; i++)
                    {

                        if (i % 2 == 0)
                        {
                            angle += Mathf.Sign(angle) * deltaAngle;
                        }
                        else
                        {
                            angle = -angle;
                        }
                        tmpVel = newVel.Rotate(angle);
                        raycastHits = new RaycastHit2D[1];
                        shipCollider.Raycast(tmpVel, raycastHits, raycastDist, planetLayer);
                        if (raycastHits[0].collider != null)
                            planet = raycastHits[0].collider.GetComponent<PlanetController>();

                    }
                    newVel = tmpVel * speedDampingOnObstacle;
                }
            }
            rigid.velocity = newVel;

            cachedPosition = transform.position;

        }       

        private void OnCollisionEnter2D(Collision2D collision)
        {
            PlanetController planetOnTheWay = collision.gameObject.GetComponentInParent<PlanetController>();
            if (planetOnTheWay != null && planetOnTheWay == Target)
            {
                Explode();
            }
        }


        private void Explode()
        {

            Destroy(gameObject);
        }

        public Bounds GetBounds => shipCollider.bounds;
    }
}