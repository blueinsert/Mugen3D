using System.Collections.Generic;
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
        }

    }

}
