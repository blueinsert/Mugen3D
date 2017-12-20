using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteList : MonoBehaviour {
    public List<Sprite> m_sprites;

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

    public void Save()
    {

    }
}
