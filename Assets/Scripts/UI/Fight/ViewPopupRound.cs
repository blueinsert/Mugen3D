using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewPopupRound : UIView {
    public Text labelRoundNo; 

    public void Show(int roundNo, Action onFinish)
    {
        base.onDestroy = onFinish;
        this.labelRoundNo.text = roundNo.ToString();
        base.onFadeInComplete = () => { this.Close(); };
        base.Show();
    }

}
