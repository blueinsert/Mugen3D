using System.Collections;
using System.Collections.Generic;
namespace Mugen3D{
public class Triggers
{
    private Dictionary<PlayerId, Player> mPlayers;

    private Triggers() { }

    private static Triggers mInstance;
    public static Triggers Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new Triggers();
                mInstance.Init();
            }
            return mInstance;
        }
    }

    private void Init() {

    }

    public void AddPlayer(PlayerId id, Player p)
    {
        mPlayers[id] = p;
    }

    #region trigger function
    public string AnimName(PlayerId p)
    {
        return mPlayers[p].animCtr.animName;
    }

    public int AnimElem(PlayerId id)
    {
        return mPlayers[id].animCtr.AnimElem;
    }

    public int LeftAnimElem(PlayerId id)
    {
        return mPlayers[id].animCtr.totalFrame - mPlayers[id].animCtr.AnimElem;
    }

    public string Command(PlayerId id)
    {
        return mPlayers[id].cmdMgr.GetActiveCommandName();
    }

    #endregion

}
}
