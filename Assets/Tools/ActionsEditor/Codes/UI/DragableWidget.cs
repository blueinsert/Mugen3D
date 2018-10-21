using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mugen3D.Tools
{
    public class DragableWidget : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public System.Action<Vector3> onDrag;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnDrag(PointerEventData eventData)
        {

            this.transform.position = ActionsEditor.Instance.view.uiCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 100));
            if (onDrag != null)
                onDrag(this.transform.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
        }
    }
}
