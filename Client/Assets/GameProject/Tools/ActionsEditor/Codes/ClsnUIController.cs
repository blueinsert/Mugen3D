using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using bluebean.Mugen3D.Core;
using bluebean.UGFramework.UI;
using bluebean.UGFramework;

namespace bluebean.Mugen3D.UI
{
    public class ClsnUIController : UIViewController
    {
        public event Action<Vector3, Vector3> EventOnClsnChanged;

        [AutoBind("./Background")]
        public Image m_bgImage;
        [AutoBind("./")]
        public Toggle m_toggle;
        [AutoBind("./LeftDownCorner")]
        public Dragable m_leftDownCorner;
        [AutoBind("./RightUpCorner")]
        public Dragable m_rightUpCorner;

        protected override void OnBindFieldsComplete()
        {
            base.OnBindFieldsComplete();
            m_leftDownCorner.onDrag += OnLeftDownCornerDrag;
            m_rightUpCorner.onDrag += OnRightUpCornerDrag;
        }

        public void SetClsn(Clsn clsn) {
            Color color = Color.black;
            switch (clsn.type) {
                case 1:
                    color = Color.blue;
                    break;
                case 2:
                    color = Color.red;
                    break;
                case 3:
                    color = Color.black;
                    break;
            }
            this.m_bgImage.color = Color.black;

        }

        private void UpdateTransform() {
            UGFramework.Log.Debug.Log("ClsnControler.UpdateTransform");
            this.m_bgImage.transform.position = (m_leftDownCorner.transform.position + m_rightUpCorner.transform.position) / 2;
            this.m_bgImage.GetComponent<RectTransform>().sizeDelta = (m_rightUpCorner.GetComponent<RectTransform>().anchoredPosition - m_leftDownCorner.GetComponent<RectTransform>().anchoredPosition);
            m_leftDownCorner.GetComponent<RectTransform>().sizeDelta = new Vector2(this.m_bgImage.GetComponent<RectTransform>().sizeDelta.x / 10.0f, this.m_bgImage.GetComponent<RectTransform>().sizeDelta.y / 10.0f);
            m_rightUpCorner.GetComponent<RectTransform>().sizeDelta = new Vector2(this.m_bgImage.GetComponent<RectTransform>().sizeDelta.x / 10.0f, this.m_bgImage.GetComponent<RectTransform>().sizeDelta.y / 10.0f);
        }

        private void OnLeftDownCornerDrag(Vector3 pos) {
            UpdateTransform();
            if (EventOnClsnChanged != null) {
                EventOnClsnChanged(m_leftDownCorner.transform.position, m_rightUpCorner.transform.position);
            }
        }

        private void OnRightUpCornerDrag(Vector3 pos)
        {
            UpdateTransform();
            if (EventOnClsnChanged != null)
            {
                EventOnClsnChanged(m_leftDownCorner.transform.position, m_rightUpCorner.transform.position);
            }
        }


        /*
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
        */
    }
}
