using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D.Tools
{
    public class ActionsEditorController
    {
        private static ActionsEditorController m_instance;
        public static ActionsEditorController Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new ActionsEditorController();
                }
                return m_instance;
            }
        }
        public ActionsEditorModule module;
        public ActionsEditorView view;
        private ActionsEditorController() { }


        public Vector3 ScenePosToUIPos(Vector3 wPos)
        {
            Vector3 screenPos = m_instance.view.sceneCamera.WorldToScreenPoint(wPos);
            var canvas = m_instance.view.cancas;
            screenPos.z = canvas.planeDistance;
            var uiPos = canvas.worldCamera.ScreenToWorldPoint(screenPos);
            return uiPos;
        }

        public Vector3 UIPosToScenePos(Vector3 uiPos)
        {
            var canvas = m_instance.view.cancas;
            var screenPos = canvas.worldCamera.WorldToScreenPoint(uiPos);
            var wPos = m_instance.view.sceneCamera.ScreenToWorldPoint(screenPos);
            return wPos;
        }

        //return ui rectTransform size unit / world size unit
        public float GetUISceneLenRadio()
        {
            if (m_instance.view.sceneCamera.orthographic)
            {
                return 768.0f / (m_instance.view.sceneCamera.orthographicSize * 2);
            }
            else
            {
                return 1;
            }
        }
    }
}
