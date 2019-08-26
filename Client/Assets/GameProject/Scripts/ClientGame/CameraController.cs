using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixPointMath;
using bluebean.Mugen3D.Core;

namespace bluebean.Mugen3D.ClientGame
{
    public class CameraController
    {
        private Camera m_camera;
        private CameraComponent m_cameraComponent;

        public void Init(CameraComponent cameraComponent, Camera camera)
        {
            m_cameraComponent = cameraComponent;
            m_camera = camera;
        }

        public void Update()
        {
            m_camera.transform.position = new Vector3(m_cameraComponent.Position.x.AsFloat(), m_cameraComponent.Position.y.AsFloat(), m_cameraComponent.ZValue.AsFloat());
            m_camera.fieldOfView = m_cameraComponent.FieldOfView.AsFloat();
            m_camera.aspect = m_cameraComponent.Aspect.AsFloat();

            //m_camera.transform.rotation = Quaternion.EulerAngles(-logicCameraController.m_rotationX.AsFloat(), 0, 0);
            //var center = logicCameraController.targetCenter.ToVector3();
            //center.y += logicCameraController.config.Yoffset;
            //m_camera.transform.LookAt(center);
        }

    }
}
