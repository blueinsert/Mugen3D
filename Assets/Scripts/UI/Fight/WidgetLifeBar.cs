using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mugen3D;

public class WidgetLifeBar : MonoBehaviour {
    public Image p1LifeBar;
    public Image p2LifeBar;
    public Text labelLeftTime;
    private Character m_p1;
    private Character m_p2;

	void Start () {
        InvokeRepeating("SetLeftTime", 1, 1);
	}

    public void Init(Character p1, Character p2)
    {
        m_p1 = p1;
        m_p2 = p2;
    }

    private void SetLeftTime()
    {
        float leftTime = 0;// ClientGame.Instance.roundMgr.leftTime;
        leftTime = Mathf.Floor(leftTime);
        labelLeftTime.text = leftTime.ToString();
    }

    public void Update()
    {
        p1LifeBar.fillAmount = m_p1.GetHP() / (float) m_p1.GetMaxHP();
        p2LifeBar.fillAmount = m_p2.GetHP() / (float)m_p2.GetMaxHP();
    }

}
