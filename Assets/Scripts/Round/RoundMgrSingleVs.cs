using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;


public class RoundMgrSingleVs : RoundMgr {
   
    private FightUI m_fightUI;
    private Mugen3D.Player m_p1;
    private Mugen3D.Player m_p2;
    private int m_p1Score;
    private int m_p2Score;

    protected override void OnInit()
    {
        m_fightUI = m_clientGame.fightUI;
        m_p1 = m_clientGame.world.GetPlayer(PlayerId.P1);
        m_p2 = m_clientGame.world.GetPlayer(PlayerId.P2);
        m_p1.onDead += OnKO;
        m_p2.onDead += OnKO;
        m_roundState = RoundState.FadeIn;
        m_p1Score = 0;
        m_p2Score = 0;
    }

    protected override void OnStartRound(int roundNo)
    {
        this.m_leftTime = 60;
        Action start = () => {
            m_clientGame.world.GetPlayer(PlayerId.P1).UnlockInput();
            m_clientGame.world.GetPlayer(PlayerId.P2).UnlockInput();
            m_roundState = RoundState.Fighting;
        };
        m_fightUI.FadeIn(() => {
            m_roundState = RoundState.Introducing;
            m_fightUI.InsertView<ViewPopText>((view) => { (view as ViewPopText).Show("Round" + roundNo); });
            m_fightUI.InsertView<ViewPopText>((view) => { (view as ViewPopText).Show("Fight", start); });
        });   
    }

    protected override void OnUpdate()
    {
        if (m_roundState != RoundState.Fighting)
            return;
        if (m_leftTime <= 0)
        {
            OnTimeOver();
        }
    }

    private void OnTimeOver()
    {
        m_roundState = RoundState.BeforeEnd;
        Mugen3D.Player winner = m_p1.hp > m_p2.hp ? m_p1 : m_p2;
        if (winner == m_p1)
        {
            m_p1Score += 1;
        }
        else
        {
            m_p2Score += 1;
        }
        m_fightUI.InsertView<ViewPopText>((view) => { (view as ViewPopText).Show("TimeOver"); });
        m_fightUI.InsertView<ViewPopText>((view) =>
        {
            (view as ViewPopText).Show("Winner is " + winner.id.ToString(), BegenNextMatch);
            m_roundState = RoundState.Ending;
        });
    }

    private void OnKO(Mugen3D.Entity e)
    {
        m_roundState = RoundState.BeforeEnd;
        var p = e as Mugen3D.Player;
        var winner = p == m_p1 ? m_p2 : m_p1;
        if (winner == m_p1)
        {
            m_p1Score += 1;
        }
        else
        {
            m_p2Score += 1;
        }
        m_p1.LockInput();
        m_p1.LockInput();
        m_fightUI.InsertView<ViewPopText>((view) => { (view as ViewPopText).Show("KO"); });
        m_fightUI.InsertView<ViewPopText>((view) => {
            (view as ViewPopText).Show("Winner is " + winner.id.ToString(), BegenNextMatch);
            m_roundState = RoundState.Ending;
        });
    }

    private void BegenNextMatch()
    {
        if (TryEndMatch())
            return;
        m_fightUI.FadeOut(() =>
        {
            m_clientGame.Reset();
            StartRound(this.m_roundNo + 1);
        });
    }

    private bool TryEndMatch()
    {
        Debug.Log("p1Score:" + m_p1Score + " p2Score:" + m_p2Score);
        if (m_p1Score >= 2 || m_p2Score >= 2)
        {
            m_fightUI.CreateView<ViewWinPlayer>().Show();
            return true;
        }
        return false;
    }
}
