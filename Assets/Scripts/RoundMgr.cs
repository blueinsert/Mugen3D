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
    protected ClientGame m_clientGame;
    public float leftTime = 60;
    public RoundState roundState = RoundState.FadeIn;
    public int roundNo = 1;


    public void Init(ClientGame clientGame)
    {
        m_clientGame = clientGame;
        OnInit();
    }

    protected virtual void OnInit()
    {

    }

    public virtual void StartRound(int roundNum)
    {
        m_clientGame.world.GetPlayer(PlayerId.P1).UnlockInput();
        m_clientGame.world.GetPlayer(PlayerId.P2).UnlockInput();
    }

    public void Update() {
        OnUpdate();
    }

    protected virtual void OnUpdate() { }
}
