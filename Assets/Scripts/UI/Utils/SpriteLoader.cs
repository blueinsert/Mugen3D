
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FSoul
{

    public class SpriteLoader
    {

        private static Dictionary<string, Dictionary<string, Sprite>> m_atlas = new Dictionary<string, Dictionary<string, Sprite>>();

        public static Sprite Load(string atlas, string name)
        {
            Dictionary<string, Sprite> sprites;

            if (!m_atlas.TryGetValue(atlas, out sprites))
            {
                var ss = Resources.LoadAll<Sprite>("Atlas/" + atlas);

                if (ss != null && ss.Length > 0)
                {
                    sprites = new Dictionary<string, Sprite>();
                    foreach (var s in ss)
                    {
                        sprites.Add(s.name, s);
                    }
                }
            }

            if (sprites != null)
            {
                Sprite s;
                if (sprites.TryGetValue(name, out s))
                {
                    return s;
                }
            }

            return null;
        }

    }

}