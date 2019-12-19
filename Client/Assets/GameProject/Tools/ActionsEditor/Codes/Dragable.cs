using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace bluebean.Mugen3D.UI
{
    public class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public System.Action<Vector3> onDrag;
        private Canvas m_canvas;
        public Canvas Canvas {
            get {
                if (m_canvas == null) {
                    m_canvas = GetComponentInParent<Canvas>();
                }
                return m_canvas;
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.transform.position = Canvas.worldCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 100));
            if (onDrag != null)
                onDrag(this.transform.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
        }
    }
}
