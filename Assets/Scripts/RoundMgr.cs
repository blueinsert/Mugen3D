using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;

public enum RoundState
{
    FadeIn = 0,
    Introducing,
    Fighting,
    BeforeEnd,
    Ending,
}

public class RoundMgr {
    private ClientGame m_clientGame;
    public RoundState roundState = RoundState.FadeIn;
    public float leftTime = 60;
    public int roundNo;
    public Action onTimerOver;

    private void Init()
    {
        m_clientGame.world.GetPlayer(PlayerId.P1).onDead += EndRound;
        m_clientGame.world.GetPlayer(PlayerId.P2).onDead += EndRound;
    }

    public void SetClientGame(ClientGame clientGame)
    {
        m_clientGame = clientGame;
        Init();
    }

    public void StartRound(int roundNum)
    {
        roundNo = roundNum;
        m_clientGame.fightUI.PopupRound(roundNum, () =>
        {
            m_clientGame.fightUI.PopupFight(() =>
            {
                m_clientGame.world.GetPlayer(PlayerId.P1).UnlockInput();
                m_clientGame.world.GetPlayer(PlayerId.P2).UnlockInput();
                roundState = RoundState.Fighting;
            });
        });
    }

    public void Update()
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
