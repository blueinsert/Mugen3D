using System.Collections;
using System.Collections.Generic;
using bluebean.UGFramework.ConfigData;
using bluebean.UGFramework.UI;
using Debug = bluebean.UGFramework.Log.Debug;

namespace bluebean.UGFramework
{
    public class GameManager : ITickable
    {
        private GameManager() { }
        public static GameManager m_instance;
        public static GameManager Instance {
            get {
                return m_instance;
            }
        }

        public static GameManager CreateAndInitializeInstance()
        {
            var gameManager = new GameManager();
            if (gameManager.Intialize())
            {
                m_instance = gameManager;
                return m_instance;
            }
            else
            {
                Debug.LogError("GameManager Initialize Failed!");
                return null;
            }
        }

        private CoroutineScheduler m_coroutineHelper;
        private ConfigDataLoader m_configDataLoader;
        private AssetLoader m_assetLoader;
        private TaskManager m_taskManager;
        private UIManager m_uiManager;
        private SceneTree m_sceneTree;

        public ConfigDataLoader ConfigDataLoader { get { return m_configDataLoader; } }

        public CoroutineScheduler CoroutineHelper { get { return m_coroutineHelper; } }
        public TaskManager TaskManager { get { return m_taskManager; } }
        public AssetLoader ResourceManager { get { return m_assetLoader; } }
        public UIManager UIManager { get { return m_uiManager; } }
        public SceneTree SceneTree { get { return m_sceneTree; } }

        public bool Intialize()
        {
            m_configDataLoader = ConfigDataLoader.CreateInstance();
            m_coroutineHelper = new CoroutineScheduler();
            m_assetLoader = AssetLoader.CreateInstance();
            m_taskManager = TaskManager.CreateInstance();
            m_uiManager = UIManager.CreateInstance();
            m_sceneTree = SceneTree.CreateInstance();
            if (!m_sceneTree.Initialize())
            {
                Debug.LogError("SceneTree Initialize Failed");
                return false;
            }
            return true;
        }

        public void Deintialize()
        {
            //todo
        }

        public void Tick()
        {
            if (m_coroutineHelper != null)
            {
                m_coroutineHelper.Tick();
            }
            if (m_assetLoader != null)
            {
                m_assetLoader.Tick();
            }
            if(m_taskManager != null)
            {
                m_taskManager.Tick();
            }
            if (m_sceneTree != null)
            {
                m_sceneTree.Tick();
            }
        }

        public void StartLoadAllConfigData()
        {
            m_coroutineHelper.StartCorcoutine(m_configDataLoader.LoadAllConfigData((res) => {
                if (!res)
                {
                    Debug.Log("ConfigDataLoader Init Error");
                }
            }));
        }
    }
}
