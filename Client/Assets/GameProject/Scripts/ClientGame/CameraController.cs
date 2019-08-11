﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixPointMath;

namespace bluebean.Mugen3D.ClientGame
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        private Camera m_camera;
        private Core.CameraController logicCameraController;

        public void Init(Core.CameraController logicCameraController)
        {
            m_camera = this.GetComponent<Camera>();
            this.logicCameraController = logicCameraController;
        }

        public void Update()
        {
            m_camera.transform.position = logicCameraController.position.ToVector3();
            m_camera.fieldOfView = logicCameraController.fieldOfView.AsFloat();
            m_camera.aspect = logicCameraController.aspect.AsFloat();
            m_camera.transform.rotation = Quaternion.EulerAngles(-logicCameraController.m_rotationX.AsFloat(), 0, 0);
            var center = logicCameraController.targetCenter.ToVector3();
            center.y += logicCameraController.config.Yoffset.AsFloat();
            m_camera.transform.LookAt(center);
        }

    }
}