using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;

public class RoundMgrTrain : RoundMgr {
    private float timer = 0;

    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        timer += Time.deltaTime;
        if (timer >= 5)
        {
            timer = 0;
            var p1 = m_clientGame.world.GetPlayer(PlayerId.P1);
            var p2 = m_clientGame.world.GetPlayer(PlayerId.P2);
            ResetHP(p1);
            ResetHP(p2);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            m_clientGame.ReloadAllPlayer();
            StartRound(1);
        }
    }

    protected override void OnStartRound(int rounNo)
    {
        m_clientGame.p1.SetCtrl(true);
        m_clientGame.p2.SetCtrl(true);
    }

    private void ResetHP(Player p)
    {
        if (p.GetHP() < p.GetMaxHP())
        {
            p.SetHP(p.GetMaxHP());
        }
    }
}
