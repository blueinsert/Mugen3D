using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSoul.UI
{

    public class LuaView : LuaMonoBehaviour
    {
        public string viewName = "unset";
        private bool m_interactable = true;
        private bool m_isInited = false;

        public bool interactable { get { return m_interactable; } set { m_interactable = value; } }
        public bool isInStack = false;
         

        private MonoBehaviourEvent m_processKeyboardInput;

        public override void Init()
        {
            base.Init();
            m_processKeyboardInput = m_luaBehaviour.Get<string, MonoBehaviourEvent>("ProcessKeyboardInput");
            m_isInited = true;
        }

        protected override void Update()
        {
            if (!m_isInited)
                return;
            if (m_interactable)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    this.Close();
                    return;
                }
                if (m_processKeyboardInput != null)
                {
                    m_processKeyboardInput(m_luaBehaviour);
                }
            }
            base.Update();
        }

        public void Close()
        {
            if (isInStack)
            {
                UIManager.Instance.PopView(this);
            }
            GameObject.Destroy(this.gameObject);
        }

    }
}
