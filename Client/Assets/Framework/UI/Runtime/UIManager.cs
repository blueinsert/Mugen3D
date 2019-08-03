using System;
using System.Collections;
using System.Collections.Generic;
using Debug = bluebean.UGFramework.Log.Debug;

namespace bluebean.UGFramework.UI
{
    public class UITaskRegisterItem
    {
        public string Name { get; set; }
        public string TypeFullName { get; set; }
        public bool AllowMultipleInstance { get; set; }
    }

    public class UIManager
    {

        #region 单例模式
        private UIManager() { }
        public static UIManager m_instance;
        public static UIManager Instance { get { return m_instance; } }
        public static UIManager CreateInstance()
        {
            m_instance = new UIManager();
            return m_instance;
        }
        #endregion

        #region 内部变量
        /// <summary>
        /// uiTask stack
        /// </summary>
        private List<UITask> m_uiTaskStack = new List<UITask>();
        private Dictionary<string, List<UITask>> m_uiTaskDic = new Dictionary<string, List<UITask>>();
        private Dictionary<string, int> m_maxInstanceIDDic = new Dictionary<string, int>();

        private Dictionary<string, UITaskRegisterItem> m_uiTaskRegistyerItemDic = new Dictionary<string, UITaskRegisterItem>();
        #endregion

        #region 公共方法

        public void RegisterUITask(UITaskRegisterItem uiTaskRegisterItem)
        {
            if (string.IsNullOrEmpty(uiTaskRegisterItem.Name))
            {
                Debug.LogError("uiTaskRegisterItem's name is null or empty");
                return;
            }
            if (m_uiTaskRegistyerItemDic.ContainsKey(uiTaskRegisterItem.Name))
            {
                Debug.LogError("RegisterUITask, Already contain the same name UITask,name:" + uiTaskRegisterItem.Name);
                return;
            }
            m_uiTaskRegistyerItemDic.Add(uiTaskRegisterItem.Name, uiTaskRegisterItem);
        }

        public bool StartUITask(UIIntent intent, Action<bool> onPrepareEnd = null, Action onViewUpdateComplete = null, Action redirectOnLoadAllAssetsComplete = null)
        {
            if (!m_uiTaskRegistyerItemDic.ContainsKey(intent.Name))
            {
                Debug.LogError("StartUITask can't find UITaskRegisterItem name:" + intent.Name);
                return false;
            }
            var uiTaskRegisterItem = m_uiTaskRegistyerItemDic[intent.Name];
            var uiTask = CreateOrGetUITaskInstance(uiTaskRegisterItem);
            if (uiTask == null)
            {
                Debug.LogError("StartUITask Can't create UITask instance, typeFullName:" + uiTaskRegisterItem.TypeFullName);
            } 
            if (redirectOnLoadAllAssetsComplete != null)
            {
                uiTask.UpdateCtx.SetRedirectOnLoadAssetComplete(redirectOnLoadAllAssetsComplete);
            }
            if (onViewUpdateComplete != null)
            {
                uiTask.UpdateCtx.m_onViewUpdateComplete = onViewUpdateComplete;
            }
            StartUITaskInternal(uiTask, intent, onPrepareEnd);
            return true;
        }

        #endregion

        #region 内部方法

        private int GetInstanceID(string taskName)
        {
            if (!m_maxInstanceIDDic.ContainsKey(taskName))
            {
                m_maxInstanceIDDic.Add(taskName, 0);
            }
            int instanceId = m_maxInstanceIDDic[taskName];
            m_maxInstanceIDDic[taskName] = m_maxInstanceIDDic[taskName] + 1;
            return instanceId;
        }

        private void ResetInstanceID(string taskName)
        {
            if (!m_maxInstanceIDDic.ContainsKey(taskName))
            {
                m_maxInstanceIDDic.Add(taskName, 0);
            }
            m_maxInstanceIDDic[taskName] = 0;
        }

        private void OnUITaskStop(UITask uiTask)
        {
            m_uiTaskDic[uiTask.Name].Remove(uiTask);
            if (m_uiTaskDic[uiTask.Name].Count == 0)
            {
                ResetInstanceID(uiTask.Name);
            }
            m_uiTaskStack.Remove(uiTask);
        }

        private void OnUITaskStart(UITask uiTask)
        {
            //add to cache
            m_uiTaskStack.Add(uiTask);
            if (!m_uiTaskDic.ContainsKey(uiTask.Name))
            {
                m_uiTaskDic.Add(uiTask.Name, new List<UITask>());
            }
            m_uiTaskDic[uiTask.Name].Add(uiTask);
        }

        private UITask CreateOrGetUITaskInstance(UITaskRegisterItem uiTaskRegisterItem)
        {
            UITask instance = null;
            if (uiTaskRegisterItem.AllowMultipleInstance)
            {
                instance = ClassLoader.CreateInstance(uiTaskRegisterItem.TypeFullName, uiTaskRegisterItem.Name) as UITask;
                int instanceId = GetInstanceID(uiTaskRegisterItem.Name);
                instance.SetInstanceID(instanceId);
            }
            else
            {
                List<UITask> uiTasks;
                m_uiTaskDic.TryGetValue(uiTaskRegisterItem.Name, out uiTasks);
                if (uiTasks != null && uiTasks.Count != 0)
                {
                    instance = uiTasks[0];
                }
                else
                {
                    instance = ClassLoader.CreateInstance(uiTaskRegisterItem.TypeFullName, uiTaskRegisterItem.Name) as UITask;
                }
            }
            return instance;
        }

        private void StartUITaskInternal(UITask uiTask, UIIntent intent, Action<bool> onPrepareEnd = null)
        {
            uiTask.PrapareDataForStart((res) => {
                if (onPrepareEnd != null)
                {
                    onPrepareEnd(res);
                }
                if (res)
                {
                    StartOrResumeUITask(uiTask, intent);
                }
                else
                {
                    Debug.LogError(string.Format("PrapareDataForStart Failed UITask Name:", uiTask.Name));
                }
            });   
        }

        private void StartOrResumeUITask(UITask uiTask, UIIntent intent)
        {
            switch (uiTask.State)
            {
                case TaskState.Init:
                    uiTask.UpdateCtx.m_isInit = true;
                    uiTask.EventOnStop += () => { OnUITaskStop(uiTask); };
                    uiTask.EventOnStart += () => { OnUITaskStart(uiTask); };
                    uiTask.Start(intent);
                    break;
                case TaskState.Paused:
                    uiTask.UpdateCtx.m_isResume = true;
                    uiTask.Resume(intent);
                    break;
                case TaskState.Runing:
                    uiTask.OnNewIntent(intent);
                    break;
            }
        }

        #endregion
   
    }
}
