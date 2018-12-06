using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mugen3D
{
    public class WidgetLifeBar : UIView
    {
        public Slider sliderP1Life;
        public Slider sliderP2Life;
        public Text textLeftTime;

        private Core.Character m_p1;
        private Core.Character m_p2;

        private void Awake()
        {
            sliderP1Life = this.transform.Find("Pos/HpBarP1").GetComponent<Slider>();
            sliderP2Life = this.transform.Find("Pos/HpBarP2").GetComponent<Slider>();
            textLeftTime = this.transform.Find("Pos/LeftTime").GetComponent<Text>();
        }

        public void SetInfo(Core.Character p1, Core.Character p2)
        {
            m_p1 = p1;
            m_p2 = p2;
        }

        private void Update()
        {
            if (m_p1 != null)
            {
                sliderP1Life.value = m_p1.GetHP() / (float)m_p1.GetMaxHP();
            }
            if (m_p2 != null)
            {
                sliderP2Life.value = m_p2.GetHP() / (float)m_p2.GetMaxHP();
            }     
        }

    }
}
