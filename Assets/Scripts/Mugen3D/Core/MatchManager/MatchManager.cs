using System;
using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public enum MatchState
    {
        Stop,
        Running,
    }

    public enum RoundState
    {
        PreIntro = 0,
        Intro,
        RoundDeclare,
        Fight,
        PreOver,
        Over,
        PostOver,
    }

    public enum MatchMode
    {
        SinglePlay,
        SingleVS,
        TeamPlay,
        TeamVS,
    }

    [System.Serializable]
    public class MatchInfo
    {
        public MatchMode mode;
        public string[] p1Characters;
        public string[] p2Characters;
        public string stage;
    }

    public class MatchManager 
    {
        public int matchNo { get; private set; }
        public MatchState matchState { get; private set; }
        public int roundNo { get; private set; }
        public RoundState roundState { get; private set; }
        public Number roundTime { get; private set; }
        public MatchInfo matchInfo { get; private set; }
        public Character p1 { get; protected set; }
        public Character p2 { get; protected set; }
        public World world { get; private set; }
        public Action<Event> onEvent;
        private Number timer;

        public readonly Number FADE_IN_TIME = new Number(1);
        public readonly Number FADE_OUT_TIME = new Number(1);
        public readonly Number ROUND_DECLARATION_TIME = new Number(3);
        public readonly Number PRE_OVER_TIME = new Number(3);
        public readonly Number OVER_TIME = new Number(3);

        public MatchManager(World world, MatchInfo info)
        {
            this.world = world;
            this.matchInfo = info;
            this.matchState = MatchState.Stop;
            this.roundState = RoundState.PreIntro;
            matchNo = 0;
            roundNo = 0;
            roundTime = 0;
        }

        public void FireEvent(Event evt)
        {
            if (onEvent != null)
            {
                onEvent(evt);
            }
        }

        #region checker
        private bool IsCharactersReady()
        {
            return p1.fsmMgr.stateNo == 0 && p2.fsmMgr.stateNo == 0;  
        }

        private bool IsCharactersSteady()
        {
            return p1.fsmMgr.stateNo == 0 && p2.fsmMgr.stateNo == 0;     
        }

        private bool IsRoundEnd()
        {
            if (!p1.IsAlive() || !p2.IsAlive())
                return true; 
            if (roundTime <= 0)
                return true;
            return false;
        }

        protected Character GetWiner()
        {
            if(p1.GetHP() > p2.GetHP())
            {
                return p1;
            }else if(p2.GetHP() > p1.GetHP())
            {
                return p2;
            }
            else
            {
                return null;
            }
        }

        protected Character GetLoser()
        {
            if (p1.GetHP() > p2.GetHP())
            {
                return p2;
            }
            else if (p2.GetHP() > p1.GetHP())
            {
                return p1;
            }
            else
            {
                return null;
            }
        }
        #endregion

        public void Update()
        {
            if (matchState == MatchState.Stop)
                return;
            switch (this.roundState)
            {
                case RoundState.PreIntro:
                    timer += Time.deltaTime;
                    if (timer >= FADE_IN_TIME)
                        ChangeRoundState(RoundState.Intro);
                    break;
                case RoundState.Intro:
                    if (IsCharactersReady())
                    {
                        ChangeRoundState(RoundState.RoundDeclare);
                    }
                    break;
                case RoundState.RoundDeclare:
                    timer += Time.deltaTime;
                    if(timer >= ROUND_DECLARATION_TIME)
                    {
                        ChangeRoundState(RoundState.Fight);
                    }
                    break;
                case RoundState.Fight:
                    roundTime -= Time.deltaTime;
                    if (roundTime < 0)
                        roundTime = 0;
                    if (IsRoundEnd())
                    {
                        ChangeRoundState(RoundState.PreOver);
                    }
                    break;
                case RoundState.PreOver:
                    timer += Time.deltaTime;
                    if(timer >= PRE_OVER_TIME)
                        ChangeRoundState(RoundState.Over);
                    break;
                case RoundState.Over:
                    timer += Time.deltaTime;
                    if(timer >= OVER_TIME)
                    {
                        ChangeRoundState(RoundState.PostOver);
                    }
                    break;
                case RoundState.PostOver:
                    timer += Time.deltaTime;
                    if (timer >= FADE_OUT_TIME)
                    {
                        StopRound();
                    }
                    break;
            }
        }

        protected void ChangeRoundState(RoundState roundState)
        {
            timer = 0;
            this.roundState = roundState;
            FireEvent(new Event() { type = EventType.OnRoundStateChange, data = this.roundState });
            switch (this.roundState)
            {
                case RoundState.PreIntro:
                    p1.fsmMgr.ChangeState(0);
                    p2.fsmMgr.ChangeState(0);
                    break;
                case RoundState.Intro:
                    p1.fsmMgr.ChangeState(5900);
                    p2.fsmMgr.ChangeState(5900); 
                    break;
                case RoundState.Over:
                    if (GetLoser() != null)
                    {
                        GetLoser().fsmMgr.ChangeState(170);
                        GetWiner().fsmMgr.ChangeState(180);
                    }
                    else
                    {
                        p1.fsmMgr.ChangeState(170);
                        p2.fsmMgr.ChangeState(170);
                    }
                    break;
            }
        }  

        #region public
        public void StartMatch(int matchNo)
        {
            this.matchNo = matchNo;
            OnMatchStart();
            StartRound(0);
            this.matchState = MatchState.Running;
        }

        protected void StopMatch() {
            this.matchState = MatchState.Stop;
            OnMatchEnd();
        }

        protected void StartRound(int roundNo)
        {
            this.roundNo = roundNo;
            this.roundTime = 60;
            OnRoundStart();
            ChangeRoundState(RoundState.PreIntro);
        }

        protected void StopRound()
        {
            OnRoundEnd();
        }
        #endregion

        protected virtual void OnMatchStart() { }
        protected virtual void OnMatchEnd() { }
        protected virtual void OnRoundStart() { }
        protected virtual void OnRoundEnd() { }
    }
}
