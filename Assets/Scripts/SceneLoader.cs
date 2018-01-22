using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightStartInfo
{
    public string stage;
    public string p1CharacterName;
    public string p2CharacterName;
    public PlayMode playMode;

    public FightStartInfo(string stage, string p1Name, string p2Name, PlayMode playMode)
    {
        this.stage = stage;
        this.p1CharacterName = p1Name;
        this.p2CharacterName = p2Name;
        this.playMode = playMode;
    }
}

public class SceneLoader  {

    private static FightStartInfo fightStartInfo;

    private static void OnFightSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject gameGo = new GameObject();
        gameGo.name = "ClientGame";
        var clientGame = gameGo.AddComponent<ClientGame>();
        clientGame.StartGame(fightStartInfo.p1CharacterName, fightStartInfo.p2CharacterName, fightStartInfo.stage);
        fightStartInfo = null;
        SceneManager.sceneLoaded -= OnFightSceneLoaded;
    }

    public static void LoadFightScene(PlayMode playMode, string p1CharacterName, string p2CharacterName, string stage) {
        fightStartInfo = new FightStartInfo(stage, p1CharacterName, p2CharacterName, playMode);
        SceneManager.sceneLoaded += OnFightSceneLoaded;
        SceneManager.LoadScene("FightScene");
    }

}
