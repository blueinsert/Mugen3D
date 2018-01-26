using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>  {

    IEnumerator DoLoadingFightScene(PlayMode playMode, string p1CharacterName, string p2CharacterName, string stage) {
        SceneManager.LoadScene("Loading");
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene("FightScene", LoadSceneMode.Additive);
        yield return new WaitForEndOfFrame();
        var loadingScene = SceneManager.GetSceneByName("Loading");
        var fightScene = SceneManager.GetSceneByName("FightScene");
        SceneManager.SetActiveScene(fightScene);
        FightSceneLoadWorker worker = new FightSceneLoadWorker();
        StartCoroutine(worker.CreateGame(playMode, p1CharacterName, p2CharacterName, stage));
        StartCoroutine(worker.WaitForLoadingComplete(() => {
            SceneManager.UnloadScene(loadingScene);
        }));
    }
   

    public void LoadFightScene(PlayMode playMode, string p1CharacterName, string p2CharacterName, string stage) {
        StartCoroutine(DoLoadingFightScene(playMode, p1CharacterName, p2CharacterName, stage));
    }

}
