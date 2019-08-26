using System.Collections;
using System.Collections.Generic;
using bluebean.Mugen3D.Core;
using UnityEngine;
using Debug = bluebean.UGFramework.Log.Debug;

namespace bluebean.Mugen3D.ClientGame
{
    public partial class ClientBattleWorld
    {
        public void CreateCameraController(CameraComponent cameraComponent)
        {
            Debug.Log("ClientBattleWorld:CreateCameraController");
            m_cameraController = new CameraController();
            m_cameraController.Init(cameraComponent, m_sceneRoot.transform.Find("BattleCamera").GetComponent<Camera>());
        }

        public void CreateCharacterActor(Entity character)
        {
            var playerComponent = character.GetComponent<PlayerComponent>();
            CharacterActor characterActor = new CharacterActor(GetAsset<GameObject>(playerComponent.Config.Prefab), m_sceneRoot.transform.Find("PlayerRoot").gameObject, character);
            m_characterActorDic.Add(playerComponent.Index, characterActor);
        }
    }
}