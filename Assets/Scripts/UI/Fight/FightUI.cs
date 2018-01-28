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

    public List<UIView> views;

    public void Init(Player p1, Player p2)
    {
        lifeBar.Init(p1, p2);
    }

    public T ShowView<T>(Transform parent) where T : UIView
    {
        T result;
        foreach (var view in views)
        {
            if (view is T)
            {
                var go = GameObject.Instantiate(view.gameObject, parent);
                go.SetActive(false);
                result = go.GetComponent<T>();
                return result;
            }
        }
        return null;
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
