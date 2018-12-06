using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{

    public class FightHud : UIView
    {
        public Camera uiCamera;

        private FullScreenFadeInOut viewFadeInOut;
        private Core.Game game;

        private void Awake()
        {
            game = ClientGame.Instance.game;
            game.matchManager.onEvent += ProcessMatchEvent;        

            uiCamera = this.GetComponentInChildren<Camera>();

            viewFadeInOut = UIManager.Instance.AddView("FadeInOut", this.transform.Find("Canvas/Add")) as FullScreenFadeInOut;
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
                case Core.RoundState.Over:
                    viewFadeInOut.FadeOut();
                    break;
            }
        }
    }

}
