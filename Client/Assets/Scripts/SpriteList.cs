using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpriteList : MonoBehaviour {
    [HideInInspector]
    [SerializeField]
    private List<Sprite> m_sprites = new List<Sprite>();

    public List<Sprite> GetSprite()
    {
        return m_sprites;
    }

    public void AddSprite(Sprite s)
    {
        m_sprites.Add(s);
    }

    public void RemoveSprite(Sprite s)
    {
        m_sprites.Remove(s);
    }

    public int GetSpriteNum()
    {
        return m_sprites.Count;
    }

    public void RemoveDuplicate()
    {
        Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();
        foreach(var sprite in m_sprites){
            spriteDic[sprite.name] = sprite;
        }
        List<Sprite> spriteList = new List<Sprite>();
        foreach(var kv in spriteDic){
            spriteList.Add(kv.Value);
        }
        m_sprites = spriteList;
    }
}
