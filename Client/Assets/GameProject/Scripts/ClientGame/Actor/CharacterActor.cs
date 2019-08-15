using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = bluebean.UGFramework.Log.Debug;

namespace bluebean.Mugen3D.ClientGame
{
    public class CharacterActor : ActorBase
    {

        private CharacterAnimController3D m_animCtrl;

       public CharacterActor(GameObject prefab, GameObject parent)
        {
            m_prefabInstance = GameObject.Instantiate(prefab);
            m_prefabInstance.transform.SetParent(parent.transform);
            Animation animation = m_prefabInstance.GetComponent<Animation>();
            if (animation != null)
            {
                m_animCtrl = new CharacterAnimController3D();
                m_animCtrl.Init(animation);
            }
        }

        public void UpdateAnim(string animName, float normalizedTime)
        {
            if (m_animCtrl != null)
            {
                m_animCtrl.UpdateAnimSample(animName, normalizedTime);
            }
        }
    }
}
