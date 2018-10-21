using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D.Core;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        private Camera m_camera;
        private Character p1;
        private Character p2;
        private Core.CameraConfig config;

        public void Init(Core.CameraConfig config, Character p1, Character p2)
        {
            m_camera = this.GetComponent<Camera>();
            this.config = config;
            this.p1 = p1;
            this.p2 = p2;
        }

        public void Update()
        {
            if (p1 == null || p2 == null)
                return;
            Vector targetPos = new Vector((p1.position.x + p2.position.x) / 2, (p1.position.y + p2.position.y) / 2 + config.yOffset, config.depth);
            Vector3 pos = targetPos.ToVector3();
            Vector3 position = Vector3.Lerp(this.transform.position, pos, UnityEngine.Time.deltaTime * config.dumpRatio.AsFloat());
            this.transform.position = position;
            m_camera.fieldOfView = CalcFieldOfView();
        }

        private float CalcFieldOfView()
        {
            /*
            float w = Mathf.Abs((p2.position.x - p1.position.x).AsFloat()) + 1.4f;
            float h = w / m_camera.aspect;
            float filedOfView = Mathf.Atan(h / 2 / Mathf.Abs(config.depth.AsFloat())) / Mathf.PI * 180 * 2;
            return filedOfView;
             */
            var dist = p1.position - p2.position;
            float radio = Mathf.Abs(dist.x.AsFloat()) / config.maxPlayerDist.AsFloat();
            float filedOfView = Mathf.Lerp(config.minFiledOfView.AsFloat(), config.maxFiledOfView.AsFloat(), radio);
            return filedOfView;
        }

        /*
        private void CalcViewportRect()
        {
            Number h = Math.Tan(config.fieldOfView / 2 / 180 * Math.Pi) * Math.Abs(config.depth) * 2;
            Number w = config.depth * h;
            viewportRect.position = new Vector(position.x, position.y, 0);
            viewportRect.width = w;
            viewportRect.height = h;
        }      
        */
       
    }
}
