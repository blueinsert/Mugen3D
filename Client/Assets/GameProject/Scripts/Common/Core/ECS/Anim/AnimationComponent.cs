using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 动画帧数据
    /// </summary>
    public class ActionFrame
    {
        public Number normalizeTime { get; set; }
        public int duration { get; set; }
        public List<Clsn> clsns { get; set; }

        public Number xOffset { get; set; }
        public Number yOffset { get; set; }

        public ActionFrame()
        {
            clsns = new List<Clsn>();
        }
    }

    /// <summary>
    /// 动画定义数据
    /// </summary>
    public class ActionDef
    {
        public int animNo { get; set; }
        public string animName { get; set; }
        public int animLength { get; set; }
        public List<ActionFrame> frames { get; set; }
        public int loopStartIndex { get; set; }

        public ActionDef()
        {
            frames = new List<ActionFrame>();
        }

        public ActionDef(int actionNo)
        {
            this.animNo = actionNo;
            frames = new List<ActionFrame>();
        }

        public void CalculateAnimLength()
        {
            int length = 0;
            foreach (var frame in frames)
            {
                length += frame.duration;
            }
            this.animLength = length;
        }
    }

    /// <summary>
    /// 动画组件
    /// </summary>
    public class AnimationComponent : ComponentBase
    {
        /// <summary>
        /// 动画配置字典
        /// </summary>
        private readonly Dictionary<int, ActionDef> m_actions = new Dictionary<int, ActionDef>();
        /// <summary>
        /// 当前的动画编号
        /// </summary>
        public int m_anim;
        /// <summary>
        /// 当前的动画的持续时间
        /// </summary>
        public int m_animTime;
        /// <summary>
        /// 当前位于动画第几帧
        /// </summary>
        public int m_animElem;
        /// <summary>
        /// 该帧的已持续事件
        /// </summary>
        public int m_animElemTime;

        public int AnimLength
        {
            get
            {
                return CurAction.animLength;
            }
        }

        public int LeftAnimTime
        {
            get
            {
                return AnimLength - m_animTime;
            }
        }

        public ActionDef CurAction
        {
            get
            {
                return m_actions[m_anim];
            }
        }

        public ActionFrame CurActionFrame
        {
            get
            {
                return m_actions[m_anim].frames[m_animElem];
            }
        }

        /// <summary>
        /// 动画是否存在
        /// </summary>
        /// <param name="anim"></param>
        /// <returns></returns>
        public bool IsAnimExist(int anim)
        {
            return m_actions.ContainsKey(anim);
        }

        private void Init(ActionDef[] actions)
        {
            this.m_actions.Clear();
            foreach (var action in actions)
            {
                m_actions.Add(action.animNo, action);
            }
        }

        public void UpdateSample()
        {
            var animElemDuration = CurActionFrame.duration;
            if (m_animElemTime >= animElemDuration) //当前帧已到末尾
            {
                if (m_animElem < CurAction.frames.Count - 1)//不是最后一帧
                {
                    m_animElem++;
                    m_animElemTime = 0;
                }
                else if (m_animElem == CurAction.frames.Count - 1)//最后一帧
                {
                    if(CurAction.loopStartIndex != -1)//循环动画
                    {
                        m_animElem = CurAction.loopStartIndex;
                        m_animElemTime = 0;
                    }
                    
                }
            }
            else
            {
                m_animTime++;
                m_animElemTime++;
            }
        }

        public void ChangeAnim(int anim, int animElem = 0)
        {
            if (!m_actions.ContainsKey(anim))
            {
                Debug.LogError("anims don't contain id: " + anim);
                return;
            }
            this.m_anim = anim;
            this.m_animElem = animElem;
            this.m_animTime = -1;
            this.m_animElemTime = -1;
        }

    }

}
