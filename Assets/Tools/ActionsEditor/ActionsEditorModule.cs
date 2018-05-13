using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D.Tools
{
    public class ActionsEditorModule
    {
        private List<Mugen3D.Action> m_actions;
        private int m_curActionIndex; //begin from 1
        private int m_curActionElemIndex;//begin from 1
        public System.Action doSave;

        public List<Mugen3D.Action> actions
        {
            get
            {
                return m_actions;
            }
        }

        public Action curAction { 
            get {
                return m_actions[m_curActionIndex - 1];
            } 
        }

        public ActionFrame curActionElem
        {
            get
            {
                return curAction.frames[m_curActionElemIndex - 1];
            }
        }

        public int curActionIndex {
            get
            {
                return m_curActionIndex;
            }
        }


        public int curActionElemIndex
        {
            get {
                return m_curActionElemIndex;
            }
        }

        public int actionLength {
            get
            {
                return m_actions.Count;
            }
        }

        public ActionsEditorModule(List<Mugen3D.Action> actions)
        {
            this.m_actions = actions;
            m_curActionIndex = 1;
            m_curActionElemIndex = 1;
        }

        public void GoNextAction()
        {
            this.m_curActionIndex++;
            if (this.m_curActionIndex > actionLength)
            {
                this.m_curActionIndex = 1;
            }
        }

        public void GoPrevAction()
        {
            this.m_curActionIndex--;
            if (this.m_curActionIndex < 1)
            {
                this.m_curActionIndex = actionLength;
            }
        }

        public void AddAction()
        {   
            int id = 0;
            if (actionLength > 0)
            {
                id = m_actions[actionLength - 1].animNo + 1;
            }
            Action a = new Action(id);
            m_actions.Add(a);
            m_curActionIndex = actionLength;
        }

        public void DeleteAction()
        {
            m_actions.Remove(curAction);
            if (actionLength > 0)
            {
                if (m_curActionIndex > 1)
                {
                    m_curActionIndex--;
                }
            }
        }

        public void GoNextActionElem()
        {
            this.m_curActionElemIndex++;
            if (this.m_curActionElemIndex > curAction.frames.Count)
            {
                this.m_curActionElemIndex = 1;
            }
        }

        public void GoPrevActionElem()
        {
            this.m_curActionElemIndex--;
            if (this.m_curActionElemIndex < 1)
            {
                this.m_curActionElemIndex = curAction.frames.Count;
            }
        }

        public void CreateActionElem()
        {
            ActionFrame frame = new ActionFrame();
            curAction.frames.Add(frame);
            m_curActionElemIndex = curAction.frames.Count;
        }

        public void DeleteActionElem()
        {
            curAction.frames.Remove(curActionElem);
            if (curAction.frames.Count > 0)
            {
                if (m_curActionElemIndex > 1)
                {
                    m_curActionElemIndex--;
                }
            }
        }

        public void Save()
        {
            if (doSave != null)
            {
                doSave();
            }
        }
    }
}