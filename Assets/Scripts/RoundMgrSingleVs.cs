using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;


public class RoundMgrSingleVs : RoundMgr {
   
    public Action onTimerOver;

    protected override void OnInit()
    {
        base.OnInit();
        m_clientGame.world.GetPlayer(PlayerId.P1).onDead += EndRound;
        m_clientGame.world.GetPlayer(PlayerId.P2).onDead += EndRound;
    }

    public override void StartRound(int roundNum)
    {
        roundNo = roundNum;
        m_clientGame.fightUI.FadeIn(() => {
            m_clientGame.fightUI.PopupRound(roundNum, () =>
            {
                m_clientGame.fightUI.PopupFight(() =>
                {
                    m_clientGame.world.GetPlayer(PlayerId.P1).UnlockInput();
                    m_clientGame.world.GetPlayer(PlayerId.P2).UnlockInput();
                    roundState = RoundState.Fighting;
                });
            });
        });
        
    }

    protected override void OnUpdate()
    {
        if (roundState != RoundState.Fighting)
            return;
        leftTime -= Time.deltaTime;
        if (leftTime <= 0)
        {
            TimeOver();
        }
    }

    private void TimeOver()
    {
        if (onTimerOver != null)
        {
            onTimerOver();
        }
        roundState = RoundState.BeforeEnd;
        EndRound();
    }

    public void EndRound()
    {
        m_clientGame.world.GetPlayer(PlayerId.P1).LockInput();
        m_clientGame.world.GetPlayer(PlayerId.P2).LockInput();
        m_clientGame.fightUI.PopupK0(() => {
            m_clientGame.Reset();
            this.roundNo++;
            StartRound(this.roundNo);
        });
    }
}
