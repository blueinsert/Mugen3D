using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>  {

    private void ShowLoading() {
        SceneManager.LoadScene("Loading");
    }

    IEnumerator WaitforLoadComplete(AsyncOperation oper, Action onComplete)
    {
        long startTime = DateTime.Now.Ticks;
        while (!oper.isDone) {
            yield return new WaitForEndOfFrame();
        }
        onComplete();
        long endtTime = DateTime.Now.Ticks;
        Debug.Log("fightScene load consume time:" + (endtTime - startTime)/10000 + "ms");
        yield return null;
    }

    public  void LoadFightScene(PlayMode playMode, string p1CharacterName, string p2CharacterName, string stage) {
        //ShowLoading();
        var loadOper = SceneManager.LoadSceneAsync("FightScene", LoadSceneMode.Single);
        StartCoroutine(WaitforLoadComplete(loadOper, () =>
        {
            GameObject gameGo = new GameObject();
            gameGo.name = "ClientGame";
            var clientGame = gameGo.AddComponent<ClientGame>();
            clientGame.StartGame(p1CharacterName, p2CharacterName, stage, playMode);
        }));
        
    }

}
