using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;


public class RoundMgrSingleVs : RoundMgr {
   
    private FightUI m_fightUI;
    private Mugen3D.Character m_p1;
    private Mugen3D.Character m_p2;
    private int m_p1Score;
    private int m_p2Score;

    protected override void OnInit()
    {
        m_fightUI = m_clientGame.fightUI;
        m_p1 = m_clientGame.p1;
        m_p2 = m_clientGame.p2;
        m_p1.onEvent += OnEvent;
        m_p2.onEvent += OnEvent;
        
        m_p1Score = 0;
        m_p2Score = 0;
    }

    private void OnEvent(Mugen3D.Entity entity, Mugen3D.Event e){
        if (e.type == Mugen3D.EventType.Dead)
        {
            OnKO(entity);
        }
    }


    protected override void OnStartRound(int roundNo)
    {
        m_roundState = RoundState.FadeIn;
        Task task1 = new Task((task) => { 
            m_fightUI.FadeIn(() => { 
                task.Finish();
                m_roundState = RoundState.Introducing;
            }); 
        });
        ParallelTask task2 = new ParallelTask();
        task2.AddTask(new Task((t) => { m_p1.ChangeState(190, () => { t.Finish(); }); }));
        task2.AddTask(new Task((t) => { m_p2.ChangeState(190, () => { t.Finish(); }); }));
        Task task3 = new Task((t) => { m_fightUI.CreateView<ViewPopText>().Show("Round" + roundNo, () => { t.Finish(); }); });
        Task task4 = new Task((t) => { m_fightUI.CreateView<ViewPopText>().Show("Fight" + roundNo, () => { t.Finish(); }); });
        Task task5 = new Task((t) => {  
            m_roundState = RoundState.Fighting;
            t.Finish();
        });
        TaskQueue queue = new TaskQueue();
        queue.AddTask(task1).AddTask(task2).AddTask(task3).AddTask(task4).AddTask(task5);
        queue.Exect();
        this.m_leftTime = 60;
    }

    protected override void OnUpdate()
    {
        if (m_leftTime <= 0)
        {
            OnTimeOver();
        }
    }

    private void OnTimeOver()
    {
        m_roundState = RoundState.BeforeEnd;
        Mugen3D.Character winner = m_p1.GetHP() > m_p2.GetHP() ? m_p1 : m_p2;
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
        var p = e as Mugen3D.Character;
        var winner = p == m_p1 ? m_p2 : m_p1;
        if (winner == m_p1)
        {
            m_p1Score += 1;
        }
        else
        {
            m_p2Score += 1;
        }
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
