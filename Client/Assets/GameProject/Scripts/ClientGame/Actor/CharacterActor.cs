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

        private PlayerComponent m_playerComponent;
        private CommandComponent m_commandComponent;
        private MoveComponent m_moveComponent;
        private FSMComponent m_fsmComponent;

       public CharacterActor(GameObject prefab, GameObject parent, Entity character)
        {
            m_graphic = new CharacterGraphic();
            m_graphic.Init(prefab, parent);
            //初始化组件
            m_commandComponent = character.GetComponent<CommandComponent>();
            m_playerComponent = character.GetComponent<PlayerComponent>();
            m_moveComponent = character.GetComponent<MoveComponent>();
            //m_fsmComponent = character.GetComponent<FSMComponent>();
        }

        public void TickGraphic(float deltaTime)
        {
            m_graphic.SetPosition(m_moveComponent.Position.x.AsFloat(),m_moveComponent.Position.y.AsFloat());
            TickDebug();
        }

        protected  void TickDebug()
        {
            //GUIDebug.Instance.SetMsg(m_playerComponent.Index, "StateNo", m_fsmComponent.StateNo.ToString());
            //GUIDebug.Instance.SetMsg(m_playerComponent.Index, "StateTime", m_fsmComponent.StateTime.ToString());
            GUIDebug.Instance.SetMsg(m_playerComponent.Index, "ActiveCommands", m_commandComponent.GetActiveCommandName());
            GUIDebug.Instance.SetMsg(m_playerComponent.Index, "Pos", m_moveComponent.Position.ToString());
            GUIDebug.Instance.SetMsg(m_playerComponent.Index, "Vel", m_moveComponent.Velocity.ToString());
        }
    }
}
