using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mugen3D;

public class FightUI : MonoBehaviour {
    public WidgetLifeBar lifeBar;
    public Animator animator;
    public Transform groupIngame;
    public Transform groupBase;
    public Transform groupPopup;
    public Transform groupAdd;

    public List<UIView> views;

    private bool m_isConsumingView = false;
    private List<UIView> m_viewsToShow = new List<UIView>();
    private List<Action<UIView>> m_onReadyToShow = new List<Action<UIView>>();

    public void Init(Character p1, Character p2)
    {
        lifeBar.Init(p1, p2);
    }

    public T CreateView<T>() where T : UIView
    {
        T result;
        foreach (var view in views)
        {
            if (view is T)
            {
                var go = GameObject.Instantiate(view.gameObject, groupPopup);
                go.SetActive(false);
                result = go.GetComponent<T>();
                return result;
            }
        }
        return null;
    }

    public FightUI InsertView<T>(System.Action<UIView> onReady) where T : UIView
    {
        var view = CreateView<T>();
        m_viewsToShow.Add(view);
        m_onReadyToShow.Add(onReady);
        if (m_isConsumingView == false)
        {
            DoNexView();
        }
        return this;
    }

    private void DoNexView()
    {
        if (m_viewsToShow.Count == 0)
        {
            m_isConsumingView = false;
            return;
        }
        m_isConsumingView = true;
        var curView = m_viewsToShow[0];
        curView.onDestroy += () => {
            m_viewsToShow.RemoveAt(0);
            m_onReadyToShow.RemoveAt(0);
            DoNexView();
        };
        m_onReadyToShow[0](curView);
    }

    public void FadeIn(System.Action cb)
    {
        UIUtils.PlayAnimation(animator, "FightUI_FadeIn", (animName) => {
            if (cb != null)
                cb();
        });
    }

    public void FadeOut(System.Action cb)
    {
        UIUtils.PlayAnimation(animator, "FightUI_FadeOut", (animName) =>
        {
            if (cb != null)
                cb();
        });
    }

}
