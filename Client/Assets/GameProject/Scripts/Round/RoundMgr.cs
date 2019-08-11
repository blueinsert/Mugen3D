using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bluebean.Mugen3D.ClientGame;

public enum RoundState
{
    FadeIn = 0,
    Introducing,
    Fighting,
    BeforeEnd,
    Ending,
}

public class RoundMgr {
    public float leftTime { get { return m_leftTime; } }

    protected ClientGame m_clientGame;
    protected float m_leftTime = 60;
    protected RoundState m_roundState = RoundState.FadeIn;
    protected int m_roundNo = 1;


    public void Init(ClientGame clientGame)
    {
        m_clientGame = clientGame;
        OnInit();
    }

    protected virtual void OnInit()
    {

    }

    public void StartRound(int rounNo)
    {
        this.m_roundNo = rounNo;
        OnStartRound(rounNo); 
    }

    protected virtual void OnStartRound(int rounNo)
    {
        
    }

    public void Update() {
        if (this.m_roundState != RoundState.Fighting)
            return;
        m_leftTime -= Time.deltaTime;
        if (m_leftTime <= 0)
        {
            m_leftTime = 0;
        }
        OnUpdate();
    }

    protected virtual void OnUpdate() { }
}
