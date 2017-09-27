using UnityEngine;
namespace Mugen3D
{
    public class PlayerLoader
    {
        public static Player LoadPlayer(PlayerId id, string playerName, Vector3 initPos, Transform parent)
        {
            UnityEngine.Object o = Resources.Load<Object>("Chars/" + playerName + "/" + playerName);
            GameObject go = GameObject.Instantiate(o, parent) as GameObject;
            go.name = playerName;
            Player p = go.GetComponent<Player>();
            p.Init(p.setting);
            p.id = id;
            p.transform.localPosition = initPos;
            World.Instance.AddPlayer(id, p);
            return p;
        }
    }
}
