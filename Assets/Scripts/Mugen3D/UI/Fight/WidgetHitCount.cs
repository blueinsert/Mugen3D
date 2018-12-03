using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mugen3D
{
    public class WidgetHitCount : UIView
    {
        public Text text;
        private Core.Character m_owner;
        private float m_refreshTime;

        private void Awake()
        {
            text = this.transform.Find("Pos/Text").GetComponent<Text>();
            this.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (m_owner == null)
                return;
            m_refreshTime -= Time.deltaTime;
            if(m_refreshTime<=0 && this.gameObject.activeInHierarchy)
            {
                this.gameObject.SetActive(false);
            }
        }

        public void SetInfo(Core.Character owner)
        {
            this.m_owner = owner;
            m_owner.onEvent += ProcessEvt;
        }

        void ProcessEvt(Core.Event evt)
        {
            if(evt.type == Core.EventType.HitCountChange)
            {
                UpdateHitCount((int)evt.data);
            }
        }

        private void UpdateHitCount(int hitCount)
        {
            text.text = hitCount + "连";
            this.gameObject.SetActive(true);
            this.m_refreshTime = 2;
        }

    }
}