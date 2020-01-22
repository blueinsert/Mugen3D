using System.Collections;
using System.Collections.Generic;
using bluebean.Mugen3D.Core;
using UnityEngine;
using Debug = bluebean.UGFramework.Log.Debug;

namespace bluebean.Mugen3D.ClientGame
{
    public partial class ClientBattleWorld
    {
        public void CreateCameraController(CameraComponent cameraComponent, Camera camera)
        {
            Debug.Log("ClientBattleWorld:CreateCameraController");
            m_cameraController = new CameraController();
            m_cameraController.Init(cameraComponent, camera);
        }

        public void CreateCharacterActor(Entity character)
        {
            var playerComponent = character.GetComponent<PlayerComponent>();
            CharacterActor characterActor = new CharacterActor(GetAsset<GameObject>(playerComponent.Config.Prefab), m_playerRoot, character);
            m_characterActorList.Add(characterActor);
        }

        public void CreateStage(string prefabName)
        {
            GameObject.Instantiate(GetAsset<GameObject>(prefabName), m_stageRoot.transform, false);
        }
    }
}