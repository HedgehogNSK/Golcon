using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Golcon
{

    public class ShipController : MonoBehaviour
    {
        Collider2D shipCollider;
        SpriteRenderer sprite;
        Rigidbody2D rigid;

        private PlanetController target;
        public PlanetController Target => target;
        int owner_id;
        public int OwnerID => owner_id;
        [SerializeField] float speed = 5;


        bool initialized = false;
        private void Awake()
        {
            shipCollider = GetComponentInChildren<Collider2D>();
            sprite = GetComponentInChildren<SpriteRenderer>();
            rigid = GetComponent<Rigidbody2D>();
        }
        // Start is called before the first frame update
        void Start()
        {

        }
        public void Init(int owner_id, PlanetController target)
        {
            this.owner_id = owner_id;
            this.target = target;
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
            float raycastDist = shipCollider.bounds.size.y * 3;
            shipCollider.Raycast(newVel, raycastHits, raycastDist);

            if (raycastHits[0].collider !=null)
            {
                PlanetController planet;
                planet = raycastHits[0].collider.GetComponent<PlanetController>();

                float deltaAngle = 5;
                float angle = deltaAngle;
                Vector2 tmpVel = newVel;

                //loop turns raycasts around the ship to find free way to fly around unwanted planet
                for (int i = 0; angle <= 90 && raycastHits[0].collider != null && planet != null && planet != Target; i++)
                {

                    if (i % 2 == 0)
                    {
                        angle += Mathf.Sign(angle)*deltaAngle;
                    }
                    else
                    {
                        angle = -angle;
                    }
                    tmpVel = Rotate(newVel, angle);
                    raycastHits = new RaycastHit2D[1];
                    shipCollider.Raycast(tmpVel, raycastHits, raycastDist);
                    Debug.DrawRay(transform.position, tmpVel);
                    if (raycastHits[0].collider != null)
                        planet = raycastHits[0].collider.GetComponent<PlanetController>();

                }
                newVel = tmpVel;

            }
            rigid.velocity = newVel;

            cachedPosition = transform.position;

        }

        private Vector2 Rotate(Vector2 vector, float angle)
        {
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
            float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
            return new Vector2(cos * vector.x - sin * vector.y, sin * vector.x + cos * vector.y);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            PlanetController planetOnTheWay = collision.gameObject.GetComponentInParent<PlanetController>();
            if (planetOnTheWay != null && planetOnTheWay == target)
            {
                Explode();
            }
        }


        private void Explode()
        {

            Destroy(gameObject);
        }
    }
}