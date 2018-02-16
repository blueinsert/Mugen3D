
using UnityEngine;
using Mugen3D;

public class PlayerLoader
{
    public static Player LoadPlayer(PlayerId id, string playerName, Transform parent)
    {
        UnityEngine.Object prefab = Resources.Load<UnityEngine.Object>("Chars/" + playerName + "/" + playerName);
        GameObject go = GameObject.Instantiate(prefab, parent) as GameObject;
        go.name = playerName;
        Player p = go.GetComponentInChildren<Player>();
        p.Init();
        p.id = id;
        return p;
    }
}

