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
        /// 栈结构
        /// </summary>
        private List<UITask> m_uiTaskStack = new List<UITask>();
        /// <summary>
        /// 字典结构
        /// </summary>
        private Dictionary<string, List<UITask>> m_uiTaskDic = new Dictionary<string, List<UITask>>();
        /// <summary>
        /// 保存每个name的UITask的最大Id
        /// </summary>
        private Dictionary<string, int> m_maxInstanceIDDic = new Dictionary<string, int>();
        /// <summary>
        /// 注册字典
        /// </summary>
        private Dictionary<string, UITaskRegisterItem> m_uiTaskRegistyerItemDic = new Dictionary<string, UITaskRegisterItem>();
        #endregion

        #region 公共方法

        /// <summary>
        /// 注册UITask条目
        /// </summary>
        /// <param name="uiTaskRegisterItem"></param>
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

        /// <summary>
        /// 开启一个UITask
        /// </summary>
        /// <param name="intent"></param>
        /// <param name="onPrepareEnd"></param>
        /// <param name="onViewUpdateComplete"></param>
        /// <param name="redirectOnLoadAllAssetsComplete"></param>
        /// <returns></returns>
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

        //获取某个taskName类型的UITask最大ID
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

        /// <summary>
        /// 重置某个taskName类型的UITask最大ID
        /// </summary>
        /// <param name="taskName"></param>
        private void ResetInstanceID(string taskName)
        {
            if (!m_maxInstanceIDDic.ContainsKey(taskName))
            {
                m_maxInstanceIDDic.Add(taskName, 0);
            }
            m_maxInstanceIDDic[taskName] = 0;
        }

        /// <summary>
        /// 当UITask销毁
        /// </summary>
        /// <param name="uiTask"></param>
        private void OnUITaskStop(UITask uiTask)
        {
            //从字典中移除
            m_uiTaskDic[uiTask.Name].Remove(uiTask);
            if (m_uiTaskDic[uiTask.Name].Count == 0)
            {
                //重置最大ID
                ResetInstanceID(uiTask.Name);
            }
            //从栈中移除
            m_uiTaskStack.Remove(uiTask);
        }

        private void OnUITaskStart(UITask uiTask)
        {
            //加入栈
            m_uiTaskStack.Add(uiTask);
            if (!m_uiTaskDic.ContainsKey(uiTask.Name))
            {
                m_uiTaskDic.Add(uiTask.Name, new List<UITask>());
            }
            //加入字典
            m_uiTaskDic[uiTask.Name].Add(uiTask);
        }

        /// <summary>
        /// 创建一个新的或者复用存在的UITask
        /// </summary>
        /// <param name="uiTaskRegisterItem"></param>
        /// <returns></returns>
        private UITask CreateOrGetUITaskInstance(UITaskRegisterItem uiTaskRegisterItem)
        {
            UITask instance = null;
            if (uiTaskRegisterItem.AllowMultipleInstance)
            {
                //AllowMultipleInstance总是会创建出新的实例
                instance = ClassLoader.CreateInstance(uiTaskRegisterItem.TypeFullName, uiTaskRegisterItem.Name) as UITask;
                int instanceId = GetInstanceID(uiTaskRegisterItem.Name);
                instance.SetInstanceID(instanceId);
            }
            else
            {
                //复用之前的UITask
                List<UITask> uiTasks;
                m_uiTaskDic.TryGetValue(uiTaskRegisterItem.Name, out uiTasks);
                if (uiTasks != null && uiTasks.Count != 0)
                {
                    instance = uiTasks[0];
                }
                else
                {
                    //如果不存在，创建一个新的
                    instance = ClassLoader.CreateInstance(uiTaskRegisterItem.TypeFullName, uiTaskRegisterItem.Name) as UITask;
                }
            }
            return instance;
        }

        /// <summary>
        /// 开启UITask
        /// </summary>
        /// <param name="uiTask"></param>
        /// <param name="intent"></param>
        /// <param name="onPrepareEnd"></param>
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
