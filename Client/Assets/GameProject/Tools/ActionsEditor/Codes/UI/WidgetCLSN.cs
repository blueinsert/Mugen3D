using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using bluebean.Mugen3D.Core;

namespace Mugen3D.Tools
{
    [RequireComponent(typeof(UnityEngine.UI.Toggle))]
    public class WidgetCLSN : MonoBehaviour
    {
        public Toggle toggle;
        public Image img;
        public Clsn m_clsn;
        public DragableWidget leftDown;
        public DragableWidget rightUp;
       
        private bool isInited = false;

        // Use this for initialization
        void Start()
        {

        }

        public void Init(Clsn clsn)
        {
            this.m_clsn = clsn;
            if (clsn.type == 1)
            {
                this.img.color = Color.blue;
            }
            else if(clsn.type == 2)
            {
                this.img.color = Color.red;
            }else if(clsn.type == 3)
            {
                this.img.color = Color.black;
            }
            //var leftDown = new Vector3(this.m_clsn.x1, this.m_clsn.y1, 0);
            //var rightUp = new Vector3(this.m_clsn.x2, this.m_clsn.y2, 0);
            //this.leftDown.GetComponent<RectTransform>().position = ActionsEditorController.Instance.ScenePosToUIPos(leftDown);
           // this.rightUp.GetComponent<RectTransform>().position = ActionsEditorController.Instance.ScenePosToUIPos(rightUp);
            this.leftDown.onDrag += (uipos) =>{
                var leftDownPos = ActionsEditor.Instance.view.UIPosToScenePos(uipos);
                this.m_clsn.x1 = leftDownPos.x.ToNumber();
                this.m_clsn.y1 = leftDownPos.y.ToNumber();
            };

            this.rightUp.onDrag += (uipos) =>
            {
                var rightDownPos = ActionsEditor.Instance.view.UIPosToScenePos(uipos);
                this.m_clsn.x2 = rightDownPos.x.ToNumber();
                this.m_clsn.y2 = rightDownPos.y.ToNumber();
            };
            this.isInited = true;
        }


        // Update is called once per frame
        void Update()
        {
            if (!isInited)
                return;
            var leftDown = new Vector3(this.m_clsn.x1.AsFloat(), this.m_clsn.y1.AsFloat(), 0);
            var rightUp = new Vector3(this.m_clsn.x2.AsFloat(), this.m_clsn.y2.AsFloat(), 0);
            var center = (leftDown + rightUp) / 2;
            center = ActionsEditor.Instance.view.ScenePosToUIPos(center);
            this.transform.position = center;

            float lenRadio = ActionsEditor.Instance.view.GetUISceneLenRadio();
            Vector2 size = new Vector2(lenRadio * (m_clsn.x2.AsFloat() - m_clsn.x1.AsFloat()), lenRadio * (m_clsn.y2.AsFloat() - m_clsn.y1.AsFloat()));
            this.transform.GetComponent<RectTransform>().sizeDelta = size;
            size = size * 0.1f;
            if (size.x < 30)
            {
                size.x = 30;
            }
            if (size.y < 30)
            {
                size.y = 30;
            }
            this.leftDown.GetComponent<RectTransform>().sizeDelta = size;
            this.rightUp.GetComponent<RectTransform>().sizeDelta = size;
        }

    }
}
