using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class CameraController
    {
        private Camera m_camera;
        private Transform m_target1;
        private Transform m_target2;
        public Rect viewportRect;
        public float yOffset = 1f;
        public float dumpRatio = 10;
        

        public CameraController(Camera cam, Transform t1, Transform t2)
        {
            m_camera = cam;
            m_target1 = t1;
            m_target2 = t2;
            viewportRect = new Rect(Vector2.zero, 1, 1);
        }

        public void Update()
        {
            if (m_target1 == null)
                return;
            Vector3 newPos = new Vector3((m_target1.position.x + m_target2.position.x)/2, m_target1.position.y + yOffset, m_camera.transform.position.z);
            m_camera.transform.position = Vector3.Lerp(m_camera.transform.position, newPos, Time.deltaTime * dumpRatio);
            CalcViewportRect();
        }

        private void CalcViewportRect()
        {
            float fileOfView = m_camera.fieldOfView;
            float h = Mathf.Tan(fileOfView / 2 / 180 * Mathf.PI) * Mathf.Abs(m_camera.transform.position.z) * 2;
            float w = m_camera.aspect * h;
            viewportRect.position = new Vector2(m_camera.transform.position.x, m_camera.transform.position.y);
            viewportRect.width = w;
            viewportRect.height = h;
        }

    }
}
