using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class GameEngine
    {
        public static int gameTime = -1;
        public static float deltaTime;
        public static void Update(float _deltaTime)
        {
            gameTime++;
            deltaTime = _deltaTime;
            foreach (var p in World.Instance.Players)
            {
                p.Value.OnUpdate();
            }
            UpdateFacing();
        }

        private static void UpdateFacing(){
            var p1 = World.Instance.GetPlayer(PlayerId.P1);
            var p2 = World.Instance.GetPlayer(PlayerId.P2);
            if (p1.transform.position.z > p2.transform.position.z)
            {
                p1.ChangeFacing(-1);
                p2.ChangeFacing(1);
            }
            else
            {
                p1.ChangeFacing(1);
                p2.ChangeFacing(-1);
            }
        }

    }

}
