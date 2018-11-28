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

        public void Play(Core.EffectDef def, UnitView view)
        {
            var effectObj = LoadEffectObj(def.name);
            if (effectObj != null)
            {
                effectObj.Init(m_currentId++, def, view);
                m_activeEffects.Add(effectObj.id, effectObj);
                effectObj.onFinish += (id) => {
                    m_activeEffects.Remove(id);
                    GameObject.Destroy(effectObj.gameObject);
                };
                effectObj.Play();
            }
        }

        EffectObj LoadEffectObj(string name)
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
                var go = GameObject.Instantiate(prefab);
                effectObj = go.GetComponent<EffectObj>();
            }  
            return effectObj;
        }
    } 
}
