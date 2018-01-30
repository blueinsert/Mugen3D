using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;


public class RoundMgrSingleVs : RoundMgr {
   
    public Action onTimerOver;
    private FightUI m_fightUI;

    protected override void OnInit()
    {
        base.OnInit();
        m_fightUI = m_clientGame.fightUI;
        m_clientGame.world.GetPlayer(PlayerId.P1).onDead += OnKO;
        m_clientGame.world.GetPlayer(PlayerId.P2).onDead += OnKO;
    }

    public override void StartRound(int roundNo)
    {
        this.leftTime = 60;
        this.roundNo = roundNo;
        Action start = () => {
            m_clientGame.world.GetPlayer(PlayerId.P1).UnlockInput();
            m_clientGame.world.GetPlayer(PlayerId.P2).UnlockInput();
            roundState = RoundState.Fighting;
        };
        m_fightUI.FadeIn(() => {
            m_fightUI.InsertView<ViewPopText>((view) => { (view as ViewPopText).Show("Round" + roundNo); });
            m_fightUI.InsertView<ViewPopText>((view) => { (view as ViewPopText).Show("Fight", start); });
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
        //OnKO();
    }

    private void OnKO(Mugen3D.Entity e)
    {
        var diePlayer = e as Mugen3D.Player;
        m_clientGame.world.GetPlayer(PlayerId.P1).LockInput();
        m_clientGame.world.GetPlayer(PlayerId.P2).LockInput();
        string winner = "";
        if (diePlayer.id == PlayerId.P1)
        {
            winner = PlayerId.P2.ToString();
        }
        else if (diePlayer.id == PlayerId.P2)
        {
            winner = PlayerId.P1.ToString();
        }
        Action nextBattle = () => {
            m_fightUI.FadeOut(() =>
            {
                m_clientGame.Reset(); 
                StartRound(this.roundNo + 1);
            });
        };
        m_fightUI.InsertView<ViewPopText>((view) => { (view as ViewPopText).Show("KO"); });
        m_fightUI.InsertView<ViewPopText>((view) => { (view as ViewPopText).Show("Winner is "+winner, nextBattle); });
    }
}
