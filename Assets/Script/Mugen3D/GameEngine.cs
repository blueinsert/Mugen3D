using System.Collections.Generic;
namespace Mugen3D
{
    public class GameEngine
    {
        private static GameEngine mInstance;
        public static GameEngine Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new GameEngine();
                    mInstance.Init();
                }
                return mInstance;
            }
        }

        private void Init()
        {

        }

        private Dictionary<PlayerId, Player> mPlayers = new Dictionary<PlayerId,Player>();

        public void Update()
        {
            foreach (var p in mPlayers)
            {
                p.Value.UpdatePlayer();
            }
        }

        public void AddPlayer(PlayerId id, Player p) {
            mPlayers[id] = p;
            Triggers.Instance.AddPlayer(id, p);
            Controllers.Instance.AddPlayer(id, p);
        }

    }
}
