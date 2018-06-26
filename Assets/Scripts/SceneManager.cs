using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSoul.UI;
using Mugen3D;

public enum LoadType { 
    Load,
    Create,
}

public enum LoadMode
{
    Single,
    Additive,
}

public class SceneManager : MonoBehaviour {
    private SceneManager() { }
    public static SceneManager Instance {get{return m_instance;}}
    private static SceneManager m_instance;

    public void Awake()
    {
        m_instance = this;
        DontDestroyOnLoad(this.gameObject);
        foreach (var go in prefabScenes) {
            m_scenesPrefabs.Add(go.name, go);
        }
    }

    private Dictionary<string, GameObject> m_scenesIns = new Dictionary<string,GameObject>();
    private Dictionary<string, GameObject> m_scenesPrefabs = new Dictionary<string, GameObject>();
    public List<GameObject> prefabScenes = new List<GameObject>();
    public GameObject curScene;

    public void AddScene(string name, GameObject scene)
    {
        m_scenesIns[name] = scene;
    }

    public void RemoveScene(string name)
    {
        if (m_scenesIns.ContainsKey(name))
        {
            GameObject.Destroy(m_scenesIns[name]);
            m_scenesIns.Remove(name);
        }
    }

    public void RemoveAllScene()
    {
        foreach (var kv in m_scenesIns)
        {
            GameObject.Destroy(kv.Value);
        }
        m_scenesIns.Clear();
    }

    public void HideOtherScene(string curSceneName)
    {
        foreach (var kv in m_scenesIns)
        {
            if (kv.Key != curSceneName)
            {
                kv.Value.SetActive(false);
            }
        }
    }

    public void HideAllScene()
    {
        foreach (var kv in m_scenesIns)
        {
            kv.Value.SetActive(false);
        }
    }

    private void LoadScene(LoadType type, string sceneName, Action<GameObject> onBeginLoad, Action<GameObject> onFinishLoad, LoadMode mode = LoadMode.Single) {
        if (mode == LoadMode.Single)
        {
            RemoveAllScene();
        }
        GameObject scene = null;
        if (type == LoadType.Create)
        {
            scene = new GameObject(sceneName);
        }
        else if (type == LoadType.Load)
        {
            var prefab = m_scenesPrefabs[sceneName];
            scene = GameObject.Instantiate(prefab);
        }
        if (onBeginLoad != null)
        {
            onBeginLoad(scene);
        }
        curScene = scene;
        AddScene(sceneName, scene);
        if (onFinishLoad != null)
        {
            onFinishLoad(scene);
        }
    }

    IEnumerator DoLoadSceneAsync(LoadType type, string sceneName, Action<GameObject> onBeginLoad, Action<GameObject> onFinishLoad, LoadMode mode = LoadMode.Single) {
        if (mode == LoadMode.Single)
        {
            RemoveAllScene();
        }
        GameObject scene = null;
        if (type == LoadType.Create)
        {
            scene = new GameObject(sceneName);
        }
        else if (type == LoadType.Load)
        {
            var prefab = m_scenesPrefabs[sceneName];
            scene = GameObject.Instantiate(prefab);
        }
        if (onBeginLoad != null)
        {
            onBeginLoad(scene);
        }
        curScene = scene;
        AddScene(sceneName, scene);
        yield return new WaitForSeconds(0.5f);
        if (onFinishLoad != null)
        {
            onFinishLoad(scene);
        }
        yield return null;
    }

    private void LoadSceneAsync(LoadType type, string sceneName, Action<GameObject> onBeginLoad, Action<GameObject> onFinishLoad, LoadMode mode = LoadMode.Single)
    {
        StartCoroutine(DoLoadSceneAsync(type, sceneName, onBeginLoad, onFinishLoad, mode));
    }

    public void LoadFightScene(Mugen3D.PlayMode playMode, string p1CharacterName, string p2CharacterName, string stage) {
        LoadScene(LoadType.Load, "Scene_Loading", null, null);
        LoadSceneAsync(LoadType.Load, "Scene_Fight", 
            (scene) => {
                scene.SetActive(false);
                var clientGame = scene.AddComponent<ClientGame>();
                clientGame.CreateGame(p1CharacterName, p1CharacterName, stage, playMode);
               
            }, 
            (scene) => {
                RemoveScene("Scene_Loading");
                scene.SetActive(true);
                //scene.GetComponent<ClientGame>().StartGame();
            },
            LoadMode.Additive
        );       
    }

    public void LoadMainMenu()
    {
        LoadScene(LoadType.Load, "Scene_MainMenu",
            (scene) => { 
                UIManager.Instance.PushView("ViewMainMenu", GameObject.Find("UIRoot/Canvas/BaseGroup").transform); 
            }, 
            null
        );
    }

    /*
    private void SetEnabled<T>(GameObject go, bool isEnabled) where T : Behaviour
    {
        foreach (T t in go.GetComponentsInChildren<T>())
        {
            t.enabled = isEnabled;
        }
    }
    */
}
