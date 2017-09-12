using System.Collections.Generic;
namespace Mugen3D
{
    public class GameEngine
    {    
        public static void Update()
        {
            foreach (var p in World.Instance.Players)
            {
                p.Value.Update();
            }
        }

    }

}
