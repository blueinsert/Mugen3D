using UnityEngine;

namespace bluebean.UGFramework
{
    public enum SceneLayerType
    {
        ThreeD,
        UI,
    }

    public enum SceneLayerState
    {
        Using,
        Unused,
        Loading,
        WaitForFree,
    }

    public abstract class SceneLayer : MonoBehaviour
    {
        public GameObject PrefabInstance
        {
            get { return m_prefabInstance; }
        }

        public SceneLayerState State { get; set; }
        public string m_name; 
        private GameObject m_prefabInstance;

        public void SetName(string name)
        {
            m_name = name;
        }

        public void AttachGameObject(GameObject go)
        {
            m_prefabInstance = go;
            go.transform.SetParent(this.transform, false);
        }

        public GameObject FindGameObject(string path)
        {
            GameObject root = m_prefabInstance;
            GameObject target = m_prefabInstance;
            int index = path.IndexOf("/");
            if (index != -1)
            {
                string subPath = path.Substring(index + 1);
                target = root.transform.Find(subPath).gameObject;
            }
            return target;
        }
    }
}
