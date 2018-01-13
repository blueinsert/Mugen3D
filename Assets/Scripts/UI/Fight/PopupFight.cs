using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupFight : MonoBehaviour {
    public Animator animator;
    private Action onFinish;

    public void Show(Action onFinish)
    {
        this.onFinish = onFinish;
        UIUtils.PlayAnimation(this.animator, "popInOut", (animName) => { this.Close(); });
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
