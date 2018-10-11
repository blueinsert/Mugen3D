using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D.Core;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D
{
    [RequireComponent(typeof(Camera))]
    public class CameraView : MonoBehaviour
    {
        private CameraController m_camCtl;
        private Camera m_camera;
        private Core.Vector m_position;

        public void Init(CameraController camCtl)
        {
            this.m_camCtl = camCtl;
            m_camera = this.GetComponent<Camera>();
            m_camera.aspect = m_camCtl.config.aspect.AsFloat();
            m_camera.fieldOfView = m_camCtl.config.fieldOfView.AsFloat();
        }

        public void Update()
        {
            if (m_position != m_camCtl.position)
            {
                m_position = m_camCtl.position;
                m_camera.transform.position = new Vector3(m_position.x.AsFloat(), m_position.y.AsFloat(), m_position.z.AsFloat());
            }
            DebugDraw();
        }

        private void DebugDraw()
        {
            if (m_camCtl.viewportRect == null)
                return;
            Log.DrawRect(m_camCtl.viewportRect.LeftUp.ToVector2(), m_camCtl.viewportRect.LeftUp.ToVector2(), m_camCtl.viewportRect.LeftUp.ToVector2(), m_camCtl.viewportRect.LeftUp.ToVector2(), Color.black, UnityEngine.Time.deltaTime); 
        }
       
    }
}
