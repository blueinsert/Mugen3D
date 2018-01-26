using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightSceneLoadWorker {

    private bool isLoadingComplete = false;
    private ClientGame clientGame;

    public IEnumerator CreateGame(PlayMode playMode, string p1CharacterName, string p2CharacterName, string stage)
    {
        GameObject gameGo = new GameObject();
        gameGo.name = "ClientGame";
        var clientGame = gameGo.AddComponent<ClientGame>();
        clientGame.StartGame(p1CharacterName, p2CharacterName, stage, playMode);
        this.clientGame = clientGame;
        SetEnabled<Camera>(clientGame.gameObject, false);
        SetEnabled<AudioListener>(clientGame.gameObject, false);
        isLoadingComplete = true;
        yield return null;
    }

    public IEnumerator WaitForLoadingComplete(System.Action onComplete)
    {
        float timer = 0;
        while (timer < 1 || isLoadingComplete == false)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        SetEnabled<Camera>(clientGame.gameObject, true);
        SetEnabled<AudioListener>(clientGame.gameObject, true);
        onComplete();
        yield return null;
    }

    private void SetEnabled<T>(GameObject go, bool isEnabled) where T : Behaviour
    {
        foreach (T t in go.GetComponentsInChildren<T>())
        {
            t.enabled = isEnabled;
        }
    }

}
