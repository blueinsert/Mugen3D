using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace bluebean.Mugen3D.ClientGame
{
    /// <summary>
    /// 角色的可视化代表
    /// </summary>
    public class CharacterGraphic
    {
        public int Facing { get { return m_prefabInstance.transform.localScale.x > 0 ? 1 : -1; } }

        private GameObject m_parent;
        private GameObject m_prefabInstance;
        private Animation m_animation;

        public void Init(GameObject prefab, GameObject parent)
        {
            m_parent = parent;
            m_prefabInstance = GameObject.Instantiate(prefab, parent.transform, false);
            m_prefabInstance.transform.localScale = Vector3.one;
            m_prefabInstance.transform.position = Vector3.zero;
            m_animation = m_prefabInstance.GetComponent<Animation>();
            foreach (AnimationState state in m_animation)
            {
                state.enabled = false;
            }
        }

        public void UpdateAnimSample(string animName, float normalizedTime)
        {
            m_animation[animName].enabled = true;
            m_animation[animName].normalizedTime = normalizedTime;
            m_animation[animName].weight = 1;
            m_animation.Sample();
            m_animation[animName].enabled = false;
        }

        public void SetPosition(float x, float y)
        {
            m_prefabInstance.transform.position = new Vector3(x, y, 0);
        }

        public void SetFacing(int facing)
        {
            m_prefabInstance.transform.localScale = new Vector3(facing > 0 ? 1 : -1, 1, 1);
        }
    }
}
