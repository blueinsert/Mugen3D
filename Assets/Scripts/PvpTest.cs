using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D.Net;

public class PvpTest : MonoBehaviour {
    BattleNetClient m_battleClient;
    
	// Use this for initialization
	void Start () {
        m_battleClient = BattleNetClient.Instance;
        RegisterNetEvent();
	}

    void RegisterNetEvent()
    {
        m_battleClient.onRoomCreate += OnRoomCreate;
        m_battleClient.onMatchCreate += OnMatchCreate;
    }

    void UnRegisterNetEvent()
    {
        m_battleClient.onRoomCreate -= OnRoomCreate;
        m_battleClient.onMatchCreate -= OnMatchCreate;
    }

    void OnRoomCreate(int roomId)
    {
        Debug.Log("OnRoomCreate roomId:" + roomId);
    }

    void OnMatchCreate()
    {
        Debug.Log("OnMatchCreate");
        var multiGame = this.gameObject.AddComponent<Mugen3D.MultiPlayerGame>();
        multiGame.StartGame("Origin", "Origin", "Training", m_battleClient, 60, 60);
    }

	void Update () {
		
	}


    void OnGUI() {
        if (GUILayout.Button("CreateRoom"))
        {
            m_battleClient.Connect("127.0.0.1", 1234);
            m_battleClient.CreateRoom();
        }
        if (GUILayout.Button("JoinRoom"))
        {
            m_battleClient.Connect("127.0.0.1", 1234);
            m_battleClient.JoinRoom(0);
        }
    }
}
