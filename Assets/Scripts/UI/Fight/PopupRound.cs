using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupRound : MonoBehaviour {
    public Animator animator;
    public Text labelRoundNo; 
    private Action onFinish;

    public void Show(int roundNo, Action onFinish)
    {
        this.onFinish = onFinish;
        this.labelRoundNo.text = roundNo.ToString();
        UIUtils.PlayAnimation(this.animator, "PopRound", (animName) => { this.Close(); });
    }

    private void Close()
    {
        if (this.onFinish != null)
        {
            this.onFinish();
        }
        GameObject.Destroy(this.gameObject);
    }
}
