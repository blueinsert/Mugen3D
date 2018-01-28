using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupFight : UIView {
   
    public void Show(Action onFinish)
    {
        this.onDestroy = onFinish;
        this.onFadeInComplete = () => { this.Close(); };
        base.Show();
    }

}
