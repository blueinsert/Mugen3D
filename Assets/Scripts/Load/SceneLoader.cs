using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>  {

    IEnumerator WaitForLoadingComplete(AsyncOperation oper, System.Action onComplete)
    {
        float timer = 0;
        while (timer < 1 || oper.isDone == false)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        onComplete();
        yield return null;
    }

    public  void LoadFightScene(PlayMode playMode, string p1CharacterName, string p2CharacterName, string stage) {
       var oper = SceneManager.LoadSceneAsync("FightScene", LoadSceneMode.Single);
       StartCoroutine(WaitForLoadingComplete(oper, () => {
           var loader = GameObject.FindObjectOfType<FightSceneLoader>();
           loader.Loading(playMode, p1CharacterName, p2CharacterName, stage);
       }));
       
    }

}
