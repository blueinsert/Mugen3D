using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bluebean.UGFramework;
using bluebean.UGFramework.UI;
using Debug = bluebean.UGFramework.Log.Debug;

namespace bluebean.Mugen3D
{
    public class Mugen3DGameLauncher : MonoBehaviour
    {
        private GameManager m_gameManager;

        protected virtual bool Initialize()
        {
            m_gameManager = GameManager.CreateAndInitializeInstance();
            if (m_gameManager == null)
            {
                Debug.LogError("GameManager CreateAndInitializeInstance Failed!");
                return false;
            }
            m_gameManager.StartLoadAllConfigData();
            Mugen3DUITaskRegister.RegisterUITasks();
            UIIntent intent = new UIIntent("MainMenuUITask");
            UIManager.Instance.StartUITask(intent);
            return true;
        }

        private void Awake()
        {
            Initialize();
        }

        protected virtual void Update()
        {
            if (m_gameManager != null)
            {
                m_gameManager.Tick();
            }
        }

        private void OnApplicationQuit()
        {
            if (m_gameManager != null)
            {
                m_gameManager.OnApplicationQuit();
            }
        }
    }
}
