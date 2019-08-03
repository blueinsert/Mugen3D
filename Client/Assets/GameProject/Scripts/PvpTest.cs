using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D.Net;

public class PvpTest : MonoBehaviour {
    BattleNetClient m_battleClient;
    bool hasCreateGame = false;
	// Use this for initialization
	void Start () {
        m_battleClient = BattleNetClient.Instance;
        RegisterNetEvent();
	}

    void RegisterNetEvent()
    {
        m_battleClient.onMatchCreate += OnMatchCreate;
    }

    void UnRegisterNetEvent()
    {
        m_battleClient.onMatchCreate -= OnMatchCreate;
    }

    void OnMatchCreate()
    {
        Debug.Log("OnMatchCreate");
        var multiGame = this.gameObject.AddComponent<Mugen3D.MultiPlayerGame>();
        multiGame.StartGame("Origin", "Origin", "Training", m_battleClient, 60, 60);
        hasCreateGame = true;
    }

    void Update()
    {
    }

    void OnGUI() {
        if (!hasCreateGame)
        {
            if (GUILayout.Button("Connect"))
            {
                m_battleClient.Connect("127.0.0.1", 1234);
            }
            if (GUILayout.Button("FindMatch"))
            {
                m_battleClient.FindMatch();
            }
            if (GUILayout.Button("CancelFindMatch"))
            {
                m_battleClient.CancelFindMatch();
            }
        }
    }
}
