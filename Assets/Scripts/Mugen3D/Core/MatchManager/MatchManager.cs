using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public enum RoundState
    {
        PreIntro = 0,
        Intro,
        Fight,
        PreOver,
        Over,
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
        public int roundNo { get; private set; }
        public RoundState roundState { get; private set; }
        public Number roundTime { get; private set; }
        public MatchInfo matchInfo { get; private set; }
        public Character p1 { get; protected set; }
        public Character p2 { get; protected set; }
        public World world { get; private set; }   

        private Number timer;
        
        public MatchManager(World world, MatchInfo info)
        {
            this.world = world;
            this.matchInfo = info;
            matchNo = 0;
            roundNo = 0;
            roundTime = 0;
        }
        #region checker
        private bool IsCharactersReady()
        {
            foreach(var c in world.characters)
            {
                if (c.Value.fsmMgr.stateNo != 0)
                    return false;
            }
            return true;
        }

        private bool IsCharactersSteady()
        {
            foreach(var c in world.characters)
            {
                /*
                if (c.Value.IsAlive())
                {
                    if (!(c.Value.fsmMgr.stateNo == 0 || c.Value.fsmMgr.stateNo == 170 || c.Value.fsmMgr.stateNo == 180))
                        return false;
                }
                else
                {
                    if (!(c.Value.fsmMgr.stateNo == 5110))
                        return false;
                }  
                */
                if (c.Value.fsmMgr.stateNo != 0)
                    return false;
                
            }
            return true;
        }

        private bool IsRoundEnd()
        {
            foreach(var c in world.characters)
            {
                if (!c.Value.IsAlive())
                    return true;
            }
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
        #endregion

        public void Update()
        {
            world.Update();
            switch (this.roundState)
            {
                case RoundState.PreIntro:
                    timer += Time.deltaTime;
                    if (timer >= 2)
                        ChangeRoundState(RoundState.Intro);
                    break;
                case RoundState.Intro:
                    if (IsCharactersReady())
                    {
                        ChangeRoundState(RoundState.Fight);
                    }
                    break;
                case RoundState.Fight:
                    roundTime -= Time.deltaTime;
                    if (IsRoundEnd())
                    {
                        ChangeRoundState(RoundState.PreOver);
                    }
                    break;
                case RoundState.PreOver:
                    if (IsCharactersSteady())
                        ChangeRoundState(RoundState.Over);
                    break;
                case RoundState.Over:
                    timer += Time.deltaTime;
                    if(timer >= 2)
                    {
                        OnRoundEnd();
                    }
                    break;
            }
        }

        protected void ChangeRoundState(RoundState roundState)
        {
            timer = 0;
            this.roundState = roundState;
            this.world.FireEvent(new Event() { type = EventType.OnRoundStateChange, data = this.roundState });
            switch (this.roundState)
            {
                case RoundState.PreIntro:
                    foreach (var c in world.characters)
                    {
                        c.Value.fsmMgr.ChangeState(0);
                    }
                    break;
                case RoundState.Intro:
                    foreach(var c in world.characters)
                    {
                        c.Value.fsmMgr.ChangeState(5900);
                    }
                    break;
                case RoundState.Over:
                    foreach (var c in world.characters)
                    {
                        if(GetWiner() != c.Value)
                            c.Value.fsmMgr.ChangeState(170);//lose
                        if (GetWiner() == c.Value)
                            c.Value.fsmMgr.ChangeState(180);
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
        }

        public void StopMatch() {
            OnMatchEnd();
        }

        public void StartRound(int roundNo)
        {
            this.roundNo = roundNo;
            this.roundTime = 60;
            ChangeRoundState(RoundState.PreIntro);
            OnRoundStart();
        }
        #endregion

        protected virtual void OnMatchStart() { }
        protected virtual void OnMatchEnd() { }
        protected virtual void OnRoundStart() { }
        protected virtual void OnRoundEnd() { }
    }
}
