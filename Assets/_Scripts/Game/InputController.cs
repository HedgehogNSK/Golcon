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
        static public event System.Action<Rect> OnSelectArea;
        Camera cam;
        readonly Vector2 zero = new Vector2(0f, 0f);


        private void Start()
        {
            cam = Camera.main;
            dragRectangle.rectTransform.pivot = Vector2.up;
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
        

        public void OnBeginDrag(PointerEventData eventData)
        {
            dragRectangle.rectTransform.position = eventData.position;
            dragRectangle.gameObject.SetActive(true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 delta = eventData.position - (Vector2)dragRectangle.rectTransform.position;
            float xScale = Mathf.Abs(dragRectangle.rectTransform.localScale.x);
            float yScale = Mathf.Abs(dragRectangle.rectTransform.localScale.y);
            dragRectangle.rectTransform.localScale = new Vector2((delta.x < 0 ? -1 : 1)*xScale, (delta.y < 0 ? 1 : -1)*yScale);
            delta = new Vector2(Mathf.Abs(delta.x), Mathf.Abs(delta.y));
            dragRectangle.rectTransform.sizeDelta = delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Vector3 currentPosition = cam.ScreenToWorldPoint(eventData.position);
            Vector3 startPosition = cam.ScreenToWorldPoint(dragRectangle.rectTransform.position);
            Rect selectedArea = new Rect(startPosition,currentPosition-startPosition);
            dragRectangle.gameObject.SetActive(false);

            OnSelectArea?.Invoke(selectedArea);
        }
    }
}