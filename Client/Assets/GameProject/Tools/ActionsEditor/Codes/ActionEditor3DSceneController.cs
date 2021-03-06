﻿using bluebean.UGFramework;
using Mugen3D.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace bluebean.Mugen3D.UI
{
    public class ActionEditor3DSceneController : MonoViewController
    {
        [AutoBind("./Camera")]
        public Camera m_camera;
        [AutoBind("./CharacterRoot")]
        public GameObject m_characterRoot;

        private Vector2 m_dragStartPos;

        private AnimationController m_animController;

        public AnimationController AnimController {
            get {
                return m_animController;
            }
        }

        public void SetCharacter(GameObject prefab) {
            GameObjectUtility.DestroyChildren(m_characterRoot);
            GameObject go = GameObject.Instantiate(prefab, m_characterRoot.transform, false) as GameObject;
            m_animController = go.AddComponent<AnimationController>();
            m_animController.Init();
            go.transform.position = Vector3.zero;
        }

        public void SampleCharacterAnim(string animName, float normalizedTime) {
            m_animController.Sample(animName, normalizedTime);
        }

        public void Update()
        {
            //缩放摄像机
            //zoom out
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                m_camera.orthographicSize += 0.1f;
            }
            //Zoom in
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                m_camera.orthographicSize -= 0.1f;
                if (m_camera.orthographicSize <= 0.2f)
                {
                    m_camera.orthographicSize = 0.2f;
                }
            }
            if (Input.GetKeyDown(KeyCode.KeypadPeriod)) {
                m_camera.transform.position = new Vector3(-0.27f, 1.09f, -2);
                m_camera.orthographicSize = 1.3f;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            m_dragStartPos = eventData.position;
        }

        /// <summary>
        /// 平移摄像机
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            var pos = eventData.position;
            var deltaPos = pos - m_dragStartPos;
            deltaPos = -deltaPos * m_camera.orthographicSize * 0.01f;
            m_dragStartPos = pos;
            m_camera.transform.position = m_camera.transform.position + new Vector3(deltaPos.x, deltaPos.y, 0);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }
    }
}
