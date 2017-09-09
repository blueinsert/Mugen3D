﻿using UnityEngine;
namespace Mugen3D
{
    public class PlayerLoader
    {
        public static GameObject LoadPlayer(PlayerId id, string playerName, Transform parent)
        {
            UnityEngine.Object o = Resources.Load<Object>("Chars/" + playerName + "/" + playerName);
            GameObject player = GameObject.Instantiate(o, parent) as GameObject;
            player.name = playerName;
            Player script = player.GetComponent<Player>();
            script.Init(script.setting);
            GameEngine.Instance.AddPlayer(id, script);
            return player;
        }
    }
}
