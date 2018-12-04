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

    public class MatchManager 
    {
        public int matchNo { get; private set; }
        public int roundNo { get; private set; }
        public RoundState roundState { get; private set; }
        public Number roundTime { get; private set; }

        public World world { get; private set; }
        
        
        public MatchManager(World world)
        {
            this.world = world;
            matchNo = 0;
            roundNo = 0;
            ChangeRoundState(RoundState.PreIntro);
            roundTime = 0;
        }

        private bool IsCharactersReady()
        {
            foreach(var c in world.characters)
            {
                if (c.fsmMgr.stateNo != 0)
                    return false;
            }
            return true;
        }

        private bool IsRoundEnd()
        {
            foreach(var c in world.characters)
            {
                if (!c.IsAlive())
                    return true;
            }
            if (roundTime <= 0)
                return true;
            return false;
        }

        public void Update()
        {
            world.Update();
            switch (this.roundState)
            {
                case RoundState.PreIntro:
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
                        ChangeRoundState(RoundState.PreIntro);
                    }
                    break;
                case RoundState.PreOver:
                    break;
                case RoundState.Over:
                    break;
            }
        }

        public void StartRound() {
            ChangeRoundState(RoundState.Intro);
        }

        protected void ChangeRoundState(RoundState roundState)
        {
            this.roundState = roundState;
            this.world.FireEvent(new Event() { type = EventType.OnRoundStateChange, data = this.roundState });
            switch (this.roundState)
            {
                case RoundState.Intro:
                    foreach(var c in world.characters)
                    {
                        c.fsmMgr.ChangeState(5900);
                    }
                    break;
                case RoundState.Over:
                    foreach (var c in world.characters)
                    {
                        if(c.IsAlive())
                            c.fsmMgr.ChangeState(5900);
                    }
                    break;
            }
        }

        protected virtual void OnMatchEnd()
        {

        }
    }
}
