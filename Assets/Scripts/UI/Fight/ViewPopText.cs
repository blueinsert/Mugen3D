using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewPopText : UIView {
    public Text text;

    public void Show(string content, System.Action onFinish = null)
    {
        text.text = content;
        if (onFinish != null)
        {
            this.onDestroy += onFinish;
        }
        this.onFadeInComplete = () => { this.Close(); };
        base.Show();
    }

}
