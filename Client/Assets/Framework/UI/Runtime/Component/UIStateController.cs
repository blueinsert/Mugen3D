using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace bluebean.UGFramework
{
    [Serializable]
    public class UIColorDesc
    {
        public GameObject m_gameObject;
        public Color m_color;
    }

    [Serializable]
    public class UIStateDesc
    {
        public string m_stateName;
        public List<GameObject> m_activeObjects = new List<GameObject>();
        public List<UIColorDesc> m_uiColorDescs = new List<UIColorDesc>();
        public List<TweenMain> m_tweeners = new List<TweenMain>();
    }

    /// <summary>
    /// UI组件状态切换控制器，一个状态包括：要显示的物体，设置物体的颜色，播发UITweener
    /// </summary>
    public class UIStateController : MonoBehaviour
    {
        public List<UIStateDesc> m_uiStateDescs = new List<UIStateDesc>();

        private string m_curState;
        public string CurState
        {
            get { return m_curState; }
        }

        private bool m_hasCollect = false;

        private List<GameObject> m_gameObjectList = new List<GameObject>();

        public void CollectResources()
        {
            m_gameObjectList.Clear();
            foreach(var uiStateDesc in m_uiStateDescs)
            {
                foreach(var gameObject in uiStateDesc.m_activeObjects)
                {
                    m_gameObjectList.Add(gameObject);
                }
            }
            m_hasCollect = true;
        }

        private UIStateDesc GetUIStateDesc(string stateName)
        {
            foreach(var uiStateDesc in m_uiStateDescs)
            {
                if(uiStateDesc.m_stateName == stateName)
                {
                    return uiStateDesc;
                }
            }
            return null;
        }

        private void OnUIStateFinish()
        {

        }

        //todo alpha设置与硬切在起始或结束时刻上的不平滑
        public void SetUIState(string stateName, System.Action onEnd = null, bool replayTweeners = true)
        {
            if (!m_hasCollect)
            {
                CollectResources();
            }
            var uiStateDesc = GetUIStateDesc(stateName);
            if(uiStateDesc == null)
            {
                Debug.LogError(string.Format("the UIStateController in {0} don't has state {1}", name, stateName));
            }
            //隐藏所有物体
            foreach(var go in m_gameObjectList)
            {
                go.SetActive(false);
            }
            //显示当前state的物体
            foreach(var go in uiStateDesc.m_activeObjects)
            {
                go.SetActive(true);
            }
            //设置物体的颜色
            foreach(var uiColorDesc in uiStateDesc.m_uiColorDescs)
            {
                if (uiColorDesc.m_gameObject != null)
                {
                    Image image = uiColorDesc.m_gameObject.GetComponent<Image>();
                    if (image != null)
                    {
                        image.color = uiColorDesc.m_color;
                    }
                    Text text = uiColorDesc.m_gameObject.GetComponent<Text>();
                    if(text != null)
                    {
                        text.color = uiColorDesc.m_color; 
                    }
                }
            }
            bool hasTweeners = uiStateDesc.m_tweeners != null && uiStateDesc.m_tweeners.Count != 0;
            var lastState = m_curState;
            m_curState = stateName;
            if (!hasTweeners)
            {
                if (onEnd != null)
                {
                    onEnd();
                }
            }
            else
            {
                if (lastState == m_curState && !replayTweeners)
                {
                    if (onEnd != null)
                    {
                        onEnd();
                    }
                }
                else
                {
                    var tweeners = uiStateDesc.m_tweeners;
                    foreach (var tweener in tweeners)
                    {
                        //重置到起点
                        tweener.ResetToBeginning();
                        //移除所有事件
                        tweener.OnFinished.RemoveAllListeners();
                    }
                    //找到最长的tweener
                    TweenMain longestTweener = null;
                    float longestDuration = 0;
                    foreach (var tweener in tweeners)
                    {
                        if (tweener.delay + tweener.duration > longestDuration)
                        {
                            longestDuration = tweener.delay + tweener.duration;
                            longestTweener = tweener;
                        }
                    }
                    if (longestTweener == null)
                    {
                        if (onEnd != null)
                        {
                            onEnd();
                        }
                    }
                    else
                    {
                        //设置回调事件
                        longestTweener.OnFinished.AddListener(() =>
                        {
                            if (onEnd != null)
                            {
                                onEnd();
                            }
                        });
                    }
                    //播放所有tweener
                    foreach (var tweener in tweeners)
                    {
                        tweener.PlayForward();
                    }
                }
            }
        }

        public void SwichToNextState()
        {
            if (m_uiStateDescs.Count == 0)
                return;
            var uiStateDesc = GetUIStateDesc(m_curState);
            var index = m_uiStateDescs.FindIndex((item) => item == uiStateDesc);
            index++;
            if(index >= m_uiStateDescs.Count)
            {
                index = 0;
            }
            uiStateDesc = m_uiStateDescs[index];
            SetUIState(uiStateDesc.m_stateName);
        }
        
    }

}
