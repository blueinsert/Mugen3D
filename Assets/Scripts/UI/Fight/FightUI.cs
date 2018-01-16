using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mugen3D;

public class FightUI : MonoBehaviour {
    public WidgetLifeBar lifeBar;
    public Animator animator;
    public Transform tranIngame;
    public Transform tranBase;
    public Transform tranPopup;
    public Transform tranAdd;
    public GameObject prefabRound;
    public GameObject prefabFight;
    public GameObject prefabKO;
    public GameObject prefabOvertime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       
	}

    public void Init(Player p1, Player p2)
    {
        lifeBar.Init(p1, p2);
    }

    public void PopupRound(int roundNum, Action cb)
    {  
        var goRound = GameObject.Instantiate(prefabRound, this.tranPopup);
        var popRound = goRound.GetComponent<PopupRound>();
        popRound.Show(roundNum, cb);
    }

    public void PopupFight(Action cb)
    {
        var goFight = GameObject.Instantiate(prefabFight, this.tranPopup);
        var popFight = goFight.GetComponent<PopupFight>();
        popFight.Show(cb);
    }

    public void PopupK0(Action cb)
    {
        var goKO = GameObject.Instantiate(prefabKO, this.tranPopup);
        var popKO = goKO.GetComponent<PopupKO>();
        popKO.Show(cb);
    }

    public void PopupOverTime(Action cb)
    {
        var goOvertime = GameObject.Instantiate(prefabOvertime, this.tranPopup);
        var popOvertime = goOvertime.GetComponent<PopupOvertime>();
        popOvertime.Show(cb);
    }

    public void FadeIn(Action cb)
    {
        UIUtils.PlayAnimation(animator, "FightUI_FadeIn", (animName) => {
            if (cb != null)
                cb();
        });
    }

    public void FadeOut(Action cb)
    {
        UIUtils.PlayAnimation(animator, "FightUI_FadeOut", (animName) =>
        {
            if (cb != null)
                cb();
        });
    }
}
