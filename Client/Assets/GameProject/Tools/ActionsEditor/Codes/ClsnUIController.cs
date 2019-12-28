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
    /// <summary>
    /// 判断框UIController
    /// </summary>
    public class ClsnUIController : UIViewController
    {
        public event Action<Clsn> EventOnClsnChanged;

        [AutoBind("./Background")]
        public Image m_bgImage;
        [AutoBind("./")]
        public Toggle m_toggle;
        [AutoBind("./LeftDownCorner")]
        public Dragable m_leftDownCorner;
        [AutoBind("./RightUpCorner")]
        public Dragable m_rightUpCorner;

        private Clsn m_clsn = new Clsn(0,0,0,1,1);

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
            this.m_bgImage.color = color;
            m_clsn = clsn;
            UpdateClsn();
        }

        private void UpdateClsn() {
            var task = UIManager.Instance.FindUITask<ActionsEditorUITask>();
            var leftDown = new Vector3(m_clsn.x1.AsFloat(), m_clsn.y1.AsFloat(), 0);
            var rightUp = new Vector3(m_clsn.x2.AsFloat(), m_clsn.y2.AsFloat(), 0);
            leftDown = task.ScenePosToUIPos(leftDown);
            rightUp = task.ScenePosToUIPos(rightUp);
            var center = (leftDown + rightUp) / 2;
           
            //设置控制角点
            m_leftDownCorner.GetComponent<RectTransform>().position = new Vector2(leftDown.x, leftDown.y);
            m_rightUpCorner.GetComponent<RectTransform>().position = new Vector2(rightUp.x, rightUp.y);
            
            //设置背景图片
            this.m_bgImage.transform.position = center;
            var localLeftDown = m_leftDownCorner.GetComponent<RectTransform>().localPosition;
            var localRightUp = m_rightUpCorner.GetComponent<RectTransform>().localPosition;
            this.m_bgImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Abs(localRightUp.x - localLeftDown.x), Mathf.Abs(localRightUp.y - localLeftDown.y));

            //设置控制角点
            var lengh = (localLeftDown - localRightUp).magnitude;
            Vector2 sizeCorner = new Vector2(lengh, lengh) * 0.05f;
            m_leftDownCorner.GetComponent<RectTransform>().sizeDelta = sizeCorner;
            m_rightUpCorner.GetComponent<RectTransform>().sizeDelta = sizeCorner;
            m_leftDownCorner.GetComponent<RectTransform>().localPosition = new Vector3(localLeftDown.x, localLeftDown.y,0) + new Vector3(sizeCorner.x, sizeCorner.y, 0) / 2;
            m_rightUpCorner.GetComponent<RectTransform>().localPosition = new Vector3(localRightUp.x, localRightUp.y,0) - new Vector3(sizeCorner.x, sizeCorner.y, 0) / 2;
        }

        private void Update()
        {
            //对于缩放操作，clsn要同步变化
            UpdateClsn();
        }

        private void OnLeftDownCornerDrag(Vector3 pos) {
            var task = UIManager.Instance.FindUITask<ActionsEditorUITask>();
            var p = task.UIPosToScenePos(pos);
            m_clsn.x1 = p.x.ToNumber();
            m_clsn.y1 = p.y.ToNumber();
            UpdateClsn();
            if (EventOnClsnChanged != null) {
                EventOnClsnChanged(m_clsn);
            }
        }

        private void OnRightUpCornerDrag(Vector3 pos)
        {
            var task = UIManager.Instance.FindUITask<ActionsEditorUITask>();
            var p = task.UIPosToScenePos(pos);
            m_clsn.x2 = p.x.ToNumber();
            m_clsn.y2 = p.y.ToNumber();
            UpdateClsn();
            if (EventOnClsnChanged != null)
            {
                EventOnClsnChanged(m_clsn);
            }
        }

    }
}
