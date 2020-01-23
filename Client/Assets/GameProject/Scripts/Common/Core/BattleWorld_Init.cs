using System.Collections;
using System.Collections.Generic;
using UniLua;

namespace bluebean.Mugen3D.Core
{
    public partial class BattleWorld : WorldBase
    {

        public static void SetFileReader(CustomFileReader reader)
        {
            sFileReader = reader;
        }

        public static void SetLogDelegate(LogDelegate logDelegate, LogDelegate logWarn, LogDelegate logError) {
            Debug.m_Log = logDelegate;
            Debug.m_LogWarn = logWarn;
            Debug.m_LogError = logError;
        }

        
        public static void SetLuaFileLoader(CustomFileLoader luaFileLoader)
        {
            UniLua.LuaFile.ClearLoaders();
            UniLua.LuaFile.AddLoader(luaFileLoader);
        }

        /// <summary>
        /// 初始化单例组件(component)和系统(system)
        /// </summary>
        private void InitializeBattleWorld()
        {
            CommandComponent.StaticInit(m_commandConfigs);
            //创建单例组件
            //m_matchComponent = AddSingletonComponent<MatchComponent>();
            AddSingletonComponent<StageComponent>().Init(m_stageConfig);
            m_listener.OnCreateStage(m_stageConfig.Prefab);
            var cameraComponent = AddSingletonComponent<CameraComponent>().Init(m_cameraConfig);
            m_listener.OnCameraCreate(cameraComponent);
            m_inputComponent = AddSingletonComponent<InputComponent>();
            //创建所有系统
            AddSystem<CameraSystem>();
            AddSystem<CommandSystem>();
            AddSystem<PhysicsSystem>();
            AddSystem<MoveSystem>();
            AddSystem<AnimSystem>();
            AddSystem<FSMSystem>();
            AddSystem<AutoTurnSystem>();
            AddSystem<CollideSystem>();
            //AddSystem<LuaScriptSystem>();
            //
            //AddSystem<DelayImpactSystem>();
            //初始化LuaState
            //LuaMgr.Instance.OpenLibrary(LuaTriggerLib.LIB_NAME, LuaTriggerLib.OpenLib, false);
            //LuaMgr.Instance.OpenLibrary(LuaControllerLib.LIB_NAME, LuaControllerLib.OpenLib, false);
        }
    }
}
