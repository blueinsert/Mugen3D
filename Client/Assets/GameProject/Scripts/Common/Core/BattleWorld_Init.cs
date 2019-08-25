using System.Collections;
using System.Collections.Generic;
using UniLua;

namespace bluebean.Mugen3D.Core
{
    public partial class BattleWorld : WorldBase
    {
        /// <summary>
        /// 外部依赖初始化
        /// </summary>
        /// <param name="luaFileLoader"></param>
        /// <param name="logDelegate"></param>
        /// <param name="logWarn"></param>
        /// <param name="logError"></param>
        public static void ExternalDependenceInit(CustomFileLoader luaFileLoader, LogDelegate logDelegate, LogDelegate logWarn, LogDelegate logError)
        {
            Debug.m_Log = logDelegate;
            Debug.m_LogWarn = logWarn;
            Debug.m_LogError = logError;
            //Core.Debug.Assert = Debug.Assert;
            Core.LuaMgr.AddLoader(luaFileLoader);
            //Core.FileReader.AddReader(FileRead);
        }

        /// <summary>
        /// 初始化必要的组件和系统
        /// </summary>
        private void InitNecessarySystemAndComponets()
        {
            CommandComponent.StaticInit(m_commandConfigs);
            //创建单例组件
            m_matchComponent = AddSingletonComponent<MatchComponent>();
            m_inputComponent = AddSingletonComponent<InputComponent>();
            //创建所有系统
            AddSystem<CommandSystem>();
        }
    }
}
