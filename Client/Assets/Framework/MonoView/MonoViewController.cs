using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Debug = bluebean.UGFramework.Log.Debug;

namespace bluebean.UGFramework
{
    public class MonoViewController : MonoBehaviour
    {
        public static MonoViewController AttachViewControllerToGameObject(GameObject root, string path, string typeFullName ,bool execAutoBind = false)
        {
            GameObject target = FindChild(root, path);
            Type type = ClassLoader.GetType(typeFullName);
            if(type == null)
            {
                Debug.LogError(string.Format("ClassLoader.GetType \"{0}\" is null", typeFullName));
            }
            MonoViewController viewController = null;
            if((viewController = target.GetComponent(type) as MonoViewController) != null)
            {
                return viewController;
            }
            else
            {
                viewController = target.AddComponent(type) as MonoViewController;
                if (execAutoBind)
                    viewController.AutoBindFields();
                return viewController;
            }
        }

        private static GameObject FindChild(GameObject root, string path)
        {
            GameObject target = root;
            int index = path.IndexOf("/");
            if (index != -1)
            {
                string subPath = path.Substring(index + 1);
                target = root.transform.Find(subPath).gameObject;
            }
            return target;
        }

        public GameObject FindChild(string path)
        {
            return FindChild(this.gameObject, path);
        }

        public void AutoBindFields()
        {
            var type = this.GetType();
            foreach (var fieldInfo in type.GetFields(BindingFlags.NonPublic|BindingFlags.Public | BindingFlags.Instance))
            {
                var autoBindAttribute = fieldInfo.GetCustomAttribute<AutoBindAttribute>();
                if (autoBindAttribute != null)
                {
                    var obj = BindFiledImpl(fieldInfo.FieldType, autoBindAttribute.path);
                    if (obj != null)
                    {
                        fieldInfo.SetValue(this, obj);
                    }
                    else
                    {
                        Debug.LogError(string.Format("BindFiledImpl error {0} path:{1}", type.Name, autoBindAttribute.path));
                    }
                }
            }
            OnBindFieldsComplete();
        }

        private UnityEngine.Object BindFiledImpl(Type espectType, string path)
        {
            GameObject target = FindChild(path);
            if(target == null)
            {
                return null;
            }
            if (espectType == typeof(GameObject)){
                return target;
            }else if (espectType.IsSubclassOf(typeof(Component)))
            {
                var component = target.GetComponent(espectType);
                return component as UnityEngine.Object;
            }
            return null;
        }

        protected virtual void OnBindFieldsComplete()
        {

        }

        

    }
}
