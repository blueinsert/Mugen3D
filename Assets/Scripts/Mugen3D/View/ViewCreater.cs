using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class ViewCreater 
    {
        private delegate EntityView DelegateViewCreater(Core.Entity entity, Transform parent);

        private static Dictionary<Type, DelegateViewCreater> m_viewCreater = new Dictionary<Type, DelegateViewCreater>() {
            {typeof(Core.Character), CreateCharacterView},
            {typeof(Core.Helper), CreateHelperView},
        };

        private static EntityView CreateCharacterView(Core.Entity entity, Transform parent)
        {
            var c = entity as Core.Character;
            UnityEngine.Object prefab = ResourceLoader.Load((c.config as Core.UnitConfig).prefab);
            GameObject go = GameObject.Instantiate(prefab, parent) as GameObject;
            var view = go.AddComponent<CharacterView>();
            view.Init(c);
            return view;
        }

        private static EntityView CreateHelperView(Core.Entity entity, Transform parent)
        {
            var h = entity as Core.Helper;
            UnityEngine.Object prefab = ResourceLoader.Load((h.config as Core.HelperConfig).prefab);
            GameObject go = GameObject.Instantiate(prefab, parent) as GameObject;
            var view = go.AddComponent<UnitView>();
            view.Init(h);
            return view;
        }

        public static EntityView CreateView(Core.Entity entity, Transform parent)
        {
            DelegateViewCreater creater;
            EntityView view = null;
            if (m_viewCreater.TryGetValue(entity.GetType(), out creater))
            {
                view = creater(entity, parent);  
            }
            return view;
        }
    }
}
