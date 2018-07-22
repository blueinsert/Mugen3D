using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D.Core;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D
{
    [RequireComponent(typeof(Camera))]
    public class CameraView : EntityView
    {
        private CameraController m_camCtl;
        private Camera m_camera;
        private Vector m_position;
        private Number m_fieldOfView;
        private Number m_depth;
        private Number m_aspect;

        void Awake()
        {
            m_camera = this.GetComponent<Camera>();
        }

        public void Init(CameraController camCtl)
        {
            this.m_camCtl = camCtl;
        }

        public void Update()
        {
            if (m_depth != m_camCtl.depth) {
                m_depth = m_camCtl.depth;
            }
            if (m_position != m_camCtl.position)
            {
                m_position = m_camCtl.position;
                m_camera.transform.position = new Vector3(m_position.x.AsFloat(), m_position.y.AsFloat(), m_depth.AsFloat());
            }
            if (m_fieldOfView != m_camCtl.fieldOfView)
            {
                m_fieldOfView = m_camCtl.fieldOfView;
                m_camera.fieldOfView = m_fieldOfView.AsFloat();
            }
            if (m_aspect != m_camCtl.aspect)
            {
                m_aspect = m_camCtl.aspect;
                m_camera.aspect = m_aspect.AsFloat();
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
