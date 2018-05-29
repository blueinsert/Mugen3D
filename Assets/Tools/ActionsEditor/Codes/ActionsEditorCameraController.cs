using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
namespace Mugen3D.Tools
{
    public class ActionsEditorCameraController : MonoBehaviour
    {
        public Camera camera;

        public void ZoomIn()
        {
            camera.orthographicSize -= 0.1f;
            if (camera.orthographicSize <= 0.2f)
            {
                camera.orthographicSize = 0.2f;
            }
        }

        public void ZoomOut()
        {
            camera.orthographicSize += 0.1f;
        }

        private Vector2 lastPointerPos;

        public void OnDrag(Vector2 pos)
        {
            var deltaPos = pos - lastPointerPos;
            deltaPos = -deltaPos * this.camera.orthographicSize * 0.01f;
            lastPointerPos = pos;
            this.transform.position = this.transform.position + new Vector3(deltaPos.x, deltaPos.y, 0);
        }

        public void BeginDrag(Vector2 pos)
        {
            lastPointerPos = pos;
        }
       
    }
}
