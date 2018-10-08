using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D.Core;

namespace Mugen3D.Tools
{
    public class ActionsEditorModule
    {
        public List<Action> actions;
        public int curActionIndex; 
        public int curActionElemIndex;
        public System.Action doSave;

        public ActionsEditorModule(List<Action> actions)
        {
            this.actions = actions;
            curActionIndex = 0;
            curActionElemIndex = 0;
        }

        public void GoNextAction()
        {
            if(actions != null && actions.Count != 0){
                this.curActionIndex++;
                if (this.curActionIndex >= actions.Count)
                {
                    this.curActionIndex = 0;
                }
                curActionElemIndex = 0;
            }
        }

        public void GoPrevAction()
        {
            if (actions != null && actions.Count != 0)
            {
                this.curActionIndex--;
                if (this.curActionIndex < 0)
                {
                    this.curActionIndex = actions.Count - 1;
                }
                curActionElemIndex = 0;
            }
        }

        public void AddAction()
        {   
           
            if (actions == null)
            {
                actions = new List<Action>();
            }
            int id = 0;
            if (actions.Count > 0)
            {
                id = actions[this.curActionIndex].animNo + 1;
            }
            Action a = new Action(id);
            actions.Insert(this.curActionIndex + 1, a);
            curActionIndex = this.curActionIndex + 1;
        }

        public void DeleteAction()
        {
            if (actions.Count == 0)
                return;
            int newIndex = curActionIndex;
            if (curActionIndex == 0){//delete in the start
                newIndex = 0;

            }else if (curActionIndex == actions.Count - 1) {//in end
                newIndex = actions.Count - 2;
            }
            else {//in middle
                if (actions.Count >= 3)
                {
                    newIndex = curActionIndex;
                }
            }
            actions.RemoveAt(curActionIndex);
            curActionIndex = newIndex;
        }

        public void GoNextActionElem()
        {
            if (actions != null && actions.Count != 0 && actions[curActionIndex].frames != null && actions[curActionIndex].frames.Count != 0) {
                this.curActionElemIndex++;
                if (this.curActionElemIndex >= actions[curActionIndex].frames.Count)
                {
                    this.curActionElemIndex = 0;
                }
            }          
        }

        public void GoPrevActionElem()
        {
            if (actions != null && actions.Count != 0 && actions[curActionIndex].frames != null && actions[curActionIndex].frames.Count != 0)
            {
                this.curActionElemIndex--;
                if (this.curActionElemIndex < 0)
                {
                    this.curActionElemIndex = actions[curActionIndex].frames.Count - 1;
                }
            }      
        }

        public void CreateActionElem()
        {
            if (actions != null && actions.Count != 0 && actions[curActionIndex].frames != null) {
                ActionFrame frame = new ActionFrame();
                int newIndex = 0;
                if (actions[curActionIndex].frames.Count == 0)
                {
                    actions[curActionIndex].frames.Add(frame);
                    newIndex = 0;
                }
                else
                {
                    frame.normalizeTime = actions[curActionIndex].frames[curActionElemIndex].normalizeTime;
                    actions[curActionIndex].frames.Insert(curActionElemIndex + 1, frame);
                    newIndex = curActionElemIndex + 1;
                }
                curActionElemIndex = newIndex;
            }
        }

        public void DeleteActionElem()
        {
            if (actions != null && actions.Count != 0 && actions[curActionIndex].frames != null && actions[curActionIndex].frames.Count != 0)
            {
                int newIndex = curActionElemIndex;
                if (curActionElemIndex == 0)
                {//delete in the start
                    newIndex = 0;

                }
                else if (curActionElemIndex == actions[curActionIndex].frames.Count - 1)
                {//in end
                    newIndex = actions[curActionIndex].frames.Count - 2;
                }
                else
                {//in middle
                    if (actions[curActionIndex].frames.Count >= 3)
                    {
                        newIndex = curActionElemIndex;
                    }
                }
                actions[curActionIndex].frames.RemoveAt(curActionElemIndex);
                curActionElemIndex = newIndex;
            }
           
        }

        public void CreateClsn(int type)
        {
            if (actions != null && actions.Count != 0 && actions[curActionIndex].frames != null && actions[curActionIndex].frames.Count != 0)
            {
                Clsn clsn = new Clsn();
                clsn.type = type;
                clsn.x1 = -1;
                clsn.y1 = -1;
                clsn.x2 = 1;
                clsn.y2 = 1;
                actions[curActionIndex].frames[curActionElemIndex].clsns.Add(clsn);
            } 
        }

        public void DeleteClsn(Clsn clsn)
        {
            if (actions != null && actions.Count != 0 && actions[curActionIndex].frames != null && actions[curActionIndex].frames.Count != 0)
            {
                actions[curActionIndex].frames[curActionElemIndex].clsns.Remove(clsn);
            }
        }

        public void UseLastClsns()
        {
            if (actions != null && actions.Count != 0 && actions[curActionIndex].frames != null && actions[curActionIndex].frames.Count >= 2 && curActionElemIndex >= 1)
            {
                actions[curActionIndex].frames[curActionElemIndex].clsns = new List<Clsn>();
                var lastElem = actions[curActionIndex].frames[curActionElemIndex - 1];
                foreach (var clsn in lastElem.clsns)
                {
                    Clsn newClsn = new Clsn(clsn);
                    actions[curActionIndex].frames[curActionElemIndex].clsns.Add(newClsn);
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