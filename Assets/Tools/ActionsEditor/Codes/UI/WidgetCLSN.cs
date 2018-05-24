using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mugen3D.Tools
{
    [RequireComponent(typeof(UnityEngine.UI.Toggle))]
    public class WidgetCLSN : MonoBehaviour
    {
        public Toggle toggle;
        public Clsn m_clsn;
       
        private bool isInited = false;

        // Use this for initialization
        void Start()
        {

        }

        public void Init(Clsn clsn)
        {
            this.m_clsn = clsn;
            this.isInited = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isInited)
                return;
            var leftDown = new Vector3(this.m_clsn.x1, this.m_clsn.y1, 0);
            var rightUp = new Vector3(this.m_clsn.x2, this.m_clsn.y2, 0);
            var center = (leftDown + rightUp) / 2;
            center = ActionsEditorController.Instance.ScenePosToUIPos(center);
            this.transform.position = center;

            float lenRadio = ActionsEditorController.Instance.GetUISceneLenRadio();
            this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(lenRadio * (m_clsn.x2 - m_clsn.x1), lenRadio * (m_clsn.y2 - m_clsn.y1));
        }

    }
}
