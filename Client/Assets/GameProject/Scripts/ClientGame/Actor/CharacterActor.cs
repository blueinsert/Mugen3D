using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = bluebean.UGFramework.Log.Debug;
using bluebean.Mugen3D.Core;
using bluebean.UGFramework.ConfigData;

namespace bluebean.Mugen3D.ClientGame
{
    /// <summary>
    /// 角色对应的演员对象
    /// </summary>
    public class CharacterActor
    {
        private CharacterGraphic m_graphic;

        private TransformComponent m_transformComponent;
        private BasicInfoComponent m_playerComponent;
        private CommandComponent m_commandComponent;
        private MoveComponent m_moveComponent;
        private AnimationComponent m_animComponent;
        private FSMComponent m_fsmComponent;
        private CollideComponent m_colliderComponent;

       public CharacterActor(GameObject prefab, GameObject parent, Entity character)
        {
            m_graphic = new CharacterGraphic();
            m_graphic.Init(prefab, parent);
            //初始化组件
            m_transformComponent = character.GetComponent<TransformComponent>();
            m_commandComponent = character.GetComponent<CommandComponent>();
            m_playerComponent = character.GetComponent<BasicInfoComponent>();
            m_moveComponent = character.GetComponent<MoveComponent>();
            m_animComponent = character.GetComponent<AnimationComponent>();
            m_fsmComponent = character.GetComponent<FSMComponent>();
            m_colliderComponent = character.GetComponent<CollideComponent>();
        }

        public void TickGraphic(float deltaTime)
        {
            if (m_graphic.Facing != m_transformComponent.Facing)
            {
                m_graphic.SetFacing(m_transformComponent.Facing);
            }
            m_graphic.UpdateAnimSample(m_animComponent.CurAction.animName, m_animComponent.CurActionFrame.normalizeTime.AsFloat());
            m_graphic.SetPosition(m_transformComponent.Position.x.AsFloat(),m_transformComponent.Position.y.AsFloat());
            TickDebug();
        }

        protected  void TickDebug()
        {
            GUIDebug.Instance.SetMsg(m_playerComponent.Index, "StateNo", m_fsmComponent.StateNo.ToString());
            GUIDebug.Instance.SetMsg(m_playerComponent.Index, "StateTime", m_fsmComponent.StateTime.ToString());
            GUIDebug.Instance.SetMsg(m_playerComponent.Index, "ActiveCommands", m_commandComponent.GetActiveCommandName());
            GUIDebug.Instance.SetMsg(m_playerComponent.Index, "Pos", m_transformComponent.Position.ToString());
            GUIDebug.Instance.SetMsg(m_playerComponent.Index, "Vel", m_moveComponent.Velocity.ToString());
            GUIDebug.Instance.SetMsg(m_playerComponent.Index, "Acceler", m_moveComponent.Acceler.ToString());
            GUIDebug.Instance.SetMsg(m_playerComponent.Index, "Facing", m_transformComponent.Facing.ToString());
            GUIDebug.Instance.SetMsg(m_playerComponent.Index, "Anim", m_animComponent.Anim.ToString());
            GUIDebug.Instance.SetMsg(m_playerComponent.Index, "AnimName", m_animComponent.CurAction.animName);
            GUIDebug.Instance.SetMsg(m_playerComponent.Index, "AnimTime", m_animComponent.AnimTime.ToString());
            GUIDebug.Instance.SetMsg(m_playerComponent.Index, "AnimElem", m_animComponent.AnimElem.ToString());
            GUIDebug.Instance.SetMsg(m_playerComponent.Index, "AnimElemTime", m_animComponent.AnimElemTime.ToString());
            GUIDebug.Instance.SetMsg(m_playerComponent.Index, "LeftAnimTime", m_animComponent.LeftAnimTime.ToString());
            var coll = m_colliderComponent.Collider;
            for(int i = 0;i< coll.AttackClsnsLength; i++)
            {
                var clsn = coll.AttackClsns[i];
                Debug.DrawRect(clsn.LeftDown.ToVector2(), clsn.RightDown.ToVector2(), clsn.RightUp.ToVector2(), clsn.LeftUp.ToVector2(), Color.red);
            }
            for (int i = 0; i < coll.DefenceClsnsLength; i++)
            {
                var clsn = coll.DefenceClsns[i];
                Debug.DrawRect(clsn.LeftDown.ToVector2(), clsn.RightDown.ToVector2(), clsn.RightUp.ToVector2(), clsn.LeftUp.ToVector2(), Color.blue);
            }
            for (int i = 0; i < coll.CollideClsnsLength; i++)
            {
                var clsn = coll.CollideClsns[i];
                Debug.DrawRect(clsn.LeftDown.ToVector2(), clsn.RightDown.ToVector2(), clsn.RightUp.ToVector2(), clsn.LeftUp.ToVector2(), Color.black);
            }
        }
    }
}
