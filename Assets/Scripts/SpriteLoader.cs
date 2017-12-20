using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoader {
    private SpriteLoader() { }
    private static SpriteLoader mInstance;
    public static SpriteLoader Instance {
        get
        {
            if (mInstance == null)
            {
                mInstance = new SpriteLoader();
                mInstance.Init();
            }
            return mInstance;
        }
    }

    private Dictionary<string, Sprite> mSprites = new Dictionary<string, Sprite>();

    private void Init()
    {
        GameObject go = Resources.Load<GameObject>("Prefabs/SpriteList");
        SpriteList list = go.GetComponent<SpriteList>();
        foreach (var s in list.m_sprites)
        {
            mSprites[s.name] = s;
        }
    }

    public Sprite GetSprite(string name)
    {
        if (mSprites.ContainsKey(name))
        {
            return mSprites[name];
        }
        else
        {
            Debug.LogError("can't get sprite:" + name);
            return null;
        }
    }
}
