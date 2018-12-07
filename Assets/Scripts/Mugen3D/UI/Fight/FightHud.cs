using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{

    public class FightHud : UIView
    {
        public Camera uiCamera;

        private FullScreenFadeInOut viewFadeInOut;
        private WidgetReadyGo viewReadyGo;
        private WidgetRoundDeclaration viewRoundDeclaration;
        private WidgetKO viewKO;
        private WidgetTimeOver viewTimeOver;
        private WidgetWinner viewWinner;

        private Core.Game game;

        private void Awake()
        {
            game = ClientGame.Instance.game;
            game.matchManager.onEvent += ProcessMatchEvent;        

            uiCamera = this.GetComponentInChildren<Camera>();

            viewFadeInOut = UIManager.Instance.AddView("FadeInOut", this.transform.Find("Canvas/Add")) as FullScreenFadeInOut;
            viewReadyGo = UIManager.Instance.AddView("ReadyGo", this.transform.Find("Canvas/Base/AnchorCenter")) as WidgetReadyGo;
            viewRoundDeclaration = UIManager.Instance.AddView("RoundDeclaration", this.transform.Find("Canvas/Base/AnchorCenter")) as WidgetRoundDeclaration;
            viewKO = UIManager.Instance.AddView("KO", this.transform.Find("Canvas/Base/AnchorCenter")) as WidgetKO;
            viewTimeOver = UIManager.Instance.AddView("TimeOver", this.transform.Find("Canvas/Base/AnchorCenter")) as WidgetTimeOver;
            viewWinner = UIManager.Instance.AddView("Winner", this.transform.Find("Canvas/Base/AnchorCenter")) as WidgetWinner;
        }

        private void OnMatchStart()
        {
            (UIManager.Instance.AddView("HitCount", this.transform.Find("Canvas/Base/AnchorCenterLeft")) as WidgetHitCount).SetInfo(game.matchManager.p1);
            (UIManager.Instance.AddView("HitCount", this.transform.Find("Canvas/Base/AnchorCenterRight")) as WidgetHitCount).SetInfo(game.matchManager.p2);
            (UIManager.Instance.AddView("LifeBar", this.transform.Find("Canvas/Base/AnchorTop")) as WidgetLifeBar).SetInfo(game.matchManager.p1, game.matchManager.p2);
        }

        private void OnMatchEnd()
        {

        }

        private void OnRoundStart(int roundNo)
        {

        }

        private void OnRoundEnd()
        {
            viewFadeInOut.FadeOut();
        }

        private void OnTimeOver()
        {
            SoundPlayer.Instance.Play("Snd_TimeOver");
            viewTimeOver.Play();
        }

        private void OnKO()
        {
            SoundPlayer.Instance.Play("Snd_KO");
            viewKO.Play();
        }

        private void ProcessMatchEvent(Core.Event evt)
        {
            switch (evt.type)
            {
                case Core.EventType.OnRoundStateChange:
                    ProcessRoundState((Core.RoundState)evt.data);
                    break;
                case Core.EventType.OnMatchStart:
                    OnMatchStart();break;
                case Core.EventType.OnMatchEnd:
                    OnMatchEnd();break;
                case Core.EventType.OnRoundStart:
                    OnRoundStart((int)evt.data);break;
                case Core.EventType.OnRoundEnd:
                    OnRoundEnd();break;
            }
        }

        private void ProcessRoundState(Core.RoundState state)
        {
            switch (state)
            {
                case Core.RoundState.PreIntro:
                    viewFadeInOut.FadeIn();
                    break;
                case Core.RoundState.RoundDeclare:
                    SoundPlayer.Instance.Play("Snd_Round" + (game.matchManager.roundNo + 1));
                    SoundPlayer.Instance.Play("Snd_ReadyGo", 1);
                    viewRoundDeclaration.Play(game.matchManager.roundNo + 1, ()=> {
                        viewReadyGo.Play();
                    });
                    break;
                case Core.RoundState.PreOver:
                    if (game.matchManager.roundTime <= 0)
                        OnTimeOver();
                    else
                        OnKO();
                    break;
                case Core.RoundState.Over:
                    viewWinner.Play();
                    SoundPlayer.Instance.Play("Snd_Winner");
                    break;
            }
        }
    }

}
