
using UnityEngine;
using Mugen3D;

public class PlayerLoader
{
    public static Player LoadPlayer(PlayerId id, string playerName, Transform parent, Vector3 offset)
    {
        UnityEngine.Object o = Resources.Load<UnityEngine.Object>("Chars/" + playerName + "/" + playerName);
        GameObject go = GameObject.Instantiate(o, parent) as GameObject;
        go.name = playerName;
        go.transform.position = go.transform.position + offset;
        Player p = go.GetComponentInChildren<Player>();
        p.Init(p.setting);
        p.id = id;
        return p;
    }
}

