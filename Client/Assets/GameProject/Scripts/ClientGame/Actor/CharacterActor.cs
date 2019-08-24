using bluebean.Mugen3D.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = bluebean.UGFramework.Log.Debug;
using bluebean.Mugen3D.Core;

namespace bluebean.Mugen3D.ClientGame
{
    public class CharacterActor : ActorBase
    {
        private CommandComponent m_commandComponent;
        private CharacterAnimController3D m_animCtrl;

       public CharacterActor(GameObject prefab, GameObject parent, Entity character)
        {
            m_prefabInstance = GameObject.Instantiate(prefab);
            m_prefabInstance.transform.SetParent(parent.transform);
            Animation animation = m_prefabInstance.GetComponent<Animation>();
            if (animation != null)
            {
                m_animCtrl = new CharacterAnimController3D();
                m_animCtrl.Init(animation);
            }
            m_commandComponent = character.GetComponent<CommandComponent>();
        }

        public void UpdateAnim(string animName, float normalizedTime)
        {
            if (m_animCtrl != null)
            {
                m_animCtrl.UpdateAnimSample(animName, normalizedTime);
            }
        }

        protected override void OnTick()
        {
            GUIDebug.Instance.SetMsg(1, "ActiveCommands", m_commandComponent.GetActiveCommandName());
        }
    }
}
