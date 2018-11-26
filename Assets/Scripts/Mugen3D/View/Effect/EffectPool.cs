using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class EffectPool : Singleton<EffectPool>
    {
        public static readonly string FX_PATH = "FX/";

        int m_currentId = 0;
        Dictionary<string, GameObject> m_prefabCache = new Dictionary<string, GameObject>();
        Dictionary<int, EffectObj> m_activeEffects = new Dictionary<int, EffectObj>();

        public void Play(string name, Vector3 pos, Transform parent)
        {
            var effectObj = LoadEffectObj(name, pos, parent);
            if (effectObj != null)
            {
                effectObj.Play();
                effectObj.onFinish += (id) => {
                    m_activeEffects.Remove(id);
                    GameObject.Destroy(effectObj.gameObject);
                };
            }
        }

        EffectObj LoadEffectObj(string name, Vector3 pos, Transform parent)
        {
            GameObject prefab = null;
            if (!m_prefabCache.ContainsKey(name))
            {
                prefab = ResourceLoader.Load<GameObject>(FX_PATH + name);
                if (prefab == null)
                {
                    Debug.LogError("can't load prefab " + FX_PATH + name);
                }else
                {
                    m_prefabCache[name] = prefab;
                }
            }else
            {
                prefab = m_prefabCache[name];
            }
            EffectObj effectObj = null;
            if (prefab != null)
            {
                var go = GameObject.Instantiate(prefab, pos, Quaternion.identity, parent);
                effectObj = go.GetComponent<EffectObj>();
                effectObj.id = m_currentId++;
                m_activeEffects.Add(effectObj.id, effectObj);
            }  
            return effectObj;
        }
    } 
}
