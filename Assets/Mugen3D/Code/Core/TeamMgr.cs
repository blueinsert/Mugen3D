using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class TeamMgr
    {

        public static Player GetEnemy(Player p)
        {
            Player enemy = null;
            if (p.id == PlayerId.P1)
            {
                enemy = World.Instance.GetPlayer(PlayerId.P2);

            }else if(p.id == PlayerId.P2){
                enemy = World.Instance.GetPlayer(PlayerId.P1);
            }
            return enemy;
        }
       
    }
}
