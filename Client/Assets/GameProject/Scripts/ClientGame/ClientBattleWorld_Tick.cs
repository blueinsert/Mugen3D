using bluebean.Mugen3D.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace bluebean.Mugen3D.ClientGame
{
    public partial class ClientBattleWorld
    {
        private void UpdatePlayerInputCodes()
        {
            for (int i = 0; i < m_playerInputMapConfigList.Count; i++)
            {
                var inputMapDic = m_playerInputMapConfigList[i];
                int keycode = 0;
                foreach (var inputPair in inputMapDic)
                {
                    if (Input.GetKey(inputPair.Value))
                    {
                        keycode = keycode | Utility.GetKeycode(inputPair.Key);
                    }
                }
                m_playerInputCodes[i] = keycode;
            }
        }

        public virtual void Tick()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                this.isPuase = !this.isPuase;
            }
            if (isPuase)
            {
                if (Input.GetKeyDown(KeyCode.N))
                {
                    OnTick();
                }
            }
            else
            {
                OnTick();
            }

        }

        protected virtual void OnTick()
        {
            UpdatePlayerInputCodes();
            m_gameTimeResidual += UnityEngine.Time.deltaTime;
            while (m_gameTimeResidual > m_gameDeltaTime)
            {
                m_gameTimeResidual -= m_gameDeltaTime;
                Step();
            }
            TickGraphics(UnityEngine.Time.deltaTime);
        }

        /// <summary>
        /// 驱动表现层
        /// </summary>
        private void TickGraphics(float deltaTime)
        {
            foreach (var characterActor in m_characterActorList)
            {
                characterActor.TickGraphic(deltaTime);
            }

            if (m_cameraController != null)
            {
                m_cameraController.Tick();
            }
        }

        /// <summary>
        /// 驱动内核数据层
        /// </summary>
        protected void Step()
        {
            m_battleWorld.UpdatePlayerInput(m_playerInputCodes);
            m_battleWorld.Step();
        }
    }
}
