using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class GameEngine
    {
        public static float deltaTime;
        public static void Update(float _deltaTime)
        {
            deltaTime = _deltaTime;
            foreach (var p in World.Instance.Players)
            {
                p.Value.OnUpdate();
            }
            HitBoxManager p1Boxes = World.Instance.GetPlayer(PlayerId.P1).GetComponent<HitBoxManager>();
            HitBoxManager p2Boxes = World.Instance.GetPlayer(PlayerId.P2).GetComponent<HitBoxManager>();
            bool hit = false;
            for (int i = 0; i < p1Boxes.attactBoxes.Count; i++)
            {
                for (int j = 0; j < p2Boxes.defenceBoxes.Count; j++)
                {
                    if(ColliderSystem.CuboidCuboidTest(p1Boxes.attactBoxes[i].cuboid.GetVertexArray().ToArray(), p2Boxes.defenceBoxes[j].cuboid.GetVertexArray().ToArray())){
                        hit = true;
                        break;
                    }
                    if (hit == true)
                        break;
                }
            }
            Debug.Log("hit:" + hit);
        }

    }

}
