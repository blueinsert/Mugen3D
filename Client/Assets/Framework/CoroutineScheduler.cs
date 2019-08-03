using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace bluebean.UGFramework
{
    public class YieldInstruction
    {
        public virtual void Update() { }
        public virtual bool IsPassed() { return true; }
    }
    public class WaitForNextFrame : YieldInstruction
    {

    }

    public class WaitForSeconds : YieldInstruction
    {
        private float m_duration;

        public WaitForSeconds(float duration)
        {
            m_duration = duration;
        }

        public override void Update()
        {
            m_duration -= Time.deltaTime;
        }

        public override bool IsPassed()
        {
            return m_duration <= 0;
        }
    }

    public class WaitForFrames : YieldInstruction
    {
        private int m_frames;

        public WaitForFrames(int frames)
        {
            m_frames = frames;
        }

        public override void Update()
        {
            m_frames--;
        }

        public override bool IsPassed()
        {
            return m_frames <= 0;
        }
    }

    public class WaitUntil : YieldInstruction
    {
        private System.Func<bool> m_funcChecker;

        public WaitUntil(System.Func<bool> checker)
        {
            m_funcChecker = checker;
        }

        public override bool IsPassed()
        {
            return m_funcChecker();
        }
    }

    public class WaitWhile : YieldInstruction
    {
        private System.Func<bool> m_funcChecker;

        public WaitWhile(System.Func<bool> checker)
        {
            m_funcChecker = checker;
        }

        public override bool IsPassed()
        {
            return !m_funcChecker();
        }
    }

    public class WaitAll : YieldInstruction
    {
        private List<Func<bool>> m_funcCheckers;
        private bool m_result = false;

        public WaitAll(List<Func<bool>> checkers)
        {
            m_funcCheckers = checkers;
        }

        public override bool IsPassed()
        {
            return m_result;
        }

        public override void Update()
        {
            foreach (var checker in m_funcCheckers)
            {
                if (checker == null || !checker())
                {
                    m_result = false;
                    return;
                }
            }
            m_result = true;
        }
    }

    public class WaitAny : YieldInstruction
    {
        private List<Func<bool>> m_funcCheckers;
        private bool m_result = false;

        public WaitAny(List<Func<bool>> checkers)
        {
            m_funcCheckers = checkers;
        }

        public override bool IsPassed()
        {
            return m_result;
        }

        public override void Update()
        {
            foreach (var checker in m_funcCheckers)
            {
                if (checker != null && checker())
                {
                    m_result = true;
                    return;
                }
            }
            m_result = false;
        }
    }

    public class CoroutineScheduler : ITickable
    {
        private LinkedList<IEnumerator> corcoutines = new LinkedList<IEnumerator>();
        private List<IEnumerator> deadCorcoutines = new List<IEnumerator>();

        public void Tick()
        {
            var first = corcoutines.First;
            var node = first;
            while (node != null)
            {
                if (!MoveNext(node.Value))
                {
                    deadCorcoutines.Add(node.Value);
                }
                node = node.Next;
            }
            if (deadCorcoutines.Count != 0)
            {
                foreach (var r in deadCorcoutines)
                {
                    corcoutines.Remove(r);
                }
                deadCorcoutines.Clear();
            }
        }

        private bool MoveNext(IEnumerator corcoutine)
        {
            var current = corcoutine.Current;
            if (current is IEnumerator && MoveNext(corcoutine.Current as IEnumerator)) //优先执行子协程
            {
                return true;
            }
            else if (current is YieldInstruction)
            {
                var yieldInstruction = current as YieldInstruction;
                yieldInstruction.Update();
                if (yieldInstruction.IsPassed())
                {
                    return corcoutine.MoveNext();
                }
                return true;
            }
            else
            {
                return corcoutine.MoveNext();
            }
        }

        public void StartCorcoutine(IEnumerator corcoutine)
        {
            corcoutines.AddLast(corcoutine);
        }

    }
}
