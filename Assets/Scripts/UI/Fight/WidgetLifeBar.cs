using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mugen3D;

public class WidgetLifeBar : MonoBehaviour {
    public Image p1LifeBar;
    public Image p2LifeBar;
    public Text labelLeftTime;
    private int p1MaxHp;
    private int p2MaxHp;

	// Use this for initialization
	void Start () {
        InvokeRepeating("SetLeftTime", 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(Player p1, Player p2)
    {
        p1.onHpChange += OnP1HpChange;
        p2.onHpChange += OnP2HpChange;
        p1MaxHp = p1.MaxHP;
        p2MaxHp = p2.MaxHP;
    }

    private void SetLeftTime()
    {
        float leftTime = ClientGame.Instance.roundMgr.leftTime;
        leftTime = Mathf.Floor(leftTime);
        labelLeftTime.text = leftTime.ToString();
    }

    private void OnP1HpChange(int newHp)
    {
        p1LifeBar.fillAmount = newHp / (float)p1MaxHp;
    }

    private void OnP2HpChange(int newHp)
    {
        p2LifeBar.fillAmount = newHp / (float)p2MaxHp;
    }
}
