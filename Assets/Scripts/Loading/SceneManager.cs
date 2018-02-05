using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSoul.UI;

public class SceneManager : MonoBehaviour {
    private SceneManager() { }
    public static SceneManager Instance {get{return m_instance;}}
    private static SceneManager m_instance;

    public void Awake()
    {
        m_instance = this;
        DontDestroyOnLoad(this.gameObject);
        foreach (var go in goScenes) {
            m_scenesPrefabs.Add(go.name, go);
        }
    }

    private Dictionary<string, GameObject> m_scenesIns = new Dictionary<string,GameObject>();
    private Dictionary<string, GameObject> m_scenesPrefabs = new Dictionary<string, GameObject>();
    public List<GameObject> goScenes = new List<GameObject>();
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

    public void LoadFightScene(PlayMode playMode, string p1CharacterName, string p2CharacterName, string stage) {
        RemoveScene("Scene_MainMenu");

        GameObject gameGo = new GameObject("Scene_Fight");
        var clientGame = gameGo.AddComponent<ClientGame>();
        clientGame.CreateGame(p1CharacterName, p1CharacterName, stage, playMode);
        clientGame.StartGame();

        curScene = gameGo;
        AddScene("Scene_Fight", gameGo);
        HideOtherScene("Scene_Fight");
        
    }

    public void LoadMainMenu()
    {
        var prefab = m_scenesPrefabs["Scene_MainMenu"];
        var goScene = GameObject.Instantiate(prefab);
        UIManager.Instance.PushView("ViewMainMenu", GameObject.Find("UIRoot/Canvas/BaseGroup").transform);

        curScene = goScene;
        AddScene("Scene_MainMenu", goScene);
        HideOtherScene("Scene_MainMenu");
    }

}
