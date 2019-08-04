using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bluebean.UGFramework.UI;

namespace bluebean.UGFramework
{
    public class EasyGameObjectPool
    {
        private GameObject m_prefab;
        private GameObject m_root;

        private int m_curIndex;

        private List<GameObject> m_instances = new List<GameObject>();

        public void Setup(GameObject prefab, GameObject root)
        {
            m_prefab = prefab;
            m_root = root;
        }

        public void Deactive()
        {
            foreach (var ins in m_instances)
            {
                ins.SetActive(false);
            }
            m_curIndex = 0;
        }

        public GameObject Allocate()
        {
            foreach (var instance in m_instances)
            {
                if (!instance.gameObject.activeSelf)
                {
                    instance.gameObject.SetActive(true);
                    return instance;
                }
            }

            GameObject go = GameObject.Instantiate(m_prefab);
            go.transform.SetParent(m_root.transform, false);
            m_instances.Add(go);
            go.SetActive(true);
            return go;
        }

    }

    public class EasyGameObjectPool<T> where T : MonoViewController
    {
        private GameObject m_prefab;
        private GameObject m_root;

        private int m_curIndex;

        private List<T> m_instances = new List<T>();

        public void Setup(GameObject prefab, GameObject root)
        {
            m_prefab = prefab;
            m_root = root;
        }

        public void Deactive()
        {
            foreach (var ins in m_instances)
            {
                ins.gameObject.SetActive(false);
            }
            m_curIndex = 0;
        }

        public T Allocate(out bool isNew)
        {
            foreach (var instance in m_instances)
            {
                if (!instance.gameObject.activeSelf)
                {
                    isNew = false;
                    instance.gameObject.SetActive(true);
                    return instance;
                }
            }

            GameObject go = GameObject.Instantiate(m_prefab);
            go.transform.SetParent(m_root.transform, false);
            MonoViewController.AttachViewControllerToGameObject(go, "./", typeof(T).FullName, true);
            isNew = true;
            var ins = go.GetComponent<T>();
            m_instances.Add(ins);
            ins.gameObject.SetActive(true);
            return ins;
        }

    }
}
