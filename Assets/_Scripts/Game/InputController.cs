using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Golcon
{
    public class InputController : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] Image dragRectangle;

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

        Vector2 startPosition;
        public void OnBeginDrag(PointerEventData eventData)
        {
            startPosition = eventData.position;
            dragRectangle.rectTransform.position = startPosition;
            dragRectangle.gameObject.SetActive(true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            float x = eventData.position.x - startPosition.x;
            float y = startPosition.y - eventData.position.y;
            dragRectangle.rectTransform.sizeDelta = new Vector2(Mathf.Abs(x),Mathf.Abs(y) );
            dragRectangle.rectTransform.localScale = new Vector2(x < 0 ? -1 : 1, y < 0 ? -1 : 1);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            dragRectangle.gameObject.SetActive(false);
        }
    }
}