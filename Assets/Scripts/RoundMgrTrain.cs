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

    private void ResetHP(Player p)
    {
        if (p.hp < p.MaxHP)
        {
            p.MakeDamage(p.hp - p.MaxHP);
        }
    }
}
