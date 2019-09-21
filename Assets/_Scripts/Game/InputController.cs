using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Golcon
{
    public class InputController : MonoBehaviour, IPointerClickHandler
    {
        static public event System.Action<PlanetController> OnClick;

        Camera cam;
        readonly Vector2 zero = new Vector2(0f, 0f);


        private void Start()
        {
            cam = Camera.main;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Vector3 currentPosition = cam.ScreenToWorldPoint(eventData.position);
            RaycastHit2D currentHit = Physics2D.Raycast(currentPosition, zero);

            Collider2D hittedCollider = currentHit.collider;
            if (hittedCollider != null)
            {
                PlanetController clickedPlanet = hittedCollider.GetComponentInParent<PlanetController>();
                OnClick?.Invoke(clickedPlanet);
            }
            else
                OnClick?.Invoke(null);
        }

    }
}