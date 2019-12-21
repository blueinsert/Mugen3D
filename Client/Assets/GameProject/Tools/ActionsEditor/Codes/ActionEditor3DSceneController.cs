using bluebean.UGFramework;
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

        public void SetCharacter(GameObject prefab) {
            GameObjectUtility.DestroyChildren(m_characterRoot);
            GameObject go = GameObject.Instantiate(prefab, m_characterRoot.transform, false) as GameObject;
            go.AddComponent<AnimController>();
            go.transform.position = Vector3.zero;
        }

        public void Update()
        {
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
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            m_dragStartPos = eventData.position;
        }

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
