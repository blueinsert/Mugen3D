using System;
using System.Collections;
using System.Collections.Generic;
using Debug = bluebean.UGFramework.Log.Debug;

namespace bluebean.UGFramework
{
    public enum TaskState
    {
        Init,
        Runing,
        Paused,
        Stop,
    }

    public abstract class Task : ITickable
    {
        public TaskState State { get { return m_state; } }
        public string Name { get { return m_name; } }

        private string m_name;
        private TaskState m_state = TaskState.Init;
        
        public event Action EventOnStart;
        public event Action EventOnStop;
        public event Action EventOnPause;
        public event Action EventOnResume;

        public Task(string name = "")
        {
            m_name = name;
        }

        public void Start(Object param)
        {
            if(m_state!= TaskState.Init)
            {
                Debug.LogError("TaskBase:Start but state is't init");
                return;
            }
            m_state = TaskState.Runing;
            TaskManager.Instance.Register(this);
            if(!OnStart(param))
            {
                Debug.LogError(string.Format("task {0} OnStart Failed", m_name));
                Stop();
                return;
            }
            if (EventOnStart != null)
            {
                EventOnStart();
            } 
        }

        protected virtual bool OnStart(Object param)
        {
            return true;
        }

        public void Pause()
        {
            if (m_state != TaskState.Runing)
            {
                Debug.LogError("TaskBase:Pause but state is't Running");
                return;
            }
            m_state = TaskState.Paused;
            OnPause();
            if (EventOnPause != null)
            {
                EventOnPause();
            }
        }

        protected virtual void OnPause()
        {

        }

        public void Resume(Object param)
        {
            if (m_state != TaskState.Paused)
            {
                Debug.LogError("TaskBase:Resume but state is't Paused");
                return;
            }
            m_state = TaskState.Runing;
            if(!OnResume(param))
            {
                Pause();
                Debug.LogError(string.Format("task {0} OnResume Failed!", m_name));
                return;
            }
            if (EventOnResume != null)
            {
                EventOnResume();
            }
        }

        protected virtual bool OnResume(Object param)
        {
            return true;
        }

        public void Stop()
        {
            m_state = TaskState.Stop;
            OnStop();
            TaskManager.Instance.UnRegisterTask(this);
            if (EventOnStop != null)
            {
                EventOnStop();
            }
        }

        protected virtual void OnStop()
        {

        }

        public void Tick()
        {
            if(m_state == TaskState.Runing)
                OnTick();
        }

        protected virtual void OnTick()
        {

        }
    }

}
